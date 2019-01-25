using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    Vector3 velocity = Vector3.one * 0.1F;
    float g = 0.1F;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 acceleration = gameObject.transform.position.normalized * (-g) * Time.deltaTime;

        velocity += acceleration;

        gameObject.transform.position += velocity * Time.deltaTime;
        Debug.Log(gameObject.transform.position);
    }
}
