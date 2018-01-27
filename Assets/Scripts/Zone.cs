using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zone : MonoBehaviour {

    public float revenue = 1;
    public Transform tf;
    public Dictionary<int, int> visitedThisFrame;

    private void Start()
    {
        tf = transform.Find("Zone Sprite").transform;
        visitedThisFrame = new Dictionary<int, int>();
    }
}
