using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour {
    public Vector3 velocity = new Vector3(1, 1, 1) * 0.6F;
    float g = 0.2F;

    float steering_amonut = 1F;
    public Player owner;
    bool isColliding = false;
    const float steerDuration = 1.5f;
    float steerTime = steerDuration;
    const float steerAmount = 0.6f;
    public float maxFuel = 1000F;
    private float fuel;
    const float fuelConsumption = 40;
    private bool hasFuel = false;
    bool boosting = false;
    bool hasIncressedParticles = false;

    public GameObject houseObject;
    public bool hasAttached = false;
    public Vector3 initialDirection;


    public Material baseRed;
    public Material detailRed;
    public Material baseGreen;
    public Material detailGreen;
    public Material baseBlue;
    public Material detailBlue;

    Constants.Color color;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Array values = Enum.GetValues(typeof(Constants.Color));
        color = (Constants.Color)values.GetValue((int)UnityEngine.Random.Range(0, values.Length));
        setColor(color);
        fuel = 1000F;
        hasFuel = true;
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
        // In standalone player we have to provide our own key
        // input for unlocking the cursor
        if (Input.GetKeyDown("escape")) {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        

        Vector3 acceleration = transform.position.normalized * (-g + (-(hasFuel ? 0:1) * 2))* Time.deltaTime;

        velocity += acceleration;

        transform.position += velocity * Time.deltaTime * (boosting ? 10 : 1);

        transform.LookAt(velocity);

        transform.rotation = Quaternion.LookRotation(velocity, transform.position);

        if ((IsFalling() || hasAttached) && hasFuel) {
            hasAttached = true;
            // Player 2
            float xMove = Input.GetAxis("X2");
            float yMove = Input.GetAxis("Y2");
            boosting = Input.GetButton("Launch2");

            // Player 1
            if(owner.name == "Player") {
                xMove = Input.GetAxis("X1");
                yMove = Input.GetAxis("Y1");
                boosting = Input.GetButton("Launch1");
            }
            steer(xMove, yMove);
            fuel -= Time.deltaTime * fuelConsumption * (boosting ? 4:1);

            Debug.Log("Max: " + maxFuel + " current: " + fuel);

            owner.ReportFuelLevel(fuel / maxFuel);
            
            if (fuel < 0)
            {
                hasFuel = false;
            }
            var particlesObject = transform.Find("Particle System");
            if (!hasFuel)
            {
                //Make particles linger after the missile is destroyed.
                if (particlesObject != null)
                {
                    GameObject particles = particlesObject.gameObject;
                    particles.GetComponent<ParticleSystem>().Stop();
                    particles.transform.parent = null;
                    Destroy(particles, 6);
                }
            }
            if (boosting && !hasIncressedParticles)
            {
                
                if (particlesObject != null)
                {
                    hasIncressedParticles = true;
                    GameObject particles = particlesObject.gameObject;
                    var particleSystemObject = particles.GetComponent<ParticleSystem>();
                    particleSystemObject.emissionRate *= 2;
                    particleSystemObject.startSize *= 2;
                    // particleSystemObject.transform.localScale *= 2;
                }
            } else if (!boosting)
            {
                if (particlesObject != null)
                {
                    hasIncressedParticles = false;
                    GameObject particles = particlesObject.gameObject;
                    var particleSystemObject = particles.GetComponent<ParticleSystem>();
                    particleSystemObject.emissionRate = 30;
                    particleSystemObject.startSize = 0.4f;
                    particleSystemObject.transform.localScale = new Vector3(1, 1, 1);
                }
            }
        }

        if(!isColliding && RatioToTop() > 0.9 && !IsFalling() && steerTime > 0) {
            steerTime -= Time.deltaTime;
            velocity += initialDirection
                * Time.deltaTime 
                * steerAmount
                * (float) Math.Sin(steerTime / steerDuration * Math.PI);
        }
    }

    void steer(float xMove, float yMove) {
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

    public void OnCollision(Collider other) {
        if(IsFalling()) {
            GameObject clone = Instantiate(houseObject, transform.position, transform.rotation);
            if(other.GetComponentInParent<House>() != null)
            {
                clone.transform.LookAt(other.transform.position);
            } else
            {
                clone.transform.LookAt(new Vector3(0, 0, 0));
            }
            clone.GetComponent<House>().setColor(color);
            clone.GetComponent<House>().owner = owner;

            //Make particles linger after the missile is destroyed.
            var particlesObject = transform.Find("Particle System");
            if(particlesObject != null) {
                GameObject particles = particlesObject.gameObject;
                particles.GetComponent<ParticleSystem>().Stop();
                particles.transform.parent = null;
                Destroy(particles, 6);
            }

            Destroy(gameObject);
            Screen.lockCursor = false;
        }
        isColliding = true;
    }
    public void OnCollisionStop(Collider other) {
        isColliding = false;
    }

    public void OnDestroy() {
        owner.ReportMissileHit();
    }

    public bool IsFalling() {
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

    void SpinRocket() {
        if(!isColliding && RatioToTop() > 0.9 && !IsFalling()) {
            var velocityLocal = new Vector3(-1, 0, 0) * Time.deltaTime;
            // Transform that vector into the global space
            Vector3 targetVelocityWorld = transform.localToWorldMatrix * velocityLocal;
            velocity += targetVelocityWorld;
        }
    }
}
