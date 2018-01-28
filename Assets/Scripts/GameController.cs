using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    public EarthController earthController;
    public SatelliteController satelliteController;
    public CanvasController canvasController;
    public ContractController contractController;
    public MeteoriteController meteoriteController;
    public PlayerController playerController;

    // Use this for initialization
    void Start ()
    {
        if (GameObject.FindGameObjectWithTag("PlayerController") == null)
        {
            SceneManager.LoadScene("MainMenu");
            return;
        }

        playerController = GameObject.FindGameObjectWithTag("PlayerController").GetComponent<PlayerController>();

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

        List<Sprite> cores = new List<Sprite>();
        List<Sprite> wings = new List<Sprite>();
        foreach (Player pl in satelliteController.players)
        {
            cores.Add(pl.bodySprite);
            wings.Add(pl.wingsSprite);
        }
        canvasController.SetSprites(cores, wings);
        
        meteoriteController.Init();

        contractController.sc = satelliteController;

        //Placeholder ? TODO
        foreach(Player player in satelliteController.players)
        {
            satelliteController.SpawnSatellite(player);
            satelliteController.SpawnSatellite(player);
            satelliteController.SpawnSatellite(player);
        }

        satelliteController.StartCoroutine(satelliteController.CalculateZones());
        contractController.CreateContract();
    }

    IEnumerator waitFrame()
    {
        yield return new WaitForSeconds(1);
    }
}
