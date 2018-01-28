using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Satellite : MonoBehaviour {
    public SatelliteController sc;
    private Transform sat;
    public Player player;
    public Color color;

    public Boolean lost = false;
    public Boolean exploded = false;
    
    public Sprite explodeSprite;

    public float range = 10f;
    public float rayWidth = 0.2f;

    public float distance;
    private float minDistance = 5;
    private float maxDistance = 15;

    [Range(0, 15)]
    public float speedX = 8f;
    [Range(0, 2)]
    public float speedY = 0.8f;

    public float MaxDistance
    {
        get
        {
            return this.maxDistance;
        }
    }

    // Use this for initialization
    void Awake ()
    {
        sat = transform.Find("SatelliteSprite").transform;
        distance = sat.position.y;
	}

    public void Init()
    {
    }

    public void CalculateRevenue()
    {
        float revenues = 0;
        List<Zone> zones = sc.zones;
        Zone firstZone = null;
        Zone lastZone = null;


        Vector2 dir = sat.TransformDirection(new Vector2(-rayWidth, -1) * range);
        Debug.DrawRay(sat.position, dir, color, 1);
        LayerMask mask = LayerMask.GetMask("Default");
        RaycastHit2D hit = Physics2D.Raycast(sat.position, dir, range, mask);

        if (hit.transform != null)
        {
            firstZone = hit.transform.parent.GetComponent<Zone>();
        }

        dir = sat.TransformDirection(new Vector2(rayWidth, -1) * range);
        Debug.DrawRay(sat.position, dir, color, 1);
        hit = Physics2D.Raycast(sat.position, dir, range, mask);
        if (hit.transform != null)
        {
            lastZone = hit.transform.parent.GetComponent<Zone>();
        }

        if (firstZone == null || lastZone == null)
        {
            return;
        }

        int index = zones.IndexOf(firstZone);

        Zone currentZone = firstZone;
        int debumper = 0;
        while (true)
        {
            Vector3 start = sat.position;
            dir = currentZone.tf.position - sat.position;
            Debug.DrawRay(start, dir, Color.red, 1);

            hit = Physics2D.Raycast(start, dir, range, mask);
            if (hit.transform != null && hit.transform.parent.tag == "Zone")
            {
                Zone zone = hit.transform.parent.GetComponent<Zone>();
                zone.visitedThisFrame[player.id]++;

                if (zone.visitedThisFrame[player.id] == 1)
                    revenues += currentZone.revenue;
            }

            // Last zone checked, break
            if (currentZone == lastZone)
            {
                player.money += revenues;
                return;
            }

            // Check next zone
            ++debumper;
            ++index;
            if (index == zones.Count)
                index = 0;

            currentZone = zones[index];

            // In case of zone bug, avoid infinite loop
            if (debumper > zones.Count)
            {
                Debug.LogError("too many zones checked !");
                return;
            }
        }
    }

    public void Move(Vector2 move)
    {
        //*
        this.distance = sat.position.magnitude + move.y;
        /*
        if (lost && distance < 50)
        {
            move.y = 5;
        } else if (distance >= 50)
        {
            explode();
        }
        //*/
        if (move.y != 0)
        {
            if (this.distance >= maxDistance)
                looseSatellite();
            else if (this.distance <= minDistance)
                explode();
        }
        //*/

        transform.Rotate(-Vector3.forward * move.x * Time.deltaTime * speedX);
        sat.Translate(-Vector3.up * move.y * Time.deltaTime * speedY);
    }

    public void explode()
    {
        this.exploded = true;
        this.sat.Find("Wings").GetComponent<SpriteRenderer>().sprite = this.explodeSprite;
        this.player.satDestroyed(this);
        this.sc.sats.Remove(this);
        Destroy(gameObject);
    }

    public void looseSatellite()
    {
        this.lost = true;
    }
    

    public void setSkin(Sprite bodySprite, Sprite wingsSprite, Color color)
    {

        sat.Find("Body").GetComponent<SpriteRenderer>().sprite = bodySprite;
        sat.Find("Wings").GetComponent<SpriteRenderer>().sprite = wingsSprite;
        this.sat.Find("Cone").GetComponent<SpriteRenderer>().color = color;
    }
}
