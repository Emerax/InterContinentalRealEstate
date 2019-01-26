using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dude : MonoBehaviour
{
    public Vector3 planetCenter = Vector3.zero;

    private float timeUntilNewDirection = 0.0f;
    private Quaternion direction = Quaternion.Euler(0, 0, 0);
    
    void Start()
    {
        timeUntilNewDirection = 0.0f;
    }

    void Update()
    {
        // New movement direction
        if (timeUntilNewDirection <= 0.0f)
        {
            float maxAngle = 2.5f;
            float maxTime = 5.0f;

            float angleA = (Random.value * 2.0f - 1.0f) * maxAngle;
            float angleB = (Random.value * 2.0f - 1.0f) * maxAngle;
            float angleC = (Random.value * 2.0f - 1.0f) * maxAngle;

            direction = Quaternion.Euler(angleA, angleB, angleC);
            timeUntilNewDirection = Random.value * maxTime;
        }

        // Move in direction
        Quaternion deltaRotation = Quaternion.Lerp(Quaternion.Euler(0, 0, 0), direction, Time.deltaTime);
        transform.position = deltaRotation * (transform.position - planetCenter) + planetCenter;
        timeUntilNewDirection -= Time.deltaTime;

        // Feet towards planet center
        transform.LookAt(planetCenter);
    }
}
