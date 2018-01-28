using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteoreBehavior : MonoBehaviour {

    // Use this for initialization
    private void OnTriggerEnter2D(Collider2D other)
    {
        Meteore parent = this.transform.parent.GetComponent<Meteore>();
        if (other.gameObject.name == "Zone Sprite")
        {
            parent.destroy();
        }
    }
}
