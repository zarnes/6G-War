using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zone : MonoBehaviour {

    public float revenue = 1;
    public Transform tf;

    private void Start()
    {
        tf = transform.Find("Zone Sprite").transform;
    }
}
