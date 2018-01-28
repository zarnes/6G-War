using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SatelliteBehavior : MonoBehaviour {
    
    public void OnTriggerEnter2D(Collider2D other)
    {
        Satellite parent = this.transform.parent.GetComponent<Satellite>();
        if (other.gameObject.name == "SatelliteSprite")
        {
            Satellite otherParent = other.gameObject.transform.parent.GetComponent<Satellite>();
            if (! otherParent.exploded)
            {
                otherParent.explode();
            }
            if (! parent.exploded)
            {
                parent.explode();
            }
        } else if (other.gameObject.name == "Zone Sprite")
        {
            parent.explode();
        } else
        {
            Debug.Log(other.gameObject.name);
        }
    }
}
