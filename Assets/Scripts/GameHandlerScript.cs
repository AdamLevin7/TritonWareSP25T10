using System;
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

    public List<GameObject> availableTowers;

    [SerializeField] private GameObject GameplayUI;
    [SerializeField] private GameObject LoseScreenUI;

    public bool playing;
    public bool endingGame;
    [SerializeField] private float endSlowdownRate;
    [SerializeField] private float endAnimationDuration;
    private float endAnimationCtr;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // arbitrary
        BeginNewGame();
        money = 999999;
    }

    // Update is called once per frame
    void Update()
    {
        // testing only!
        if (Input.GetKey(KeyCode.L)) lives--;
        if (Input.GetKey(KeyCode.M)) money--;

        if (lives <= 0)
        {
            EndGame();
        }

        /*
        if (endingGame)
        {
            Time.timeScale = Math.Max(0.0f, Time.timeScale - endSlowdownRate * Time.deltaTime);
            endAnimationCtr += Time.deltaTime;
            if (endAnimationCtr > endAnimationDuration)
            {
                GameplayUI.SetActive(false);
                LoseScreenUI.SetActive(true);
                endingGame = false;
            }  
        }
        */
    }

    // end game and show lose screen (usually when)
    public void EndGame()
    {
        playing = false;
        endingGame = true;

        GameplayUI.SetActive(false);
        LoseScreenUI.SetActive(true);
    }

    // begins new game (used in callbacks and button triggers)
    public void BeginNewGame()
    {
        playing = true;
        currentRound = 1;
        lives = maxLives;
        money = 0;
        GameplayUI.SetActive(true);
        LoseScreenUI.SetActive(false);

        endAnimationCtr = 0.0f;
        endingGame = false;
    }

    // function to go back to menu (mainly used for buttons)
    public void GoToMenu()
    {
        Debug.Log("pretend this button works right now");
    }

    // template function for checking whether or not there are any bonuses to be applied due to balances
    public void CheckBalances()
    {
        return;
    }
}
