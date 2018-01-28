using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SatelliteController : MonoBehaviour {

    public float multiplier;
    public float satPrice;

    public List<Satellite> sats;
    public List<Zone> zones;

    public List<Player> players;

    public CanvasController canvasController;
    public ContractController contractController;
    private Transform satellitesTf;

    public List<Sprite> wingsSprites;
    public List<Sprite> bodySprites;
    
    public GameObject satellitePrefab;
    public int maxDistance = 13;
    public int lostDistance = 30;
    
    // Generates sats and zones here
    private System.Random rnd;

    // Use this for initialization
    public void Init()
    {
        satellitesTf = GameObject.Find("Earth").transform.Find("Satellites");
        rnd = new System.Random();
        /*players = new List<Player>();
        
        //pl = new Player(4, zones);
        //players.Add(pl);

        sats = new List<Satellite>(FindObjectsOfType<Satellite>());
        foreach (Satellite sat in sats)
        {
            sat.sc = this;

            // placeholder
            /*sat.player = players[0];
            players[0].sats.Add(sat);
        }

        //placeholder
        players[0].addSat(sats[0]);
        players[0].addSat(sats[1]);
        players[0].addSat(sats[3]);
        players[1].addSat(sats[2]);
        players[2].addSat(sats[4]);*/
        //players[3].addSat(sats[5]);
    }

    public IEnumerator CalculateZones()
    {
        while (true)
        {
            int lost = 0;
            foreach(Player pl in players)
            {
                if (pl.points == 10)
                {
                    StartCoroutine(canvasController.FinalWin(pl.id - 1));
                }

                if (pl.lost)
                    ++lost;
            }

            if (lost == players.Count)
            {
                StartCoroutine(canvasController.FinalLose());
                break;
            }

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

    public void SpawnSatellite(Player player)
    {
        float placingAngle;

        if (sats.Count == 0)
        {
            placingAngle = 0;
        }
        else
        {
            List<float> angles = new List<float>();
            foreach (Satellite sat in sats)
            {
                angles.Add(sat.transform.localEulerAngles.z);
            }

            angles.Sort((a1, a2) => a1.CompareTo(a2));

            int indexBetterPlacement = 0;
            float spaceBetterPlacement = 0;

            for (int i = 1; i < angles.Count; ++i)
            {
                float space = angles[i] - angles[i - 1];

                if (space > spaceBetterPlacement)
                {
                    indexBetterPlacement = i - 1;
                    spaceBetterPlacement = space;
                }
            }

            // if only 1 other satellite
            if (spaceBetterPlacement == 0)
                placingAngle = 300;
            else
                placingAngle = angles[indexBetterPlacement] + (spaceBetterPlacement / 2);
        }

        Transform satelliteTf = Instantiate(satellitePrefab, satellitesTf).transform;
        satelliteTf.localEulerAngles = new Vector3(0, 0, placingAngle);
        Satellite newSat = satelliteTf.GetComponent<Satellite>();

        player.addSat(satelliteTf.GetComponent<Satellite>());
        sats.Add(newSat);
        newSat.sc = this;

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


            // A button
            float a = Input.GetAxis("A" + i);

            if (a > 0.1f && !player.buyFlag)
            {
                float price = player.satellitesBought * satPrice * multiplier;
                if (player.money >= price)
                {
                    player.buyFlag = true;
                    player.money -= price;
                    player.satellitesBought++;
                    SpawnSatellite(player);
                    canvasController.UpdateMoney(players);
                    canvasController.UpdatePrice(player.id - 1, (int)(player.satellitesBought * satPrice * multiplier));
                }
            }
            else if (a < 0.1f && player.buyFlag)
            {
                player.buyFlag = false;
            }
        }
    }
}
