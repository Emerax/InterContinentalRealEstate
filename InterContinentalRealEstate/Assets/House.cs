using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class House : MonoBehaviour
{
    public int dudeScore = 1;

    public Material baseRed;
    public Material detailRed;
    public Material roofRed;
    public Material baseGreen;
    public Material detailGreen;
    public Material roofGreen;
    public Material baseBlue;
    public Material detailBlue;
    public Material roofBlue;
    
    float scaleVelocity = 1;
    float scale = 0;
    float extraScale = 1.5F;
    float time = 0;
    const float duration = 1;

    private Constants.Color color;
    public Player owner;

    private Collider collider;
    bool isBuilt = false;

    void Start()
    {
        collider = GetComponentInChildren<Collider>();
        collider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        Dude dude = other.GetComponentInParent<Dude>();
        if (dude != null && dude.color == color && !isBuilt)
        {
            Destroy(other.transform.parent.gameObject);
            owner.score += dudeScore;
        }
        else if(dude != null) {
            dude.OnHouseCollision();
        }
    }
    private void OnTriggerExit(Collider other) {
        Dude dude = other.GetComponentInParent<Dude>();
        if(dude != null) {
            dude.OnHouseCollisionStop();
        }
    }

    public void setColor(Constants.Color color)
    {
        this.color = color;

        var renderer = GetComponentInChildren<Renderer>();
        var materials = renderer.sharedMaterials;
        switch (color)
        {
            case Constants.Color.Red:
                materials[0] = baseRed;
                materials[1] = roofRed;
                materials[2] = detailRed;
                break;
            case Constants.Color.Green:
                materials[0] = baseGreen;
                materials[1] = roofGreen;
                materials[2] = detailGreen;
                break;
            case Constants.Color.Blue:
                materials[0] = baseBlue;
                materials[1] = roofBlue;
                materials[2] = detailBlue;
                break;
        }
        renderer.sharedMaterials = materials;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        scale = EaseOutElastic(time * 1.5F);
        transform.localScale = Vector3.one * scale;
    }

    float EaseOutElastic(float t) {
        float elasticityTime = 1.0f;
        if (t >= elasticityTime)
        {
            // collider.isTrigger = false;
            isBuilt = true;
        }

        if(t == 0) {
            t = 0.001F;
        }
        var sinVal = (float) Math.Sin(Math.PI * 8 * t);
        var timeClamped = Math.Min(t, elasticityTime);
        var val = Math.Min(timeClamped * 4, 1)+ sinVal * (1 - timeClamped) * 0.25;
        return (float) val;
    }
}
