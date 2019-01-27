using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FollowCamera : MonoBehaviour
{
    Vector3 crashPosition;
    Quaternion crashRotation;
    public Player player;
    float zoomOutTime = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Find the missile object
        Missile missile = null;
        foreach (var m in FindObjectsOfType(typeof(Missile)) as Missile[]) {
            if(m.owner == player) {
                missile = m;
            }
        }

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
            transform.rotation = Quaternion.Lerp(
                missile.transform.rotation,
                playerRotationQuat,
                distance
            );
            crashPosition = transform.position;
            crashRotation = transform.rotation;
            zoomOutTime = 0;
        }
        else {
            float missileDistance = zoomOutAmount(zoomOutTime);
            transform.position = playerPosition * missileDistance
                    + (crashPosition * (1-missileDistance));
            transform.rotation = Quaternion.Lerp(crashRotation, playerRotationQuat, missileDistance);

            zoomOutTime = Math.Min(zoomOutTime + Time.deltaTime, 1);
            Screen.lockCursor = true;
            if (Input.GetKeyDown("escape"))
            {
                Screen.lockCursor = false;
            }
            // transform.position -= (transform.position - playerPosition) * Time.deltaTime;
            // // transform.eulerAngles -= (transform.eulerAngles - initialRotation) * Time.deltaTime;
            // transform.LookAt(crashPosition);
        }
    }

    float zoomOutAmount(float t) {
        // The integral of this (math.sqrt(t) * (1-t**3))
        // divided by its max value (0.444)
        return -2/9.0f*(float)Math.Pow(t, (3/2.0f)) * ((float)Math.Pow(t, 3) - 3) / 0.444F;
    }
}
