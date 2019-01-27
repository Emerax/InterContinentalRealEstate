using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {
    public float horizontalCameraSpeed;
    public float verticalCameraSpeed;
    public float movementSpeed;
    public float minDist, maxDist;
    public GameObject planet;
    public GameObject silo;

    public int score = 0;

    public GameObject missilePrefab;

    private bool missileLaunched = false;

    private Canvas canvas;
    private Text scoreText;
    private Image fuelBar;
    private Image nextMissile;
    private Constants.Color nextColor;

    // Start is called before the first frame update
    void Start() {
        foreach(var joystick in Input.GetJoystickNames()) {
            Debug.Log(joystick);
        }
        canvas = transform.Find("Canvas").GetComponent<Canvas>();
        scoreText = canvas.transform.Find("ScoreText").GetComponent<Text>();
        scoreText.text = "Score: " + score;
        fuelBar = canvas.transform.Find("FuelBar").GetComponent<Image>();
        nextMissile = canvas.transform.Find("NextMissile").GetComponent<Image>();
        nextColor = NewColor();
        nextMissile.color = TranslateToSensibleColor(nextColor);
    }

    // Update is called once per frame
    void Update() {
        UpdateOrbitalMovement();
        ReadInput();
        UpdateUI();

        if(missileLaunched) {
            silo.transform.Find("DirectionIndicator").gameObject.SetActive(false);
        }
        else {
            silo.transform.Find("DirectionIndicator").gameObject.SetActive(true);
        }
        silo.transform.Find("DirectionIndicator").transform.localScale = new Vector3(
            1,
            1,
            LaunchDirection()
        );
    }

    void UpdateOrbitalMovement() {
        // Find the missile object
        Missile missile = null;
        foreach (var m in FindObjectsOfType(typeof(Missile)) as Missile[]) {
            if(m.owner == this) {
                missile = m;
            }
        }

        if (missile == null || !(missile.GetComponent(typeof(Missile)) as Missile).IsFalling()) {
            // Player 2
            float horizontalTranslation = Input.GetAxis("X2");
            float verticalTranslation = Input.GetAxis("Y2");

            // Player 1
            if(gameObject.name == "Player") {
                horizontalTranslation = Input.GetAxis("X1");
                verticalTranslation = Input.GetAxis("Y1");
            }

            transform.RotateAround(planet.transform.position, Vector3.up, horizontalTranslation * Time.deltaTime * horizontalCameraSpeed);
            transform.RotateAround(planet.transform.position, Vector3.right, -verticalTranslation * Time.deltaTime * verticalCameraSpeed);

            transform.LookAt(planet.transform.position);
        }
    }

    void ReadInput() {
        // Player 2
        bool fire = Input.GetButton("Launch2");

        // Player 1
        if(gameObject.name == "Player") {
            fire = Input.GetButton("Launch1");
        }
        if (fire) {
            if (!missileLaunched) {
                //FIRE ZHE MIZZILEZ
                GameObject missile = Instantiate(
                    missilePrefab,
                    silo.transform.position,
                    silo.transform.rotation
                );
                var component = missile.GetComponent<Missile>();
                component.velocity = silo.transform.position.normalized;

                component.initialDirection = silo.transform.TransformVector(
                    new Vector3(LaunchDirection(), 0, 0)
                );
                component.SetOwner(this);
                missileLaunched = true;

                missile.GetComponent<Missile>().setColor(nextColor);
                nextColor = NewColor();
                nextMissile.color = TranslateToSensibleColor(nextColor);
            }
        }
    }

    float LaunchDirection() {
        var cameraInLocal = silo.transform.InverseTransformPoint(transform.position);
        return (cameraInLocal.x > 0) ? 1 : -1;
    }

    void UpdateUI() {
        scoreText.text = "Score: " + score;
    }

    public void ReportMissileHit() {
        missileLaunched = false;
        fuelBar.fillAmount = 1;
    }

    public void ReportFuelLevel(float level) {
        fuelBar.fillAmount = level;
    }

    private Constants.Color NewColor() {
        Array values = Enum.GetValues(typeof(Constants.Color));
        return (Constants.Color)values.GetValue(UnityEngine.Random.Range(0, values.Length));
    }

    private Color TranslateToSensibleColor(Constants.Color nextColor) {
        if(nextColor == Constants.Color.Blue) {
            return new Color(0, 115.0f/ 255.0f, 202.0f / 255.0f);
        } else if(nextColor == Constants.Color.Green) {
            return new Color(48.0f / 255.0f, 204.0f / 255.0f, 0);
        } else {
            return new Color(204.0f / 255.0f, 0, 4.0f / 255.0f);
        }
    }
}
