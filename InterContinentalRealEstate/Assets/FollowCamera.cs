using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    Vector3 crashPosition;
    public Player player;

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
            transform.eulerAngles = playerRotation * (distance)
                     + missile.transform.eulerAngles * (1 - distance);
            crashPosition = transform.position;
        }
        else {
            transform.position = playerPosition;
            transform.rotation = playerRotationQuat;
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
}
