using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class EarthController : MonoBehaviour {

    private Zone[] zones;
    public GameObject zonePrefab;
    public Transform earth;

    private int nbZones = 32;
    private int indexNorthPole = 0;
    private int indexSouthPole;


    public EarthController()
    {
        this.zones = new Zone[this.nbZones];
        this.indexNorthPole = 0;
        this.indexSouthPole = this.nbZones / 2;
    }
    private void Start()
    {
        this.earth = transform.Find("EarthSprite");
        this.GenerateZones();
    }
    // Update is called once per frame
    void Update () {
        transform.Rotate(Vector3.forward * Time.deltaTime);
    }

    public void GenerateZones()
    {
        float length = this.earth.GetComponent<CircleCollider2D>().radius / (float)this.nbZones / (float)3.2;
        // 360° / nb ZOnes (4) = 90 °
        //Géné North à South
        Vector3 rotationVector = new Vector3(0,0);
        rotationVector.z = -1;
        for (int i = this.indexNorthPole; i < this.indexSouthPole; i++ )
        {
            Zone zoneObject = Instantiate<GameObject>(this.zonePrefab).GetComponent<Zone>();
            
            zoneObject.transform.RotateAround(new Vector3(0,0), rotationVector, (float)(360.0 / (float)this.nbZones) * i);
            Vector3 scale = new Vector3(length, (float)0.01020107);
            zoneObject.transform.Find("Zone Sprite").transform.localScale = scale;
            zones[i] = zoneObject.GetComponent<Zone>();
            if (this.indexNorthPole == i)
            {
                zones[i].type = Zone.ZoneType.POLE;
            } 
        }
        //Géné South à North
        for (int i = this.indexSouthPole; i < this.nbZones; i++)
        {
            //*
            Zone zoneObject = Instantiate<GameObject>(this.zonePrefab).GetComponent<Zone>();

            zoneObject.transform.RotateAround(new Vector3(0, 0), rotationVector, (float)(360.0 / (float)this.nbZones) * i);
            Vector3 scale = new Vector3(length, (float)0.01020107);
            zoneObject.transform.Find("Zone Sprite").transform.localScale = scale;
            this.zones[i] = zoneObject.GetComponent<Zone>();
            //*/
            if (this.indexSouthPole == i)
            {
                zones[i].type = Zone.ZoneType.POLE;
            }
        }
        int index = 0;
        Array zoneTypes = Enum.GetValues(typeof(Zone.ZoneType));
        System.Random random = new System.Random();
        while (index < this.nbZones)
        {
            int nbZonesToAffect = random.Next(1, this.nbZones / 4);
            Zone.ZoneType type = (Zone.ZoneType)zoneTypes.GetValue(random.Next(zoneTypes.Length - 2));
            Debug.Log(type);
            for (int j  = 0; j < nbZonesToAffect;  j++)
            {
                index ++;
                if (index >= this.nbZones)
                {
                    break;
                }
                if (this.zones[index].type == Zone.ZoneType.NONE)
                {
                    this.zones[index].type = type;
                } else
                {
                    index++;
                    break;
                }
            }

        }
    }
}
