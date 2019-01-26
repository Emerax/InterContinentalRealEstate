﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour {
    public Vector3 velocity = new Vector3(1, 1, 1) * 0.6F;
    float g = 0.2F;

    float steering_amonut = 1F;
    Player owner;

    public GameObject houseObject;
    public bool hasAttached = false;

    public Material baseRed;
    public Material detailRed;
    public Material baseGreen;
    public Material detailGreen;
    public Material baseBlue;
    public Material detailBlue;

    // Start is called before the first frame update
    void Start()
    {
        Screen.lockCursor = true;
        setColor(Constants.Color.Blue);
    }

    void setColor(Constants.Color color)
    {
        var renderer = GetComponentInChildren<Renderer>();
        var materials = renderer.sharedMaterials;
        switch (color)
        {
            case Constants.Color.Red:
                materials[0] = baseRed;
                materials[1] = detailRed;
                break;
            case Constants.Color.Green:
                materials[0] = baseGreen;
                materials[1] = detailGreen;
                break;
            case Constants.Color.Blue:
                materials[0] = baseBlue;
                materials[1] = detailBlue;
                break;
        }
        renderer.sharedMaterials = materials;
    }

    // Update is called once per frame
    void Update() {
        Debug.Log(IsFalling());
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

        if(IsFalling() || hasAttached) {
            hasAttached = true;
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

    public void OnCollision(Collider other) {
        if(IsFalling()) {
            GameObject clone = Instantiate(houseObject, transform.position, transform.rotation);
            clone.transform.LookAt(new Vector3(0, 0, 0));
            Destroy(this.gameObject);
            Screen.lockCursor = false;
        }
    }

    public void OnDestroy() {
        owner.ReportMissileHit();
    }

    bool IsFalling() {
        if(hasAttached || Vector3.Angle(transform.position, velocity) > 90) {
            hasAttached = true;
            return true;
        }
        return false;
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

    public void SetOwner(Player player) {
        owner = player;
    }
}
