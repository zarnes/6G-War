using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public SatelliteController satelliteController;
    public EarthController earthController;


    // Use this for initialization
    void Start () {
        this.earthController = new EarthController();
        this.satelliteController = new SatelliteController();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
