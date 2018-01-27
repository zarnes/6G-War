using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour {

    public Satellite sat;
	
	// Update is called once per frame
	void Update ()
    {
        for (int i = 1; i <= 4; ++i)
        {
            Debug.Log(i + " : horizontal : " + Input.GetAxis("Horizontal" + i) + ", vertical : " + Input.GetAxis("Vertical" + i));
        }

        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        sat.Move(new Vector2(x, y));
    }
}
