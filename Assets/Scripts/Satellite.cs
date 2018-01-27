using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Satellite : MonoBehaviour {
    public SatelliteController sc;
    private Transform sat;
    public Color color;

    public float range = 10f;
    public float rayWidth = 0.2f;

    public float distance;
    private float minDistance = 10;
    private float maxDistance = 7;

    [Range(0, 15)]
    public float speedX = 8f;
    [Range(0, 2)]
    public float speedY = 0.8f;

    // Use this for initialization
    void Start ()
    {
        sat = transform.Find("Sprite").transform;
        distance = sat.position.y;
	}

    private void Update()
    {
        /*Debug.DrawRay(sat.position, sat.TransformDirection(new Vector2(rayWidth, -1) * range), color, Time.deltaTime);
        Debug.DrawRay(sat.position, sat.TransformDirection(new Vector2(-rayWidth, -1) * range), color, Time.deltaTime);*/
    }

    public float CalculateRevenue()
    {
        List<Zone> zones = sc.zones;
        Zone firstZone = null;
        Zone lastZone = null;

        Vector2 dir = sat.TransformDirection(new Vector2(-rayWidth, -1) * range);
        Debug.DrawRay(sat.position, dir, color, 1);
        RaycastHit2D hit = Physics2D.Raycast(sat.position, dir);
        if (hit.transform != null)
        {
            firstZone = hit.transform.GetComponent<Zone>();
        }

        dir = sat.TransformDirection(new Vector2(rayWidth, -1) * range);
        Debug.DrawRay(sat.position, dir, color, 1);
        hit = Physics2D.Raycast(sat.position, dir, range);
        if (hit.transform != null)
        {
            lastZone = hit.transform.GetComponent<Zone>();
        }

        if (firstZone == null || lastZone == null)
        {
            return 0;
        }

        int index = zones.IndexOf(firstZone);

        Zone currentZone = firstZone;
        while (true)
        {
            Vector3 start = sat.position;
            dir = currentZone.tf.position - sat.position;
            Debug.DrawRay(start, dir, Color.red, 1);

            // Last zone checked, break
            if (currentZone == lastZone)
                break;

            // Check next zone
            ++index;
            if (index == zones.Count)
                index = 0;
        }

        /*foreach (Zone zone in zones)
        {
            Vector3 start = sat.position;
            Vector3 dir =zone.tf.position - sat.position;
            //Physics2D.raycast(sat.position, sat.position - zone.transform.position, range)
            Debug.DrawRay(start, dir, Color.red, 1);
        }*/
        return 0;
    }

    private void CalculateCovering()
    {
        List<Satellite> sats = sc.sats;
        {

        }
    }

    public void Move(Vector2 move)
    {
        //distance += move.y;
        /*if (distance + move.y > maxDistance)
            move.y = maxDistance - distance;
        /*else if (distance + move.y < minDistance)
            move.y = minDistance - distance;*/
        

        transform.Rotate(-Vector3.forward * move.x * Time.deltaTime * speedX);
        sat.Translate(Vector3.up * move.y * Time.deltaTime * speedY);

        distance = sat.transform.position.y;
    }
}
