using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zone : MonoBehaviour {

    public float revenue = 1;
    public Transform tf;
    public Dictionary<int, int> visitedThisFrame;

    public enum ZoneType { EARTH, SEA, POLE, NONE };
    private ZoneType Type = ZoneType.NONE;

    public Color colorEarth = Color.red;
    public Color colorPole = Color.white;
    public Color colorSea = Color.blue;

    public ZoneType type
    {
        get
        {
            return this.Type;
        }
        set
        {
            this.Type = value;
            Color color = Color.black;
            switch (value)
            {
                case ZoneType.EARTH:
                    color = this.colorEarth;
                    break;
                case ZoneType.POLE:
                    color = this.colorPole;
                    break;
                case ZoneType.SEA:
                    color = this.colorSea;
                    break;

            }
            transform.Find("Zone Sprite").transform.GetComponent<SpriteRenderer>().color = color;
        }
    }

    private void Awake()
    {
        tf = transform.Find("Zone Sprite").transform;
        visitedThisFrame = new Dictionary<int, int>();
    }
}
