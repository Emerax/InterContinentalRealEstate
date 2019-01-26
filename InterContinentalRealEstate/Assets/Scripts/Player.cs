using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public float horizontalCameraSpeed;
    public float verticalCameraSpeed;
    public float movementSpeed;
    public float minDist, maxDist;
    public GameObject planet;

    // Start is called before the first frame update
    void Start() {
        if (!planet) {
            Debug.LogError("Planet not set in player!");
        }
    }

    // Update is called once per frame
    void Update() {
        UpdateOrbitalMovement();
    }

    void UpdateOrbitalMovement() {
        float horizontalTranslation = Input.GetAxis("Mouse X");
        float verticalTranslation = Input.GetAxis("Mouse Y");

        float movement = Input.GetAxisRaw("Mouse ScrollWheel");

        Debug.Log(movement);

        Vector3 direction = (planet.transform.position - transform.position).normalized;
        float distance = Vector3.Distance(transform.position, planet.transform.position);

        if(movement > 0 && distance > minDist) {
            Debug.Log("Moving in");
            transform.Translate(direction * movementSpeed * movement);
        } else if (movement < 0 && distance < maxDist) {
            Debug.Log("Moving out");
            transform.Translate(direction * movementSpeed * movement);
        }


        if (distance > minDist && distance < maxDist) {
        }

        transform.RotateAround(planet.transform.position, Vector3.up, horizontalTranslation * Time.deltaTime * horizontalCameraSpeed);
        transform.RotateAround(planet.transform.position, Vector3.right, -verticalTranslation * Time.deltaTime * verticalCameraSpeed);

        transform.LookAt(planet.transform.position);
    }
}
