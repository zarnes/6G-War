﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SatelliteController : MonoBehaviour {

    public List<Satellite> sats;
    public List<Zone> zones;

    public List<Player> players;

    public CanvasController canvasController;
    public ContractController contractController;

    public List<Sprite> wingsSprites;
    public List<Sprite> bodySprites;

    public int maxDistance = 13;
    public int lostDistance = 30;

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
        pl.color = new Color(255, 0, 0, 0.37f);
        pl.bodySprite = bodySprites[pl.id];
        pl.wingsSprite = wingsSprites[pl.id];
        players.Add(pl);

        pl = new Player(2, zones);
        pl.color = new Color(0, 255, 0, 0.37f);
        pl.bodySprite = bodySprites[pl.id];
        pl.wingsSprite = wingsSprites[pl.id];
        players.Add(pl);
        pl = new Player(3, zones);
        pl.color = new Color(0, 0, 255, 0.37f);
        pl.bodySprite = bodySprites[pl.id];
        pl.wingsSprite = wingsSprites[pl.id];
        players.Add(pl);
        //pl = new Player(4, zones);
        //players.Add(pl);

        sats = new List<Satellite>(FindObjectsOfType<Satellite>());
        foreach (Satellite sat in sats)
        {
            sat.sc = this;

            // placeholder
            /*sat.player = players[0];
            players[0].sats.Add(sat);*/
        }

        //placeholder
        players[0].addSat(sats[0]);
        players[0].addSat(sats[1]);
        players[0].addSat(sats[3]);
        players[1].addSat(sats[2]);
        players[2].addSat(sats[4]);
        //players[3].addSat(sats[5]);
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
            if (player.lost)
            {
                continue;
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
                    player.sats[player.currentSat].Move(new Vector2(0, -5));
                    if (player.sats[player.currentSat].distance > this.lostDistance)
                    {
                        player.satDestroyed(player.sats[player.currentSat]);
                    }
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
