using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    Vector3 initialPosition;
    // Start is called before the first frame update
    void Start()
    {
        initialPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Find the missile object
        var missile = GameObject.Find("Missile");
        if(missile != null) {
            var missileComponent = missile.GetComponent(typeof(Missile)) as Missile;
            var distance = missileComponent.CameraDistanceRatio();
        }
    }
}
