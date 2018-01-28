using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    public EarthController earthController;
    public SatelliteController satelliteController;
    public CanvasController canvasController;
    public ContractController contractController;

    // Use this for initialization
    void Start ()
    {
        if (GameObject.FindGameObjectWithTag("PlayerController") == null)
        {
            SceneManager.LoadScene("MainMenu");
            return;
        }

        earthController.Init();

        List<Zone> zones = new List<Zone>();
        foreach (Transform tfZone in earthController.transform.Find("Zones"))
        {
            zones.Add(tfZone.GetComponent<Zone>());
        }
        satelliteController.zones = zones;
        satelliteController.players = GameObject.FindGameObjectWithTag("PlayerController").GetComponent<PlayerController>().players;

        satelliteController.Init();
        canvasController.Init();
        
        /*foreach (Zone zone in zones)
        {
            zone.visitedThisFrame = new Dictionary<int, int>();
            foreach(Player player in satelliteController.players)
            {
                zone.visitedThisFrame.Add(player.id, 0);
            }
        }*/

        contractController.sc = satelliteController;
        contractController.CreateContract();

        //Placeholder ? TODO
        foreach(Player player in satelliteController.players)
        {
            satelliteController.SpawnSatellite(player);
            satelliteController.SpawnSatellite(player);
            satelliteController.SpawnSatellite(player);
        }

        satelliteController.StartCoroutine(satelliteController.CalculateZones());
    }

    IEnumerator waitFrame()
    {
        yield return new WaitForSeconds(1);
    }
}
