using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zone : MonoBehaviour {

    public float revenue = 1;
    public Transform tf;
    public Dictionary<int, int> visitedThisFrame;

    public enum ZoneType { EARTH, SEA, POLE, NONE };
    private ZoneType Type = ZoneType.NONE;

    public Sprite spriteEarth;
    public Sprite spritePole;
    public Sprite spriteSea;
    public Sprite spriteDefault;

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
                    color = Color.red;
                    sprite = this.spriteEarth;
                    break;
                case ZoneType.POLE:
                    color = Color.white;
                    sprite = this.spritePole;
                    break;
                case ZoneType.SEA:
                    color = Color.blue;
                    sprite = this.spriteSea;
                    break;
                default:
                    color = Color.black;
                    sprite = this.spriteDefault;
                    break;

            }
            transform.Find("Zone Sprite").transform.GetComponent<SpriteRenderer>().sprite = sprite;
        }
    }

    private void Start()
    {
        tf = transform.Find("Zone Sprite").transform;
        visitedThisFrame = new Dictionary<int, int>();
    }
}
