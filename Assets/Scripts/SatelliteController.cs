using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SatelliteController : MonoBehaviour {

    public List<Satellite> sats;
    public List<Zone> zones;

    public List<Player> players;

    public CanvasController canvasController;
    public ContractController contractController;

    // Use this for initialization
    public void Init() {
        // Generates sats and zones here

        zones = new List<Zone>();
        foreach (Transform tfZone in GameObject.Find("Zones").transform)
        {
            zones.Add(tfZone.GetComponent<Zone>());
        }

        players = new List<Player>();

        Player pl = new Player(1, zones);
        players.Add(pl);

        pl = new Player(2, zones);
        players.Add(pl);

        sats = new List<Satellite>(FindObjectsOfType<Satellite>());
        foreach (Satellite sat in sats)
        {
            sat.sc = this;

            // placeholder
            /*sat.player = players[0];
            players[0].sats.Add(sat);*/
        }

        //placeholder
        sats[0].player = players[0];
        players[0].sats.Add(sats[0]);
        sats[1].player = players[0];
        players[0].sats.Add(sats[1]);
        sats[2].player = players[1];
        players[1].sats.Add(sats[2]);
        sats[3].player = players[0];
        players[0].sats.Add(sats[3]);
	}

    public IEnumerator CalculateZones()
    {
        while (true)
        {
            foreach (Zone zone in zones)
            {
                zone.visitedThisFrame = new Dictionary<int, int>();
                foreach(Player player in players)
                {
                    zone.visitedThisFrame[player.id] = 0;
                }
            }

            foreach (Satellite sat in sats)
            {
                sat.CalculateRevenue();
                canvasController.UpdateMoney(players);
            }

            contractController.CheckContract();

            yield return new WaitForSeconds(1);
        }
    }

	// Update is called once per frame
	void Update ()
    {
        
        // To avoir update before start
        if (players == null)
            return;
        

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
            //*
            for (int j = 0; j < player.sats.Count; j++)
            {
                if (player.sats[j].lost == true)
                {
                    player.sats[player.currentSat].Move(new Vector2(0, -10));
                    if (player.sats[player.currentSat].distance > player.sats[player.currentSat].MaxDistance)
                    {
                        player.looseSatellite(j);
                    }
                }
                if (player.sats[j].exploded == true)
                {
                    player.looseSatellite(j);
                }
            }
            //*/

            // Handle satellite switching
            float switchSat = Input.GetAxis("SwitchLeft" + i);
            if (switchSat == 1 && !player.switchLock)
                player.SwitchSatellite();
            else if (switchSat == -1 && !player.switchLock)
                player.SwitchSatellite(false);
            else if (switchSat == 0 && player.switchLock)
                player.switchLock = false;
        }
    }
}
