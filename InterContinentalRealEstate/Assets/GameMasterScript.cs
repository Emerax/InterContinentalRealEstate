using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;

public class GameMasterScript : MonoBehaviour
{
    public Vector3 planetCenter = Vector3.zero;
    private int dudesOnStart = 5;
    public float radie;
    public GameObject dude;
    private System.Random rnd;
    public Player player1;
    public Player player2;
    private bool started = false;

    const int StartGameTime = 3 * 60;
    float gameTimeLeft;
    // Start is called before the first frame update
    void Start()
    {
        rnd = new System.Random();
        reset(false);
    }

    // Update is called once per frame
    void Update()
    {
        var dudes = FindObjectsOfType(typeof(Dude));
        if(dudes.Length < dudesOnStart) {
            spawnDudes(dudesOnStart - dudes.Length);
        }

        if (started)
        {
            gameTimeLeft -= Time.deltaTime;
        }
        Debug.Log(gameTimeLeft);

        if (Input.GetKeyUp(KeyCode.R))
        {
            reset(true);
        }else if (Input.GetKeyUp(KeyCode.T))
        {
            reset(false);
        }

        if (gameTimeLeft <= 0)
        {
            bool player1Wins = player1.score > player2.score;
            bool equal = player1.score == player2.score;

            Debug.Log("Player 1 wins = " + player1Wins + " Player 2 wins = " + !player1Wins + "equal = " + equal);
        }
        
    }

    private void reset(bool removeHouses)
    {
        if (started)
        {
            removedDudes();
            removedMissiles();
        }
        spawnDudes(dudesOnStart);

        if (removeHouses)
        {
            removedHouses();
        }

        player1.score = 0;
        player2.score = 0;
        gameTimeLeft = StartGameTime;
        started = true;
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

    private void removedDudes()
    {
        var dudes = Resources.FindObjectsOfTypeAll<Dude>();
        foreach (var dude in dudes)
        {
            if (!(PrefabUtility.GetPrefabParent(dude.gameObject) == null && PrefabUtility.GetPrefabObject(dude.gameObject) != null))
            {
                Destroy(dude.gameObject);
            }
        }
    }

    private void removedHouses()
    {
        var houses = Resources.FindObjectsOfTypeAll<House>();
        foreach (var house in houses)
        {
            if (!(PrefabUtility.GetPrefabParent(house.gameObject) == null && PrefabUtility.GetPrefabObject(house.gameObject) != null))
            {
                Destroy(house.gameObject);
            }
        }
    }

    private void removedMissiles()
    {
        var missiles = Resources.FindObjectsOfTypeAll<Missile>();
        foreach (var missile in missiles)
        {
            if (!(PrefabUtility.GetPrefabParent(missile.gameObject) == null && PrefabUtility.GetPrefabObject(missile.gameObject) != null))
            {
                Destroy(missile.gameObject);
            }
        }
    }
}
