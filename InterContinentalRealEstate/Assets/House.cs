using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour
{
    float scaleAccelerationFactor = 10F;
    float scaleVelocity = 1;
    float scale = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        scale = Mathf.SmoothDamp(scale, 50, ref scaleVelocity, 100);
        transform.localScale = Vector3.one * scale;
    }
}
