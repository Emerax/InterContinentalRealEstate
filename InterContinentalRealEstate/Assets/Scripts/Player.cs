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

    // Start is called before the first frame update
    void Start() {
        canvas = transform.Find("Canvas").GetComponent<Canvas>();
        scoreText = canvas.transform.Find("ScoreText").GetComponent<Text>();
        scoreText.text = "Score: " + score;
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
            float horizontalTranslation = Input.GetAxis("Mouse X");
            float verticalTranslation = Input.GetAxis("Mouse Y");
            if(gameObject.name == "Player") {
                horizontalTranslation = Input.GetAxis("Joy X");
                verticalTranslation = Input.GetAxis("Joy Y");
            }

            float movement = Input.GetAxisRaw("Mouse ScrollWheel");


            Vector3 direction = (planet.transform.position - transform.position).normalized;
            float distance = Vector3.Distance(transform.position, planet.transform.position);

            if (movement > 0 && distance > minDist) {
                transform.Translate(direction * movementSpeed * movement);
            } else if (movement < 0 && distance < maxDist) {
                transform.Translate(direction * movementSpeed * movement);
            }


            if (distance > minDist && distance < maxDist) {
            }

            transform.RotateAround(planet.transform.position, Vector3.up, horizontalTranslation * Time.deltaTime * horizontalCameraSpeed);
            transform.RotateAround(planet.transform.position, Vector3.right, -verticalTranslation * Time.deltaTime * verticalCameraSpeed);

            transform.LookAt(planet.transform.position);
        }
    }

    void ReadInput() {
        bool fire = Input.GetKeyDown(KeyCode.Space) || Input.GetButton("Fire2");
        if(gameObject.name == "Player") {
            fire = Input.GetButton("Fire1");
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
    }
}
