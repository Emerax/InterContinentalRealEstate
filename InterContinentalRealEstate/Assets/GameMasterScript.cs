using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameMasterScript : MonoBehaviour
{
    public Vector3 planetCenter = Vector3.zero;
    private int dudesOnStart = 5;
    public float radie;
    public GameObject dude;
    private System.Random rnd;

    // Start is called before the first frame update
    void Start()
    {
        rnd = new System.Random();
        reset();
    }

    // Update is called once per frame
    void Update()
    {
        var dudes = FindObjectsOfType(typeof(Dude));
        if(dudes.Length < dudesOnStart) {
            spawnDudes(dudesOnStart - dudes.Length);
        }
    }

    private void reset()
    {
        spawnDudes(dudesOnStart);
    }

    private void spawnDudes(int amount)
    {
        for (int i = 0; i < amount; i++){
            int psi = rnd.Next(0, 180);
            int fi = rnd.Next(0, 360);

            double x = radie * Math.Sin(psi) * Math.Cos(fi);
            double y = radie * Math.Sin(fi) * Math.Sin(psi);
            double z = radie * Math.Cos(psi);
            Instantiate(dude, new Vector3((float)x, (float)y, (float)z), new Quaternion(0, 0, 0,0));
        }
    }
}
