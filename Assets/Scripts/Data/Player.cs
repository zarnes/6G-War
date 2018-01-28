using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Player
{
    public int id;

    public float money;
    public float points;
    public int satellitesBought = 1;

    public List<Satellite> sats;
    public int currentSat;
    public bool switchLock;
    public bool buyFlag;

    public Color color;
    public Sprite bodySprite;
    public Sprite wingsSprite;

    public bool lost = false;

    public Player(int id, Sprite bodySprite, Sprite wingsSprite, Color color)
    {
        this.id = id;
        this.bodySprite = bodySprite;
        this.wingsSprite = wingsSprite;
        this.color = color;

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

    public void LoseSatellite(int index)
    {
        Object.Destroy(sats[index]);
    }


    public override string ToString()
    {
        return id.ToString();
    }

    public void addSat(Satellite sat)
    {
        sat.player = this;
        sat.setSkin(bodySprite, wingsSprite, color);
        sats.Add(sat);
    }

    public void satDestroyed(Satellite sat)
    {
        SwitchSatellite(false);
        sats.Remove(sat);
        if (sats.Count == 0)
        {
            lost = true;
        }
        if (currentSat == sats.Count)
        {
            currentSat--;
        }
    }
}
