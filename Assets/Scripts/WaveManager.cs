using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WaveManager : MonoBehaviour
{

    public static WaveManager Instance { get; private set; } 
    public List<WavePattern> possibleWavePatterns;

    void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SummonWave(int waveNumber)
    {
        switch (waveNumber)
        {
            case 1: 
                EnemyManager.Instance.AddWavePattern(possibleWavePatterns[0]);
                break;
            case 2:
                EnemyManager.Instance.AddWavePattern(possibleWavePatterns[1]);
                break;
            case 3:
                // EnemyManager.Instance.AddWavePattern(possibleWavePatterns[69])
                // EnemyManager.Instance.AddWavePattern(possibleWavePatterns[420]) 
                break;
            default: 
                // enemy stat changes here
                // global stat multiplier in EnemyManager?
                // like hp and speed multis
                // i.e. scaledHP = 1 + 0.1(waveNumber - GameManager.Instance.maxWaves), scaledSpeed = 1 + 0.1(waveNumber - GameManager.Instance.maxWaves), 
                break;
        }

        // totalEnemiesInWave used to check if everybody is dead in EnemyManager
        EnemyManager.Instance.totalEnemiesInWave = 0;
        EnemyManager.Instance.enemiesKilledThisWave = 0;
        foreach(WavePattern wp in EnemyManager.Instance.waveQueue)
        {
            EnemyManager.Instance.totalEnemiesInWave += wp.numberToSummon;
        }
        Debug.Log(EnemyManager.Instance.totalEnemiesInWave);
    }
}
