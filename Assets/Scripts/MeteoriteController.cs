using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class MeteoriteController : MonoBehaviour {

    int SpawnPoint1;
    int SpawnPoint2;
    int distance;
    Vector3 earthPoint = new Vector3(0, 0);

    public void Init()
    {
        SpawnPoint1 = -56;
        SpawnPoint2 = 128;
        distance = 65;
        StartCoroutine(this.SpawnMeteorites());
    }

    public IEnumerator SpawnMeteorites()
    {
        System.Random random = new System.Random();
        while(true) {
            Vector3 spawnPointRotation = new Vector3(0, 0);
            int point = random.Next(2);
            if (point == 1)
            {
                spawnPointRotation.z = SpawnPoint1;
            } else
            {
                spawnPointRotation.z = SpawnPoint2;
            }
            Vector3 direction = new Vector3(random.Next(-20, 300), random.Next(-15, 15));

            Debug.DrawRay(spawnPointRotation, direction);


            yield return new WaitForSeconds(random.Next(5, 20));
        }
    }


}
