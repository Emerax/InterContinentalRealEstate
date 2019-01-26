using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class House : MonoBehaviour
{
    float scaleVelocity = 1;
    float scale = 0;
    float extraScale = 1.5F;
    float time = 0;
    const float duration = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        scale = EaseOutElastic(time * 1.5F);
        transform.localScale = Vector3.one * scale;
    }

    float EaseOutElastic(float t) {
        if(t == 0) {
            t = 0.001F;
        }
        var sinVal = (float) Math.Sin(Math.PI * 8 * t);
        var timeClamped = Math.Min(t, 1);
        var val = Math.Min(timeClamped * 4, 1)+ sinVal * (1 - timeClamped) * 0.25;
        return (float) val;
    }
}
