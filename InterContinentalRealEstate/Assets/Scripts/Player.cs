using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public float horizontalCameraSpeed;
    public float verticalCameraSpeed;
    public float movementSpeed;
    public float minDist, maxDist;
    public GameObject planet;
    public GameObject silo;

    public GameObject missilePrefab;
    private bool missileLaunched = false;

    // Start is called before the first frame update
    void Start() {
    }

    // Update is called once per frame
    void Update() {
        UpdateOrbitalMovement();
        ReadInput();
    }

    void UpdateOrbitalMovement() {
        if (!missileLaunched) {
            float horizontalTranslation = Input.GetAxis("Mouse X");
            float verticalTranslation = Input.GetAxis("Mouse Y");

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
        if (Input.GetKeyDown(KeyCode.Space)) {
            if (!missileLaunched) {
                //FIRE ZHE MIZZILEZ
                GameObject missile = Instantiate(
                    missilePrefab,
                    silo.transform.position,
                    silo.transform.rotation
                );
                missile.GetComponent<Missile>().velocity = silo.transform.position.normalized;
                missile.GetComponent<Missile>().SetOwner(this);
                missileLaunched = true;
            }
        }
    }

    public void ReportMissileHit() {
        missileLaunched = false;
    }
}
