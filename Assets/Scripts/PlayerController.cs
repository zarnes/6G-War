using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

    bool[] controllersConnected = { true, false, false, false };
    bool[] playersPlaying = { true, false, false, false };

    int[] playersCores = { 0, 0, 0, 0 };
    int[] playersWings = { 0, 0, 0, 0 };

    public List<Sprite> cores;
    public List<Sprite> wings;
    // todo color ?

    public List<Player> players;

	// Use this for initialization
	void Start ()
    {
        if (GameObject.FindGameObjectsWithTag("PlayerController").Length > 1)
            Destroy(gameObject);
        else
            DontDestroyOnLoad(gameObject);

        //Placeholder TODO degager
        controllersConnected = new bool[] { true, true, false, false };
        playersPlaying = new bool[] { true, true, false, false };

        playersCores[0] = 1;
        playersWings[0] = 1;

        StartOrQuit(1);
    }

    public void StartOrQuit(int playerIndex)
    {
        if (playerIndex == 1 && controllersConnected[0] && playersPlaying[0])
        {
            players = new List<Player>();
            for (int i = 0; i < 4; ++i)
            {
                if (playersPlaying[i])
                {
                    Player player = new Player(i + 1, cores[playersCores[i]], wings[playersWings[i]], Color.blue);
                    players.Add(player);
                }
            }

            if (players.Count >= 2)
            {
                SceneManager.LoadScene("main");
            }
        }

        playersPlaying[playerIndex - 1] = !playersPlaying[playerIndex - 1];
    }

    public void ChangeCore(int player, int direction)
    {
        playersCores[player] += direction;

        if (playersCores[player] == cores.Count)
            playersCores[player] = 0;
        else if (playersCores[player] < 0)
            playersCores[player] = cores.Count - 1;
    }

    public void ChangeWings(int player, int direction)
    {
        playersWings[player] += direction;

        if (playersWings[player] == wings.Count)
            playersWings[player] = 0;
        else if (playersWings[player] < 0)
            playersWings[player] = wings.Count - 1;
    }
}
