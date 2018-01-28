using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Zone : MonoBehaviour {

    public float revenue = 1;
    public Transform tf;
    public Dictionary<int, int> visitedThisFrame;

    public enum ZoneType { EARTH, SEA, POLE, NONE };
    public enum EarthZoneType { SAND, GRASS, TOWN, MOUNTAIN };
    private ZoneType Type = ZoneType.NONE;
    private EarthZoneType earthType = EarthZoneType.GRASS;

    public float lowestLimit = 11;
    public float lowerLimit = 21;
    public float upperLimit = 90;

    public Sprite spritePole;
    public Sprite spriteSea;

    public Sprite spriteGrass;
    public Sprite spriteSand;
    public Sprite spriteTown;
    public Sprite spriteMountain;

    public ZoneType type
    {
        get
        {
            return this.Type;
        }
        set
        {
            this.Type = value;
            Color color;
            Sprite sprite;
            switch (value)
            {
                case ZoneType.EARTH:
                    sprite = this.setEarth();
                    break;
                case ZoneType.POLE:
                    revenue = 1f;
                    sprite = this.spritePole;
                    break;
                case ZoneType.SEA:
                    revenue = 2f;
                    sprite = this.spriteSea;
                    break;
                default:
                    revenue = 1f;
                    sprite = this.spritePole;
                    break;
            }
            transform.Find("Zone Sprite").transform.GetComponent<SpriteRenderer>().sprite = sprite;
        }
    }

    private Sprite setEarth()
    {
        Sprite sprite;
        System.Random rand = new System.Random();
        int value = rand.Next(101);
        if (value < this.lowestLimit)
        {
            this.revenue = 2f;
            sprite = this.spriteSand;
        } else if (value < this.lowerLimit)
        {
            this.revenue = 2f;
            sprite = this.spriteMountain;
        } else if (value < this.upperLimit)
        {
            this.revenue = 4f;
            sprite = this.spriteGrass;
        } else
        {
            this.revenue = 8f;
            sprite = this.spriteTown;
        }
        return sprite;
    }

    private void Awake()
    {
        tf = transform.Find("Zone Sprite").transform;
        visitedThisFrame = new Dictionary<int, int>();
    }
}
