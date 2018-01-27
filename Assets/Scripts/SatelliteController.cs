using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SatelliteController : MonoBehaviour {

    public List<Satellite> sats;
    public List<Zone> zones;

	// Use this for initialization
	void Start () {
        zones = new List<Zone>(FindObjectsOfType<Zone>());
        sats = new List<Satellite>(FindObjectsOfType<Satellite>());

        foreach (Satellite sat in sats)
        {
            sat.sc = this;
        }
        StartCoroutine(CalculateZones());
	}
	
    IEnumerator CalculateZones()
    {
        yield return new WaitForEndOfFrame();
        while (true)
        {
            Debug.Log("Calculating revenue for " + zones.Count + " zones and " + sats.Count + " satellites.");
            foreach (Satellite sat in sats)
            {
                sat.CalculateRevenue();
            }
            yield return new WaitForSeconds(1);
        }
    }

	// Update is called once per frame
	void Update () {
		
	}
}
