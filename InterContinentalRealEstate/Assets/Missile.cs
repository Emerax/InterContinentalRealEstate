using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    Vector3 velocity = new Vector3(1, 0, -1) * 0.6F;
    float g = 0.2F;

    float steering_amonut = 1F;

    // Start is called before the first frame update
    void Start()
    {
        Screen.lockCursor = true;
    }

    // Update is called once per frame
    void Update()
    {
        // In standalone player we have to provide our own key
        // input for unlocking the cursor
        if (Input.GetKeyDown("escape")) {
            Screen.lockCursor = false;
        }

        Vector3 acceleration = transform.position.normalized * -g * Time.deltaTime;

        velocity += acceleration;

        transform.position += velocity * Time.deltaTime;

        transform.LookAt(velocity);

        transform.rotation = Quaternion.LookRotation(velocity, transform.position);

        if(IsFalling()) {
            float xMove = Input.GetAxis("Mouse X") + Input.GetAxis("Joy X");
            float yMove = Input.GetAxis("Mouse Y") + Input.GetAxis("Joy Y");

            // Build a new local vector to use for rotateTo
            var velocityLocal = new Vector3(xMove, yMove, 1);

            // Transform that vector into the global space
            var targetVelocityWorld = transform.localToWorldMatrix * velocityLocal;

            velocity = Vector3.RotateTowards(
                velocity,
                targetVelocityWorld,
                steering_amonut * Time.deltaTime,
                0.0F
            );
        }
    }

    bool IsFalling() {
        return Vector3.Angle(transform.position, velocity) > 90;
    }

    float RatioToTop() {
        var projected = Vector3.Project(velocity, transform.position);
        return System.Math.Min(projected.magnitude * 5, 1.0F);
    }

    public float CameraDistanceRatio() {
        if(IsFalling()) {
            return 0;
        }
        else {
            return RatioToTop();
        }
    }
}
