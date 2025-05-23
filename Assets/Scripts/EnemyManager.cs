using UnityEngine;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine.Pool;

public class Enemy
{
    public EnemyContainer enemyObject;
    public float halfHeight;
    public int currentNodeIdx;
    public Vector2 direction;
    public float moveSpeed;
    public float hp;
    public float maxHealth;

    public float slowFactor;
    public uint slowDuration;
    public float moveSpeedReduction;
    public bool wasSlowed;

    public Enemy(EnemyContainer enemyObject, float halfHeight, int currentNodeIdx, Vector2 direction, float moveSpeed, float hp)
    {
        this.enemyObject = enemyObject;
        this.halfHeight = halfHeight;
        this.currentNodeIdx = currentNodeIdx;
        this.direction = direction;
        this.moveSpeed = moveSpeed;
        this.hp = hp;
        maxHealth = hp;

        slowFactor = 1;
        slowDuration = 0;
        moveSpeedReduction = 0;
        wasSlowed = false;
    }
}

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;

        enemyPool = new(
            () => Instantiate(EnemyPrefab),
            enemy => enemy.gameObject.SetActive(true),
            enemy => enemy.gameObject.SetActive(false),
            enemy => Destroy(enemy.gameObject),
            false,
            MAX_ENEMIES
        );
    }

    public int totalEnemiesKilled = 0;

    //Enemy Specific References Paired by Index
    public EnemyContainer EnemyPrefab;
    private ObjectPool<EnemyContainer> enemyPool;
    public List<Enemy> activeEnemies = new();
    public List<Enemy> deadEnemies = new();
    public int totalEnemiesInWave;
    public int enemiesKilledThisWave;
    //References for Creating Node Path
    public Transform NodeContainer;
    public List<Vector3> NodePositionList = new();

    public float defaultEnemyMoveSpeed = 2f;

    // round wave stuff
    // synced by index
    public List<WavePattern> waveQueue = new();
    private List<float> wavePatternsIntervalCtrs = new();
    private List<float> wavePatternsInitDelayCtrs = new();
    private List<float> wavePatternsAlreadySummonedCounts = new();

    private const int MAX_ENEMIES = 128;

    void DummyAddMoney()
    {
        if(Synergy.Instance.totalSynergy){
            GameManager.Instance.money += 10;
        }
        else{
            GameManager.Instance.money += 5;
        }
    }

    /// <summary>
    /// Tries to get the alive enemy with the matching GameObject
    /// </summary>
    /// <param name="enemyGameObject">The enemy's GameObject</param>
    /// <returns>The Matching Enemy</returns>
    public Enemy TryGetEnemy(GameObject enemyGameObject)
    {
        for (int i = 0; i < activeEnemies.Count; i++)
        {
            Enemy enemy = activeEnemies[i];
            if (enemy.enemyObject.gameObject.Equals(enemyGameObject)) return enemy;
        }

        for (int i = 0; i < deadEnemies.Count; i++)
        {
            Enemy enemy = deadEnemies[i];
            if (enemy.enemyObject.gameObject.Equals(enemyGameObject))
            {
                Debug.LogWarning("tried to get enemy + " + enemyGameObject + ", but they were dead");
                return null;
            }
        }

        Debug.LogError("tried to get enemy + " + enemyGameObject + ", but they were in neither aliveEnemies nor deadEnemies");
        return null;
    }

    /// <summary>
    /// Deals <paramref name="damageHPAmount"/> to the enemy whose game object is <paramref name="EnemyReference"/>. 
    /// Will kill the enemy if damage cause the enemy's hp to go below 0.
    /// </summary>
    /// <param name="EnemyReference">GameObject of the enemy to deal damage to</param>
    /// <param name="damageHPAmount">The amount of damage to deal to the enemy</param>
    public void EnemyTakeDamage(GameObject EnemyReference, float damageHPAmount)
    {
        Enemy enemy = TryGetEnemy(EnemyReference);
        if (enemy == null) return;

        enemy.hp -= damageHPAmount;

        enemy.enemyObject.healthBarSlider.value = enemy.hp / enemy.maxHealth;
        AudioManager.Instance.PlayOneShot(AudioManager.Instance.enemyHitSound);
        if (enemy.hp < 0)
        {
            DummyAddMoney();
            KillEnemy(enemy);
        }
    }

    /// <summary>
    /// Creates a new Enemy with an initial hp <paramref name="TotalHP"/>
    /// </summary>
    /// <param name="TotalHP">The enemy's initial hp</param>
    public void CreateNewEnemy(EnemyData enemyData)
    {
        EnemyContainer newEnemy = enemyPool.Get();
        newEnemy.sphereCollider.enabled = true;
        float halfHeight = newEnemy.sphereCollider.radius;
        newEnemy.transform.position = NodePositionList[0] + halfHeight * Vector3.back;
        newEnemy.meshRenderer.materials[0].SetColor("_BaseColor", enemyData.color);
        newEnemy.healthBarSlider.value = 1.0f;
        activeEnemies.Add(new Enemy(newEnemy, halfHeight, 0, Vector2.zero, enemyData.speed, enemyData.hp));
    }

    /// <summary>
    /// Checks if <paramref name="enemyRef"/> corresponds to a dead enemy.
    /// </summary>
    /// <param name="enemyRef">An enemy GameObject</param>
    /// <returns></returns>
    public bool EnemyIsDead(GameObject enemyRef)
    {
        for (int i = 0; i < deadEnemies.Count; i++)
        {
            Enemy enemy = deadEnemies[i];
            if (enemy.enemyObject.gameObject.Equals(enemyRef)) return true;
        }

        return false;
    }

    void Start()
    {
        // This should be done on load of the scene with the path
        foreach (Transform child in NodeContainer)
        {
            NodePositionList.Add(child.position);
        }

    }

    // Update is called once per frame
    void Update()
    {
        // kill enemies for reals 
        for (int i = 0; i < deadEnemies.Count; ++i)
        {
            Enemy enemy = deadEnemies[i];
            enemyPool.Release(enemy.enemyObject);
            deadEnemies.Remove(enemy);
            enemiesKilledThisWave++;
        }

        // update positions of currently alive enemies
        for (int i = 0; i < activeEnemies.Count; ++i)
        {
            Enemy enemy = activeEnemies[i];

            if(enemy.slowDuration > 0)
            {
                enemy.slowDuration--;
            }
            if(!enemy.wasSlowed)
            {
                enemy.wasSlowed = true;
                enemy.moveSpeedReduction = enemy.moveSpeed - enemy.moveSpeed/enemy.slowFactor;
                enemy.moveSpeed = enemy.moveSpeed - enemy.moveSpeedReduction;
            }
            else if(enemy.slowDuration == 0 && enemy.wasSlowed)
            {
                enemy.wasSlowed = false;
                enemy.moveSpeed = enemy.moveSpeed + enemy.moveSpeedReduction;
                // enemy.slowFactor = 1;
                enemy.moveSpeedReduction = 0;
            }

            if (enemy.currentNodeIdx + 1 < NodePositionList.Count)
            {
                enemy.direction = (NodePositionList[enemy.currentNodeIdx + 1] - NodePositionList[enemy.currentNodeIdx]).normalized;
                Vector2 movement = enemy.moveSpeed * Time.deltaTime * enemy.direction;
                enemy.enemyObject.gameObject.transform.position += (Vector3)movement;

                float DistanceToNode = Vector2.Distance(enemy.enemyObject.gameObject.transform.position, NodePositionList[enemy.currentNodeIdx + 1]);

                // changed this to look a little smoother
                if (DistanceToNode < movement.magnitude)
                {

                    enemy.enemyObject.gameObject.transform.position = NodePositionList[enemy.currentNodeIdx + 1] + enemy.halfHeight * Vector3.back;
                    enemy.currentNodeIdx++;

                    // new code! -aiden
                    // if hits last node, delete enemy and subtract life
                    if (enemy.currentNodeIdx >= NodePositionList.Count - 1)
                    {
                        // arbitrary number for now
                        GameManager.Instance.lives -= (int)Mathf.Max(0, enemy.hp);
                        enemiesKilledThisWave++;
                        activeEnemies.Remove(enemy);
                        enemyPool.Release(enemy.enemyObject);

                        continue;
                    }
                }

            }
        }

        // active round wave stuff
        if (GameManager.Instance.isWaveActive)
        {
            UpdateWavePatterns();

            // if all enemies are dead, end round
            if (totalEnemiesInWave <= enemiesKilledThisWave)
            {
                GameManager.Instance.EndWave();
                EnemyManager.Instance.totalEnemiesInWave = 0;
                EnemyManager.Instance.enemiesKilledThisWave = 0;
            }
        }
    }

    /// <summary>
    /// Kills the <paramref name="enemy"/>. Does not immediately destroy the <paramref name="enemy"/>'s GameObject
    /// </summary>
    /// <param name="enemy">The enemy to kill</param>
    public void KillEnemy(Enemy enemy)
    {
        enemy.enemyObject.sphereCollider.enabled = false;
        activeEnemies.Remove(enemy);
        deadEnemies.Add(enemy);

        AudioManager.Instance.PlayOneShot(AudioManager.Instance.enemyDeathSound);
    }

    public void AddWavePattern(WavePattern wave)
    {
        waveQueue.Add(wave);
        wavePatternsAlreadySummonedCounts.Add(0.0f);
        wavePatternsInitDelayCtrs.Add(0.0f);
        wavePatternsIntervalCtrs.Add(0.0f);
    }

    private void UpdateWavePatterns()
    {
        // probably not optimized
        for (int i = 0; i < waveQueue.Count; i++)
        {
            // already summoned all towers in pattern
            if (wavePatternsAlreadySummonedCounts[i] >= waveQueue[i].numberToSummon) 
            {
                continue;
            }
            // check if initial delay has passed
            else if (wavePatternsInitDelayCtrs[i] < waveQueue[i].initialDelay)
            {
                wavePatternsInitDelayCtrs[i] += Time.deltaTime;
                continue;
            }
            // check if enough time has passed to spawn another enemy
            else if (wavePatternsIntervalCtrs[i] < waveQueue[i].summonInterval)
            {
                wavePatternsIntervalCtrs[i] += Time.deltaTime;
                continue;
            }
            // FINALLY, summon an enemy lol
            else
            {
                // enemy tyes do not yet exist
                CreateNewEnemy(waveQueue[i].enemyToSummon);
                wavePatternsAlreadySummonedCounts[i]++;
                wavePatternsIntervalCtrs[i] = 0.0f;
                continue;
            }
        }
    }

    public void ClearWavePatterns()
    {
        waveQueue.Clear();
        wavePatternsAlreadySummonedCounts.Clear();
        wavePatternsInitDelayCtrs.Clear();
        wavePatternsIntervalCtrs.Clear();

        activeEnemies.Clear();
        deadEnemies.Clear();
    }
    public void EnemySlowed(GameObject EnemyReference, uint duration, float factor)
    {

        Enemy enemy = TryGetEnemy(EnemyReference);
        if (enemy == null) return;

        SlowEnemy(enemy, duration, factor);
    }

    // following a different naming scheme
    public void SlowEnemy(Enemy enemy, uint duration, float factor)
    {
        if(enemy.wasSlowed && enemy.slowFactor == factor)
        {
            enemy.slowDuration = duration;
        }
        else if(enemy.wasSlowed)
        {
            enemy.slowFactor = factor;
            enemy.slowDuration = duration; 
            float formerTotalSpeed = enemy.moveSpeed + enemy.moveSpeedReduction;
            float newReduction = formerTotalSpeed - (formerTotalSpeed / factor);
            float oldSpeed = enemy.moveSpeed;
            enemy.moveSpeedReduction = newReduction;
            enemy.moveSpeed = formerTotalSpeed - newReduction;
        }
        else 
        {
            enemy.slowFactor = factor;
            enemy.slowDuration = duration;
        }
    }
}
