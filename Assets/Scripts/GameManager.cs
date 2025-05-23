using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Pool;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int lives;
    public int maxLives;

    public int currentWave;
    public int maxWaves;

    public int money;
    public int starterMoney;

    public List<GameObject> availableTowers;

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
    private float totalResonanceBuff = 1.25f;

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
        if (lives <= 0 && !endingGame)
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

        UIHandlerScript.Instance.roundActiveComponent.SetActive(false);
        UIHandlerScript.Instance.roundLossUI.SetActive(true);
        UIHandlerScript.Instance.UpdateLossUI();

        AudioManager.Instance.musicEventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        AudioManager.Instance.PlayOneShot(AudioManager.Instance.loseSound);
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
        EnemyManager.Instance.totalEnemiesKilled = 0;

        playing = true;
        currentWave = 0;
        isWaveActive = false;
        lives = maxLives;
        money = starterMoney;
        UIHandlerScript.Instance.roundActiveComponent.SetActive(true);
        UIHandlerScript.Instance.roundLossUI.SetActive(false);
        UIHandlerScript.Instance.roundWinUI.SetActive(false);
        SettingsManager.Instance.settingsUI.SetActive(false);

        endAnimationCtr = 0.0f;
        endingGame = false;

        UIHandlerScript.Instance.UpdateWaveNumber(currentWave, maxWaves);
        UIHandlerScript.Instance.SwitchWaveButton(isWaveActive, Time.timeScale);

        StartCoroutine(StartMusic());
    }

    private IEnumerator StartMusic()
    {
        if (!FMODUnity.RuntimeManager.HaveAllBanksLoaded) yield return null;

        AudioManager.Instance.InitializeMusic(AudioManager.Instance.gameplayMusic);
    }

    // function to go back to menu (mainly used for buttons)
    public void GoToMenu()
    {
        SceneManager.LoadScene(0);
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
        // Debug.Log("wave active" + isWaveActive);
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
        Vector2 uiMousePos = Input.mousePosition;
        if (uiMousePos.y < 290.0f && UIHandlerScript.Instance.towerSelectionUI.activeSelf) return;

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

    public void WinGame()
    {
        UIHandlerScript.Instance.roundActiveComponent.SetActive(false);
        UIHandlerScript.Instance.roundWinUI.SetActive(true);

        AudioManager.Instance.musicEventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        AudioManager.Instance.PlayOneShot(AudioManager.Instance.winSound);
        
        playing = false;
        endingGame = true;
    }

    public void EndWave()
    {
        isWaveActive = false;
        Time.timeScale = 1.0f;

        if (currentWave == maxWaves) WinGame();

        UIHandlerScript.Instance.SwitchWaveButton(isWaveActive, Time.timeScale);
        ClearProjectiles();
        ClearEnemies();
        if(Synergy.Instance.totalSynergy){
            money += (int)(100 * currentWave * totalResonanceBuff);
        }
        else{
            money += 100 * currentWave;
        }
    }

    
}
