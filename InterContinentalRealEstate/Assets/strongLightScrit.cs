using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class strongLightScrit : MonoBehaviour
{

    Vector3 velocity = new Vector3(-1, 0, 1) * 0.6F;
    float g = 0.2F;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 acceleration = transform.position.normalized * -g * Time.deltaTime;

        velocity += acceleration;

        transform.position += velocity * Time.deltaTime;

        transform.LookAt(velocity);

        transform.rotation = Quaternion.LookRotation(velocity, transform.position);
    }
}
