using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WaveManager : MonoBehaviour
{

    public static WaveManager Instance { get; private set; } 
    public List<WavePattern> possibleWavePatterns;

    [SerializeField] private EnemyData yellowSlime;
    [SerializeField] private EnemyData blackSlime;
    [SerializeField] private EnemyData pinkSlime;

    [SerializeField] private float freeplayYellowNumberScale;
    [SerializeField] private float freeplayBlackNumberScale;
    [SerializeField] private float freeplayPinkNumberScale;
    [SerializeField] private float randomScaleOffet;

    private WavePattern yellowSlimesRandomPattern;
    private WavePattern blackSlimesRandomPattern;
    private WavePattern pinkSlimesRandomPattern;


    void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        yellowSlimesRandomPattern = new();
        blackSlimesRandomPattern = new();
        pinkSlimesRandomPattern = new();

        yellowSlimesRandomPattern.enemyToSummon = yellowSlime;
        blackSlimesRandomPattern.enemyToSummon = blackSlime;
        pinkSlimesRandomPattern.enemyToSummon = pinkSlime;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SummonWave(int waveNumber)
    {
        // enemy stat changes here
        // global stat multiplier in EnemyManager?
        // like hp and speed multis
        // i.e. scaledHP = 1 + 0.1(waveNumber - GameManager.Instance.maxWaves), scaledSpeed = 1 + 0.1(waveNumber - GameManager.Instance.maxWaves), 
        
        // OR, spam towers to high hell because its funny

        switch (waveNumber)
        {
            case 1: 
                EnemyManager.Instance.AddWavePattern(possibleWavePatterns[0]);
                break;
            case 2:
                EnemyManager.Instance.AddWavePattern(possibleWavePatterns[1]);
                EnemyManager.Instance.AddWavePattern(possibleWavePatterns[2]);
                break;
            case 3:
                EnemyManager.Instance.AddWavePattern(possibleWavePatterns[3]);
                break;
            case 4: 
                EnemyManager.Instance.AddWavePattern(possibleWavePatterns[4]);
                EnemyManager.Instance.AddWavePattern(possibleWavePatterns[5]);
                EnemyManager.Instance.AddWavePattern(possibleWavePatterns[6]);
                break;
            case 5:
                EnemyManager.Instance.AddWavePattern(possibleWavePatterns[7]);
                break;
            case 6:
                EnemyManager.Instance.AddWavePattern(possibleWavePatterns[8]);
                EnemyManager.Instance.AddWavePattern(possibleWavePatterns[9]);
                EnemyManager.Instance.AddWavePattern(possibleWavePatterns[10]);
                EnemyManager.Instance.AddWavePattern(possibleWavePatterns[11]);
                break;
            case 7: 
                EnemyManager.Instance.AddWavePattern(possibleWavePatterns[12]);
                break;
            case 8:
                EnemyManager.Instance.AddWavePattern(possibleWavePatterns[13]);
                EnemyManager.Instance.AddWavePattern(possibleWavePatterns[14]);
                break;
            case 9:
                EnemyManager.Instance.AddWavePattern(possibleWavePatterns[15]);
                EnemyManager.Instance.AddWavePattern(possibleWavePatterns[16]);
                EnemyManager.Instance.AddWavePattern(possibleWavePatterns[17]);
                EnemyManager.Instance.AddWavePattern(possibleWavePatterns[18]);
                break;
            case 10:
                EnemyManager.Instance.AddWavePattern(possibleWavePatterns[19]);
                EnemyManager.Instance.AddWavePattern(possibleWavePatterns[20]);
                EnemyManager.Instance.AddWavePattern(possibleWavePatterns[21]);
                EnemyManager.Instance.AddWavePattern(possibleWavePatterns[22]);
                break;
            case 11:
                EnemyManager.Instance.AddWavePattern(possibleWavePatterns[23]);
                EnemyManager.Instance.AddWavePattern(possibleWavePatterns[24]);
                EnemyManager.Instance.AddWavePattern(possibleWavePatterns[25]);
                break;
            case 12:
                EnemyManager.Instance.AddWavePattern(possibleWavePatterns[26]);
                EnemyManager.Instance.AddWavePattern(possibleWavePatterns[27]);
                EnemyManager.Instance.AddWavePattern(possibleWavePatterns[28]);
                EnemyManager.Instance.AddWavePattern(possibleWavePatterns[29]);
                break;
            case 13:
                EnemyManager.Instance.AddWavePattern(possibleWavePatterns[30]);
                EnemyManager.Instance.AddWavePattern(possibleWavePatterns[31]);
                EnemyManager.Instance.AddWavePattern(possibleWavePatterns[32]);
                EnemyManager.Instance.AddWavePattern(possibleWavePatterns[33]);
                EnemyManager.Instance.AddWavePattern(possibleWavePatterns[34]);
                break;
            case 14:
                EnemyManager.Instance.AddWavePattern(possibleWavePatterns[35]);
                break;
            case 15:
                EnemyManager.Instance.AddWavePattern(possibleWavePatterns[36]);
                EnemyManager.Instance.AddWavePattern(possibleWavePatterns[37]);
                EnemyManager.Instance.AddWavePattern(possibleWavePatterns[38]);
                EnemyManager.Instance.AddWavePattern(possibleWavePatterns[39]);
                break;
            case 16:
                EnemyManager.Instance.AddWavePattern(possibleWavePatterns[40]);
                EnemyManager.Instance.AddWavePattern(possibleWavePatterns[41]);
                break;
            case 17:
                EnemyManager.Instance.AddWavePattern(possibleWavePatterns[42]);
                break;
            case 18:
                EnemyManager.Instance.AddWavePattern(possibleWavePatterns[43]);
                EnemyManager.Instance.AddWavePattern(possibleWavePatterns[44]);
                EnemyManager.Instance.AddWavePattern(possibleWavePatterns[45]);
                EnemyManager.Instance.AddWavePattern(possibleWavePatterns[46]);
                EnemyManager.Instance.AddWavePattern(possibleWavePatterns[47]);
                break;
            case 19:
                EnemyManager.Instance.AddWavePattern(possibleWavePatterns[48]);
                EnemyManager.Instance.AddWavePattern(possibleWavePatterns[49]);
                EnemyManager.Instance.AddWavePattern(possibleWavePatterns[50]);
                break;
            case 20:
                EnemyManager.Instance.AddWavePattern(possibleWavePatterns[51]);
                EnemyManager.Instance.AddWavePattern(possibleWavePatterns[52]);
                EnemyManager.Instance.AddWavePattern(possibleWavePatterns[53]);
                EnemyManager.Instance.AddWavePattern(possibleWavePatterns[54]);
                break;   
            case 21:
                EnemyManager.Instance.AddWavePattern(possibleWavePatterns[55]);
                EnemyManager.Instance.AddWavePattern(possibleWavePatterns[56]);
                EnemyManager.Instance.AddWavePattern(possibleWavePatterns[57]);
                EnemyManager.Instance.AddWavePattern(possibleWavePatterns[58]);
                EnemyManager.Instance.AddWavePattern(possibleWavePatterns[59]);
                EnemyManager.Instance.AddWavePattern(possibleWavePatterns[60]);                
                break;
            case 22:
                EnemyManager.Instance.AddWavePattern(possibleWavePatterns[61]);
                break;
            case 23:
                EnemyManager.Instance.AddWavePattern(possibleWavePatterns[62]);
                break;
            case 24:
                EnemyManager.Instance.AddWavePattern(possibleWavePatterns[63]);
                EnemyManager.Instance.AddWavePattern(possibleWavePatterns[64]);
                EnemyManager.Instance.AddWavePattern(possibleWavePatterns[65]);
                EnemyManager.Instance.AddWavePattern(possibleWavePatterns[66]);
                EnemyManager.Instance.AddWavePattern(possibleWavePatterns[67]);
                break;
            case 25:
                EnemyManager.Instance.AddWavePattern(possibleWavePatterns[68]);
                break;
            case 26:
                EnemyManager.Instance.AddWavePattern(possibleWavePatterns[69]);
                break;
            case 27:
                EnemyManager.Instance.AddWavePattern(possibleWavePatterns[70]);
                EnemyManager.Instance.AddWavePattern(possibleWavePatterns[71]);
                break;
            case 28:
                EnemyManager.Instance.AddWavePattern(possibleWavePatterns[72]);
                break;
            case 29:
                EnemyManager.Instance.AddWavePattern(possibleWavePatterns[73]);
                EnemyManager.Instance.AddWavePattern(possibleWavePatterns[74]);
                break;
            case 30:
                EnemyManager.Instance.AddWavePattern(possibleWavePatterns[75]);
                EnemyManager.Instance.AddWavePattern(possibleWavePatterns[76]);
                break;               
            default: 
                // enemy stat changes here
                // global stat multiplier in EnemyManager?
                // like hp and speed multis
                // i.e. scaledHP = 1 + 0.1(waveNumber - GameManager.Instance.maxWaves), scaledSpeed = 1 + 0.1(waveNumber - GameManager.Instance.maxWaves), 
                
                // OR, spam towers to high hell because its funny

                float randomScale = UnityEngine.Random.Range(1.0f - randomScaleOffet, 1.0f + randomScaleOffet);
                yellowSlimesRandomPattern.numberToSummon = (int)(Math.Round(randomScale * (freeplayYellowNumberScale * (waveNumber - 30) + 75)));
                blackSlimesRandomPattern.numberToSummon = (int)(Math.Round(randomScale * (freeplayBlackNumberScale * (waveNumber - 30))));
                pinkSlimesRandomPattern.numberToSummon = (int)(Math.Round(randomScale * (freeplayPinkNumberScale * (waveNumber - 30))));

                yellowSlimesRandomPattern.summonInterval = 0.1f;
                pinkSlimesRandomPattern.summonInterval = UnityEngine.Random.Range(1.0f, 2.0f);
                blackSlimesRandomPattern.summonInterval = UnityEngine.Random.Range(2.0f, 4.0f);

                EnemyManager.Instance.AddWavePattern(yellowSlimesRandomPattern);
                EnemyManager.Instance.AddWavePattern(blackSlimesRandomPattern);
                EnemyManager.Instance.AddWavePattern(pinkSlimesRandomPattern);
                
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
