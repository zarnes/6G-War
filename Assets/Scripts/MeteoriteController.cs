using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class MeteoriteController : MonoBehaviour {

    int SpawnPoint1;
    int SpawnPoint2;
    int distance;
    Vector3 earthPoint = new Vector3(0, 0);

    public GameObject meteorePrefab;
    List<GameObject> meteores;

    public void Init()
    {
        SpawnPoint1 = -56;
        SpawnPoint2 = 128;
        distance = 130;
        this.meteores = new List<GameObject>();
        StartCoroutine(this.SpawnMeteorites());
    }

    public IEnumerator SpawnMeteorites()
    {
        System.Random random = new System.Random();
        while(true)
        {
            Vector3 target = new Vector3(random.Next(-20, 30), random.Next(-15, 15));
            Vector3 spawnPointRotation = new Vector3(0, 0);
            int point = random.Next(2);
            if (point == 1)
            {
                spawnPointRotation.z = SpawnPoint1;
            } else
            {
                spawnPointRotation.z = SpawnPoint2;
            }
            GameObject meteore = Instantiate(this.meteorePrefab);
            meteore.transform.position = target;
            Vector3 zAxis = new Vector3(0, 0, 1);
            meteore.transform.RotateAround(target, zAxis, spawnPointRotation.z);
            Vector3 scale = meteore.transform.localScale;
            Vector3 meteorePoint = new Vector3(0, this.distance, 3);
            meteore.transform.Find("MeteoreSprite").transform.position = meteorePoint;






            this.meteores.Add(meteore);

            int time = random.Next(3, 10);


            yield return new WaitForSeconds(time);
        }
    }

    public void Update()
    {
        if (this.meteores == null)
        {
            return;
        }
        for (int i = 0; i < this.meteores.Count; i++)
        {
            if (this.meteores[i] == null)
            {
                this.meteores.Remove(this.meteores[i]);
            }
            GameObject meteore = this.meteores[i];
            Vector3 position = meteore.transform.Find("MeteoreSprite").transform.localPosition;
            position.y += -0.35f;
            position.x = 0;
            meteore.transform.Find("MeteoreSprite").transform.localPosition = position;
            if (position.y < -200)
            {
                Destroy(meteore);
                this.meteores.Remove(meteore);
            }
        }
    }


}
