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

    public Player()
    {
        id = 0;

        money = 0;
        points = 0;

        sats = new List<Satellite>();
        currentSat = 0;
        switchLock = false;
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
}
