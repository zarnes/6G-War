using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayersCanvasController : MonoBehaviour
{
    List<Text> names;
    List<Text> money;

	void Start ()
    {
        names = new List<Text>();
        money = new List<Text>();

        foreach (Transform playerTf in transform.Find("SidePanel"))
        {
            names.Add(playerTf.Find("Name").GetComponent<Text>());
            money.Add(playerTf.Find("Money").GetComponent<Text>());
        }
    }

    public void UpdateMoney(List<Player> players)
    {
        for (int i = 0; i < players.Count; ++i)
        {
            money[i].text = players[i].money.ToString() + "$";
        }
    }
}
