using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int lives;
    public int maxLives;

    public int currentWave;
    public int maxWaves;

    public int money;
    public int starterMoney;

    public GameObject towerManager;
    public GameObject enemyManager;

    public List<GameObject> availableTowers;

    public GameObject activeGameUI;
    public GameObject loseScreenUI;

    public BoxCollider groundCollider;

    public bool playing;
    public bool isWaveActive;
    public float waveSpeedUpFactor = 2f;
    public bool isSpedUp;
    public bool placingTower;

    public bool endingGame;
    [SerializeField] private float endSlowdownRate;
    [SerializeField] private float endAnimationDuration;
    private float endAnimationCtr;

    public InputSystem_Actions inputActions;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;

        inputActions = new();
    }

    private void OnEnable()
    {
        inputActions.Enable();
        inputActions.Player.Interact.started += StartInteract;
    }

    private void OnDisable()
    {
        inputActions.Player.Interact.started -= StartInteract;
        inputActions.Disable();
    }

    private void Start()
    {
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
        // towerManager.ClearTowers()
        // or do it yourself here

        // delete all towers and projectiles
        ClearTowers();
        ClearProjectiles();
        ClearEnemies();

        playing = true;
        currentWave = 0;
        isWaveActive = false;
        lives = maxLives;
        money = starterMoney;
        activeGameUI.SetActive(true);
        loseScreenUI.SetActive(false);

        endAnimationCtr = 0.0f;
        endingGame = false;

        UIHandlerScript.Instance.UpdateWaveNumber(currentWave, maxWaves);
        UIHandlerScript.Instance.SwitchWaveButton(isWaveActive, Time.timeScale);
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

    public void ControlWave()
    {
        if (isWaveActive)
        {
            Time.timeScale = (Time.timeScale == 1.0f) ? waveSpeedUpFactor : 1.0f;
            isSpedUp = Time.timeScale > 1.0f;
        }
        else
        {
            SendNextWave();
        }
        UIHandlerScript.Instance.SwitchWaveButton(isWaveActive, Time.timeScale);
        Debug.Log("wave active" + isWaveActive);
    }

    public void SendNextWave()
    {
        currentWave++;
        isWaveActive = true;
        
        // probably pull summon[] data from .json or some other data file
        WaveManager.Instance.SummonWave(currentWave);
        UIHandlerScript.Instance.UpdateWaveNumber(currentWave, maxWaves);
        UIHandlerScript.Instance.SwitchWaveButton(isWaveActive, Time.timeScale);
    }

    // GameManager's own click event check!
    // only used for towerselectedui for now
    private void StartInteract(InputAction.CallbackContext ctx)
    {
        Vector2 mousePos = inputActions.Player.MousePos.ReadValue<Vector2>();
        // because of different resolutions i guess (maybe redundant)
        Vector2 worldMousePos = (Vector2)Camera.main.ScreenToWorldPoint(mousePos);
        if (worldMousePos.y < -4.0f) return;

        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        bool hit = Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity);

        // should hit ground no matter what, but some redundancy wouldn't hurt :)
        if (hitInfo.collider == null) return;

        // if clicking "nothing", hide tower selected ui
        if (!hitInfo.collider.gameObject.CompareTag("tower"))
        {
            UIHandlerScript.Instance.SetTowerSelectedUIState(false);
        }
    }

    // delete all towers on screen
    public void ClearTowers()
    {
        
        GameObject[] towersOnScreen = GameObject.FindGameObjectsWithTag("tower");

        foreach (GameObject t in towersOnScreen)
        {
            Destroy(t);
        }
    }

    // delete all projectiles on screem
    public void ClearProjectiles()
    {

        GameObject[] projectilesOnScreen = GameObject.FindGameObjectsWithTag("Projectile");

        foreach (GameObject p in projectilesOnScreen)
        {
            Destroy(p);
        }
    }

    public void ClearEnemies()
    {
        GameObject[] enemiesOnScreen = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject e in enemiesOnScreen)
        {
            Destroy(e);
        }

        EnemyManager.Instance.ClearWavePatterns();
    }

    public void EndWave()
    {
        isWaveActive = false;
        Time.timeScale = 1.0f;
        UIHandlerScript.Instance.SwitchWaveButton(isWaveActive, Time.timeScale);
        ClearProjectiles();
        ClearEnemies();

        money += 100 * currentWave;
    }
}
