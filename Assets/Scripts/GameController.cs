using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public EarthController earthController;
    public SatelliteController satelliteController;
    public CanvasController canvasController;
    public ContractController contractController;

    // Use this for initialization
    void Start ()
    {
        earthController.Init();
        satelliteController.Init();
        canvasController.Init();

        contractController.sc = satelliteController;
        contractController.CreateContract();

        satelliteController.StartCoroutine(satelliteController.CalculateZones());
    }

    IEnumerator waitFrame()
    {
        yield return new WaitForSeconds(1);
    }
}
