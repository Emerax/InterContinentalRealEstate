using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    Vector3 initialPosition;
    Vector3 initialRotation;
    // Start is called before the first frame update
    void Start()
    {
        initialPosition = transform.position;
        initialRotation = transform.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        // Find the missile object
        var missile = GameObject.FindWithTag("Missile");
        
        if(missile != null) {
            var missileComponent = missile.GetComponent(typeof(Missile)) as Missile;
            var distance = (float) System.Math.Sqrt(
                System.Math.Sqrt(missileComponent.CameraDistanceRatio())
            );
            var targetPosition = missile.transform.Find("CameraSeat").transform.position;

            transform.position = initialPosition * (distance)
                     + targetPosition * (1 - distance);
            transform.eulerAngles = initialRotation * (distance)
                     + missile.transform.eulerAngles * (1 - distance);
        }
        else {
            transform.position = initialPosition;
            transform.eulerAngles = initialRotation;
        }
    }
}
