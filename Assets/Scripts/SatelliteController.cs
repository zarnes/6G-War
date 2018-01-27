using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SatelliteController : MonoBehaviour {

    public List<Satellite> sats;
    public List<Zone> zones;

    public List<Player> players;

    public PlayersCanvasController pcc;

	// Use this for initialization
	void Start () {
        // Generates sats and zones here

        players = new List<Player>();

        Player pl = new Player();
        pl.id = 1;
        players.Add(pl);

        sats = new List<Satellite>(FindObjectsOfType<Satellite>());
        foreach (Satellite sat in sats)
        {
            sat.sc = this;

            // placeholder
            sat.player = players[0];
            players[0].sats.Add(sat);
        }

        zones = new List<Zone>();
        foreach (Transform tfZone in GameObject.Find("Zones").transform)
        {
            zones.Add(tfZone.GetComponent<Zone>());
        }
        
        StartCoroutine(CalculateZones());
	}

    IEnumerator CalculateZones()
    {
        yield return new WaitForEndOfFrame();
        while (true)
        {
            foreach (Satellite sat in sats)
            {
                sat.CalculateRevenue();
                pcc.UpdateMoney(players);
            }
            yield return new WaitForSeconds(1);
        }
    }

	// Update is called once per frame
	void Update ()
    {
        // Manage players input
        for (int i = 1; i <= 4; ++i)
        {
            Player player = players.Find(pl => pl.id == i);
            if (player == null)
            {
                return;
            }
            
            Vector2 movement = new Vector2(Input.GetAxis("Horizontal" + i), Input.GetAxis("Vertical" + i));
            if (movement != Vector2.zero)
            {
                player.sats[player.currentSat].Move(movement);
            }
        }

    }
}
