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
    private Transform satellitesTf;

    public List<Sprite> wingsSprites;
    public List<Sprite> bodySprites;

    public GameObject satellitePrefab;

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
        /*LayerMask mask = LayerMask.GetMask("Satellite");
        float i = 0;
        
        while (true)
        {
            float x = Mathf.Cos(i);
            float y = Mathf.Sin(i);

            Vector2 direction = new Vector2(x, y);
            direction = direction.normalized;

            Debug.DrawRay(satellitesTf.position, direction * 11, Color.blue, 20);
            RaycastHit2D hit = Physics2D.Raycast(satellitesTf.position, direction * 11, 1, mask);
            if (hit.transform == null)
            {
                Transform satelliteTf = Instantiate(satellitePrefab, satellitesTf).transform;
                Satellite sat = satelliteTf.GetComponent<Satellite>();
                //Debug.Log(i);

                sat.transform.localRotation = Quaternion.Euler(0, 0, i);

                player.addSat(satelliteTf.GetComponent<Satellite>());
                sats.Add(sat);
                sat.sc = this;
                break;
            }

            i += 10;

            if (i > 630)
                break;
        }*/

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
                    player.sats[player.currentSat].Move(new Vector2(0, -10));
                    if (player.sats[player.currentSat].distance > player.sats[player.currentSat].MaxDistance)
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
            if (a > 0.5f && !player.buyFlag)
            {
                Debug.Log(player.id + " buying");
                player.buyFlag = true;
                // add money condition
            }
            else if (a < 0.5f && player.buyFlag)
            {
                player.buyFlag = false;
            }
        }
    }
}
