using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMenuManager : MonoBehaviour {

    private int step = 0;
    public Canvas MainMenu;
    public Canvas StartMenu;
    public PlayerController playerController;

    // Use this for initialization
    void Start () {
        StartMenu.enabled = false;
        this.playerController.Init();

    }
	
	// Update is called once per frame
	void Update ()
    {
        List<Player> players = this.playerController.players;
        if (StartMenu.enabled == true)
        {
            /*
            float a = Input.GetAxis("A" + i);

            if (a > 0.1f && !player.buyFlag) ;
            //*/
        } else
        {
            for (int i = 1; i <= players.Count; i++)
            {
                float a = Input.GetAxis("Start" + i);
                if (a > 0.1f)
                {
                    StartMenu.enabled = true;
                    MainMenu.enabled = false;
                }
            }
        }

    }
    
}
