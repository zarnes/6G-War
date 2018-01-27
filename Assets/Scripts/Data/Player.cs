using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Player
{
    public int id;

    public float money;
    public float points;

    public List<Satellite> sats;
    public int currentSat;
    public bool switchLock;

    public bool lost = false;

    public Player(int id, List<Zone> zones)
    {
        this.id = id;

        money = 0;
        points = 0;

        sats = new List<Satellite>();
        currentSat = 0;
        switchLock = false;

        foreach(Zone zone in zones)
        {
            zone.visitedThisFrame.Add(id, 0);
        }
    }

    public void SwitchSatellite(bool next = true)
    {
        Satellite oldSat = sats[currentSat];
        sats = sats.OrderBy(s => s.transform.rotation.z).ToList();
        currentSat = sats.IndexOf(oldSat);

        currentSat += next ? 1 : -1;

        if (currentSat < 0)
            currentSat = sats.Count - 1;
        else if (currentSat == sats.Count)
            currentSat = 0;

        switchLock = true;
    }

    public void looseSatellite(int index)
    {
        Satellite.Destroy(this.sats[index]);
    }


    public override string ToString()
    {
        return this.id.ToString();
    }

    public void addSat(Satellite sat)
    {
        sat.player = this;
        sat.bodySprite = sat.bodySprites[this.id];
        sat.wingsSprite = sat.wingsSprites[this.id];
        sat.Init();
        this.sats.Add(sat);
    }

    public void satDestroyed(Satellite sat)
    {
        this.sats.Remove(sat);
        if (this.sats.Count() == 0)
        {
            this.lost = true;
        }
        else
        {
            this.SwitchSatellite();
        }
    }
}
