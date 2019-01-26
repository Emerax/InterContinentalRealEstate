using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour
{
    public Material baseRed;
    public Material detailRed;
    public Material roofRed;
    public Material baseGreen;
    public Material detailGreen;
    public Material roofGreen;
    public Material baseBlue;
    public Material detailBlue;
    public Material roofBlue;

    float scaleAccelerationFactor = 10F;
    float scaleVelocity = 1;
    float scale = 0;

    // Start is called before the first frame update
    void Start()
    {
        setColor(Constants.Color.Blue);
    }

    void setColor(Constants.Color color)
    {
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
        scale = Mathf.SmoothDamp(scale, 50, ref scaleVelocity, 100);
        transform.localScale = Vector3.one * scale;
    }
}
