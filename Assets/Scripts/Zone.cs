using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Zone : MonoBehaviour {

    public float revenue = 1;
    public Transform tf;
    public Dictionary<int, int> visitedThisFrame;
    private UnityEngine.Random rand = new UnityEngine.Random();

    public enum ZoneType { EARTH, SEA, POLE, NONE };
    public enum EarthZoneType { SAND, GRASS, TOWN, MOUNTAIN };
    private ZoneType Type = ZoneType.NONE;
    private EarthZoneType earthType = EarthZoneType.GRASS;

    public float lowestLimit = 11;
    public float lowerLimit = 21;
    public float upperLimit = 90;
    public bool withBonus= false;

    public Sprite spritePole;
    public Sprite spriteSea;

    public Sprite spriteGrass;
    public Sprite spriteSand;
    public Sprite spriteTown;
    public Sprite spriteMountain;
    public Sprite spriteBonusCity;
    public Sprite spriteBonusPlatform;

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
        int value = (int)(UnityEngine.Random.value * 100f);
        Debug.Log(value);
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
            this.revenue = 6f;
            sprite = this.spriteTown;
        }
        return sprite;
    }
    public void setBonus()
    {
        this.revenue *= 4f;
        SpriteRenderer renderer = this.transform.Find("bonusSprite").gameObject.GetComponent<SpriteRenderer>();
        renderer.enabled = true;
        if (this.Type == ZoneType.SEA)
        {
            renderer.sprite = this.spriteBonusPlatform;
        } else
        {
            renderer.sprite = this.spriteBonusCity;
        }
        this.withBonus = true;
    }
    private void Awake()
    {
        tf = transform.Find("Zone Sprite").transform;
        visitedThisFrame = new Dictionary<int, int>();
    }

    public void destroy()
    {
        if (this.withBonus)
        {
            this.revenue /= 4f;
            this.transform.Find("bonusSprite").gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }
        if (this.Type == ZoneType.POLE)
        {
            this.type = ZoneType.SEA;
        }
        else if (this.Type == ZoneType.SEA || this.Type == ZoneType.EARTH)
        {
            this.revenue = 2f;
            transform.Find("Zone Sprite").transform.GetComponent<SpriteRenderer>().sprite = this.spriteSand;
        }
    }
}
