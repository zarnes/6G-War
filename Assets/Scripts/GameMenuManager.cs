using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMenuManager : MonoBehaviour {

    private int step = 0;
    public Canvas MainMenu;
    public Canvas StartMenu;

    // Use this for initialization
    void Start () {
        StartMenu.enabled = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown("space"))
        {
            StartMenu.enabled = true;
            if (step == 0)
            {
                step = 1;
                MainMenu.enabled = false;
            }
        }

    }
    
}
