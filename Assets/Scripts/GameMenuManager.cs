using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMenuManager : MonoBehaviour {

    private int step = 0;
    public Canvas MainMenu;
    public Canvas StartMenu;
    public Canvas CreditMenu;
    public PlayerController playerController;

    public List<Sprite> sprites;
    public List<Sprite> bodySprites;
    public List<Sprite> wingsSprites;

    // Use this for initialization
    void Start ()
    {
        CreditMenu.enabled = false;
        StartMenu.enabled = false;
        MainMenu.enabled = true;
        this.playerController.Init();

    }
	
	// Update is called once per frame
	void Update ()
    {
        List<Player> players = this.playerController.players;
        if (StartMenu.enabled == true)
        {
            for (int i = 1; i <= players.Count; i++)
            {
                /**
                float a = Input.GetAxis("A" + i);

                if (a > 0.1f)
                {
                    players[i - 1].bodySprite = this.bodySprites[];
                }
                this.transform.Find("Body" + i).GetComponent<Image>().sprite = players[i - 1].bodySprite;
                this.transform.Find("Wings" + i).GetComponent<Image>().sprite = players[i - 1].wingsSprite;
    //*/
            }

            if (Input.GetAxis("Start") > 0.1f)
            {
                SceneManager.LoadScene("main");
            }
        }
        else
        {
            for (int i = 1; i <= players.Count; i++)
            {
                float a = Input.GetAxis("A" + i);
                if (a > 0.1f)
                {
                    StartMenu.enabled = true;
                    MainMenu.enabled = false;
                    CreditMenu.enabled = false;
                }
                float b = Input.GetAxis("B" + i);
                if (b > 0.2f )
                {
                    CreditMenu.enabled = !CreditMenu.enabled;
                    StartMenu.enabled = false;
                    MainMenu.enabled = !MainMenu.enabled;
                } 
            }
        }

    }
    
}
