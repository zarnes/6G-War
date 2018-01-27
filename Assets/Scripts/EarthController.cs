using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class EarthController : MonoBehaviour {

    private Zone[] zones;
    public GameObject zonePrefab;
    public Transform earth;

    public int nbZones;
    private int indexNorthPole = 0;
    private int indexSouthPole;


    public void Init()
    {
        this.zones = new Zone[this.nbZones];
        this.indexNorthPole = 0;
        this.indexSouthPole = this.nbZones / 2;
        this.earth = transform.Find("EarthSprite");
        this.GenerateZones();
    }

    // Update is called once per frame
    void Update ()
    {
        transform.Rotate(Vector3.forward * Time.deltaTime);
    }

    public void GenerateZones()
    {
        float length = (this.earth.GetComponent<CircleCollider2D>().radius * (float)7.5) / (float)this.nbZones;

        Transform zonesParent = transform.Find("Zones");
        this.generatePart(this.indexNorthPole, this.indexSouthPole, length, zonesParent);
        this.generatePart(this.indexSouthPole, this.nbZones, length, zonesParent);
        this.assignTypes();
    }

    private void generatePart(int start, int end, float length, Transform zonesParent)
    {
        Vector3 rotationVector = new Vector3(0, 0);
        rotationVector.z = -1;
        for (int i = start; i < end; i++)
        {
            Zone zoneObject = Instantiate(this.zonePrefab, zonesParent).GetComponent<Zone>();
            
            zoneObject.transform.RotateAround(new Vector3(0,0), rotationVector, (float)(360.0 / (float)this.nbZones) * i);
            Vector3 scale = new Vector3(length, (float)0.01020107);
            zoneObject.transform.Find("Zone Sprite").transform.localScale = scale;
            Vector3 position = new Vector3(0, this.earth.GetComponent<CircleCollider2D>().radius * this.earth.transform.localScale.y);
            zoneObject.transform.Find("Zone Sprite").transform.localPosition = position;
            zones[i] = zoneObject.GetComponent<Zone>();
            if (start == i)
            {
                zones[i].type = Zone.ZoneType.POLE;
            } 
        }
    }

    private void assignTypes ()
    {
        int index = 0;
        Array zoneTypes = Enum.GetValues(typeof(Zone.ZoneType));
        System.Random random = new System.Random();
        while (index < this.nbZones)
        {
            int nbZonesToAffect = random.Next(1, this.nbZones / 4);
            Zone.ZoneType type = (Zone.ZoneType)zoneTypes.GetValue(random.Next(zoneTypes.Length - 2));
            for (int j  = 0; j < nbZonesToAffect;  j++)
            {
                index++;
                if (index >= this.nbZones)
                {
                    break;
                }
                if (this.zones[index].type == Zone.ZoneType.NONE)
                {
                    this.zones[index].type = type;
                }
                else
                {
                    break;
                }
            }

        }
    }
}
