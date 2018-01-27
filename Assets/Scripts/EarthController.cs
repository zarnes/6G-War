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

    
    private void Start()
    {
        this.earth = transform.Find("EarthSprite");
        this.indexNorthPole = 0;
        this.indexSouthPole = this.nbZones / 2;
        this.zones = new Zone[this.nbZones];
        this.GenerateZones();
    }
    // Update is called once per frame
    void Update () {
        transform.Rotate(Vector3.forward * Time.deltaTime);
    }

    public void GenerateZones()
    {
        //float length = (this.earth.GetComponent<CircleCollider2D>().radius * this.earth.transform.localScale.y) / (float)this.nbZones;
        float length = (this.earth.GetComponent<CircleCollider2D>().radius * (float)7.5) / (float)this.nbZones;
        // 360° / nb ZOnes (4) = 90 °
        //Géné North à South
        GameObject zonesParent = transform.Find("Zones").gameObject;
        this.generatePart(this.indexNorthPole, this.indexSouthPole, length, zonesParent);
        this.generatePart(this.indexSouthPole, this.nbZones, length, zonesParent);
        this.assignTypes();
    }

    private void generatePart(int start, int end, float length, GameObject zonesParent)
    {
        Vector3 rotationVector = new Vector3(0, 0);
        rotationVector.z = -1;
        //Géné South à North
        for (int i = start; i < end; i++)
        {
            Zone zoneObject = Instantiate<GameObject>(this.zonePrefab).GetComponent<Zone>();

            zoneObject.transform.RotateAround(new Vector3(0, 0), rotationVector, (float)(360.0 / (float)this.nbZones) * i);
            Vector3 scale = new Vector3(length, (float)0.65);
            zoneObject.transform.Find("Zone Sprite").transform.localScale = scale;
            Vector3 position = new Vector3(0, this.earth.GetComponent<CircleCollider2D>().radius * this.earth.transform.localScale.y);
            zoneObject.transform.Find("Zone Sprite").transform.localPosition = position;
            this.zones[i] = zoneObject.GetComponent<Zone>();
            if (start == i)
            {
                this.zones[i].type = Zone.ZoneType.POLE;
            }
            this.zones[i].transform.parent = zonesParent.transform;
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
            Debug.Log(type);
            for (int j = 0; j < nbZonesToAffect; j++)
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
