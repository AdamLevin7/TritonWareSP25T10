using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int lives;
    public int maxLives;

    public int currentRound;
    public int maxRounds;

    public int money;
    public int starterMoney;

    public GameObject towerManager;
    public GameObject enemyManager;

    public List<GameObject> availableTowers;

    public GameObject activeGameUI;
    public GameObject loseScreenUI;

    public BoxCollider groundCollider;

    public bool playing;
    public bool endingGame;
    [SerializeField] private float endSlowdownRate;
    [SerializeField] private float endAnimationDuration;
    private float endAnimationCtr;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;

        BeginNewGame();
    }

    // Update is called once per frame
    void Update()
    {
        // testing only!
        if (Input.GetKey(KeyCode.L)) lives--;
        if (Input.GetKey(KeyCode.M)) money += 1000;

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

        activeGameUI.SetActive(false);
        loseScreenUI.SetActive(true);
    }

    // begins new game (used in callbacks and button triggers)
    public void BeginNewGame()
    {
        playing = true;
        currentRound = 1;
        lives = maxLives;
        money = starterMoney;
        activeGameUI.SetActive(true);
        loseScreenUI.SetActive(false);

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
