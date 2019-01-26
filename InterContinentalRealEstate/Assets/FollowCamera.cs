using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    Vector3 crashPosition;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // Find the missile object
        var missile = GameObject.FindWithTag("Missile");
        var player = GameObject.Find("Player");
        
        var playerPosition = player.transform.position;
        var playerRotation = player.transform.eulerAngles;
        var playerRotationQuat = player.transform.rotation;
        if(missile != null) {
            var missileComponent = missile.GetComponent(typeof(Missile)) as Missile;
            var distance = missileComponent.hasAttached ? 0 : (float) System.Math.Sqrt(
                System.Math.Sqrt(missileComponent.CameraDistanceRatio())
            );
            var targetPosition = missile.transform.Find("CameraSeat").transform.position;

            transform.position = playerPosition * (distance)
                     + targetPosition * (1 - distance);
            transform.eulerAngles = playerRotation * (distance)
                     + missile.transform.eulerAngles * (1 - distance);
            crashPosition = transform.position;
        }
        else {
            transform.position = playerPosition;
            transform.rotation = playerRotationQuat;
            // transform.position -= (transform.position - playerPosition) * Time.deltaTime;
            // // transform.eulerAngles -= (transform.eulerAngles - initialRotation) * Time.deltaTime;
            // transform.LookAt(crashPosition);
        }
    }
}
