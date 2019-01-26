using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileCollider : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnTriggerEnter(Collider other) {
        var parentMissile = transform.parent.gameObject
            .GetComponent(typeof(Missile)) as Missile;
        parentMissile.OnCollision(other);
    }
    void OnTriggerExit(Collider other) {
        var parentMissile = transform.parent.gameObject
            .GetComponent(typeof(Missile)) as Missile;
        parentMissile.OnCollisionStop(other);
    }
}
