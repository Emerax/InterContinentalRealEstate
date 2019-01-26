using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dude : MonoBehaviour
{
    public Vector3 planetCenter = Vector3.zero;

    public Material clothesRed;
    public Material clothesGreen;
    public Material clothesBlue;

    private float timeUntilNewDirection = 0.0f;
    private Quaternion direction = Quaternion.Euler(0, 0, 0);

    private int clothesMaterialID = 0;
    private Rigidbody rb;

    public float speed;
    public Constants.Color color;

    void Start()
    {
        timeUntilNewDirection = 0.0f;
        rb = GetComponent<Rigidbody>();
        Array values = Enum.GetValues(typeof(Constants.Color));
        color = (Constants.Color)values.GetValue((int)UnityEngine.Random.Range(0, values.Length));
        setColor(color);
    }

    void setColor(Constants.Color color)
    {
        this.color = color;

        var renderer = GetComponentInChildren<Renderer>();
        var materials = renderer.sharedMaterials;
        switch (color) {
            case Constants.Color.Red:
                materials[1] = clothesRed;
                break;
            case Constants.Color.Green:
                materials[1] = clothesGreen;
                break;
            case Constants.Color.Blue:
                materials[1] = clothesBlue;
                break;
        }
        renderer.sharedMaterials = materials;
    }

    void Update()
    {
        // New movement direction
        if (timeUntilNewDirection <= 0.0f)
        {
            float maxAngle = 2.5f * speed;
            float maxTime = 5.0f;

            float angleA = (UnityEngine.Random.value * 2.0f - 1.0f) * maxAngle;
            float angleB = (UnityEngine.Random.value * 2.0f - 1.0f) * maxAngle;
            float angleC = (UnityEngine.Random.value * 2.0f - 1.0f) * maxAngle;

            direction = Quaternion.Euler(angleA, angleB, angleC);
            timeUntilNewDirection = UnityEngine.Random.value * maxTime;
        }

        // Move in direction
        Quaternion deltaRotation = Quaternion.Lerp(Quaternion.Euler(0, 0, 0), direction, Time.deltaTime);
        rb.MovePosition(deltaRotation * (transform.position - planetCenter) + planetCenter);
        timeUntilNewDirection -= Time.deltaTime;

        // Feet towards planet center
        transform.LookAt(planetCenter);
    }
}
