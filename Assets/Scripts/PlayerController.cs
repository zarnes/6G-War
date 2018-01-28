using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    bool[] controllersConnected = { false, false, false, false };
    bool[] playersPlaying = { false, false, false, false };

    int[] playersCores = { 0, 0, 0, 0 };
    int[] playersWings = { 0, 0, 0, 0 };

    public List<Sprite> cores;
    public List<Sprite> wings;
    // todo color ?

	// Use this for initialization
	void Start ()
    {
        if (GameObject.FindGameObjectsWithTag("PlayerController").Length > 1)
            Destroy(gameObject);
        else
            DontDestroyOnLoad(gameObject);
	}

    public void StartOrQuit(int player)
    {
        if (player == 1)
        {
            // TODO create player and load scene
        }

        playersPlaying[player - 1] = !playersPlaying[player - 1];
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
