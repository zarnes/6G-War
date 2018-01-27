using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public int id;

    public float money;
    public float points;

    public List<Satellite> sats;
    public int currentSat;

    public Player()
    {
        id = 0;

        money = 0;
        points = 0;

        sats = new List<Satellite>();
        currentSat = 0;
    }
}
