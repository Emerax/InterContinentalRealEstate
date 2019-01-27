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

    // Start is called before the first frame update
    void Start() {
        canvas = transform.Find("Canvas").GetComponent<Canvas>();
        scoreText = canvas.transform.Find("ScoreText").GetComponent<Text>();
        scoreText.text = "Score: " + score;
        fuelBar = canvas.transform.Find("FuelBar").GetComponent<Image>();
    }

    // Update is called once per frame
    void Update() {
        UpdateOrbitalMovement();
        ReadInput();
        UpdateUI();
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
            float verticalTranslation = Input.GetAxis("X2");

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
                component.initialDirection = silo.transform.TransformVector(new Vector3(1, 0, 0));
                component.SetOwner(this);
                missileLaunched = true;
            }
        }
    }

    void UpdateUI() {
        scoreText.text = "Score: " + score;
    }

    public void ReportMissileHit() {
        missileLaunched = false;
        fuelBar.fillAmount = 1;
    }

    public void ReportFuelLevel(float level) {
        Debug.Log("Level: " + level);
        fuelBar.fillAmount = level;
    }
}
