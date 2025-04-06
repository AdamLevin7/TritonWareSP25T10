using System.Collections.Generic;
using UnityEngine;

public class GameHandlerScript : Singleton
{
    public int lives;
    public int maxLives;

    public int currentRound;
    public int maxRounds;

    public int money;

    public GameObject towerManager;
    public GameObject enemyManager;

    public List<Tower> availableTowers;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // arbitrary
        lives = maxLives;
        currentRound = maxRounds;
        money = 9999999;
    }

    // Update is called once per frame
    void Update()
    {
        // testing only!
        if (Input.GetKey(KeyCode.L)) lives--;
        if (Input.GetKey(KeyCode.M)) money--;
    }

    // template function for checking whether or not there are any bonuses to be applied due to balances
    public void CheckBalances()
    {
        return;
    }
}
