using UnityEngine;
using System.Collections.Generic;
using JetBrains.Annotations;

public class Enemy
{
    public GameObject gameObject;
    public float halfHeight;
    public int currentNodeIdx;
    public Vector2 direction;
    public float moveSpeed;
    public int hp;

    public Enemy(GameObject gameObject, float halfHeight, int currentNodeIdx, Vector2 direction, float moveSpeed, int hp)
    {
        this.gameObject = gameObject;
        this.halfHeight = halfHeight;
        this.currentNodeIdx = currentNodeIdx;
        this.direction = direction;
        this.moveSpeed = moveSpeed;
        this.hp = hp;
    }
}

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

    //Enemy Specific References Paired by Index
    public GameObject EnemyPrefab;
    public List<Enemy> activeEnemies = new();
    public List<Enemy> deadEnemies = new();
    //References for Creating Node Path
    public Transform NodeContainer;
    public List<Vector3> NodePositionList = new();

    public float defaultEnemyMoveSpeed = 2f;

    // round wave stuff
    // synced by index
    public List<WavePattern> wavePatterns = new();
    public List<float> wavePatternsIntervalCtrs = new();
    public List<float> wavePatternsInitDelayCtrs = new();
    public List<float> wavePatternsAlreadySummonedCounts = new();

    [SerializeField] private List<WavePattern> possibleWavePatterns;

    void DummyAddMoney()
    {
        GameManager.Instance.money += 5;
        Debug.Log("Money added");
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
            if (enemy.gameObject.Equals(enemyGameObject)) return enemy;
        }

        for (int i = 0; i < deadEnemies.Count; i++)
        {
            Enemy enemy = deadEnemies[i];
            if (enemy.gameObject.Equals(enemyGameObject))
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
    public void EnemyTakeDamage(GameObject EnemyReference, int damageHPAmount)
    {
        Enemy enemy = TryGetEnemy(EnemyReference);
        if (enemy == null) return;

        enemy.hp -= damageHPAmount;
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
    public void CreateNewEnemy(int TotalHP)
    {
        GameObject newEnemy = Instantiate(EnemyPrefab, NodePositionList[0], Quaternion.identity);
        float newHalfHeight = newEnemy.GetComponent<SphereCollider>().radius;
        newEnemy.transform.position += newHalfHeight * Vector3.back;
        activeEnemies.Add(new Enemy(newEnemy, newHalfHeight, 0, Vector2.zero, defaultEnemyMoveSpeed, TotalHP));
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
            if (enemy.gameObject.Equals(enemyRef)) return true;
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

        // This should be done on event trigger for a new Enemy
        // CreateNewEnemy(10);
        // testing! - aiden
        AddWavePattern(possibleWavePatterns[0]);
    }

    // Update is called once per frame
    void Update()
    {
        // kill enemies for reals 
        for (int i = 0; i < deadEnemies.Count; ++i)
        {
            Enemy enemy = deadEnemies[i];
            Destroy(enemy.gameObject);
            deadEnemies.Remove(enemy);
        }

        // update positions of currently alive enemies
        for (int i = 0; i < activeEnemies.Count; ++i)
        {
            Enemy enemy = activeEnemies[i];

            if (enemy.currentNodeIdx + 1 < NodePositionList.Count)
            {
                enemy.direction = (NodePositionList[enemy.currentNodeIdx + 1] - NodePositionList[enemy.currentNodeIdx]).normalized;
                Vector2 movement = enemy.moveSpeed * Time.deltaTime * enemy.direction;
                enemy.gameObject.transform.position += (Vector3)movement;

                float DistanceToNode = Vector2.Distance(enemy.gameObject.transform.position, NodePositionList[enemy.currentNodeIdx + 1]);

                // changed this to look a little smoother
                if (DistanceToNode < movement.magnitude)
                {

                    enemy.gameObject.transform.position = NodePositionList[enemy.currentNodeIdx + 1] + enemy.halfHeight * Vector3.back;
                    enemy.currentNodeIdx++;

                    // new code! -aiden
                    // if hits last node, delete enemy and subtract life
                    if (enemy.currentNodeIdx >= NodePositionList.Count - 1)
                    {
                        // arbitrary number for now
                        GameManager.Instance.lives -= Mathf.Max(0, enemy.hp);
                        KillEnemy(enemy);
                        continue;
                    }
                }

            }
        }

        // active round wave stuff
        if (GameManager.Instance.isWaveActive)
        {
            UpdateWavePatterns();
        }

        //Test of new enemy creation and or deletion
        if (Input.GetKeyUp(KeyCode.J))
        { 
            CreateNewEnemy(10);
        }

        //Test Damage and Health
        if (Input.GetKeyUp(KeyCode.K))
        { 
            EnemyTakeDamage(activeEnemies[0].gameObject, 5);
        }
    }

    /// <summary>
    /// Kills the <paramref name="enemy"/>. Does not immediately destroy the <paramref name="enemy"/>'s GameObject
    /// </summary>
    /// <param name="enemy">The enemy to kill</param>
    public void KillEnemy(Enemy enemy)
    {
        enemy.gameObject.GetComponent<Collider>().enabled = false;
        activeEnemies.Remove(enemy);
        deadEnemies.Add(enemy);
    }

    public void AddWavePattern(WavePattern wave)
    {
        wavePatterns.Add(wave);
        wavePatternsAlreadySummonedCounts.Add(0.0f);
        wavePatternsInitDelayCtrs.Add(0.0f);
        wavePatternsIntervalCtrs.Add(0.0f);
    }

    private void UpdateWavePatterns()
    {
        // probably not optimized
        for (int i = 0; i < wavePatterns.Count; i++)
        {
            // already summoned all towers in pattern
            if (wavePatternsAlreadySummonedCounts[i] >= wavePatterns[i].numberToSummon) 
            {
                continue;
            }
            // check if initial delay has passed
            else if (wavePatternsInitDelayCtrs[i] < wavePatterns[i].initialDelay)
            {
                wavePatternsInitDelayCtrs[i] += Time.deltaTime;
                continue;
            }
            // check if enough time has psased to spawn another enemy
            else if (wavePatternsIntervalCtrs[i] < wavePatterns[i].summonInterval)
            {
                wavePatternsIntervalCtrs[i] += Time.deltaTime;
                continue;
            }
            // FINALLY, summon an enemy lol
            else
            {
                // certain enemies do not yet exist
                CreateNewEnemy(10);
                wavePatternsAlreadySummonedCounts[i]++;
                wavePatternsIntervalCtrs[i] = 0.0f;
                continue;
            }
        }
    }

    public void ClearWavePatterns()
    {
        wavePatterns.Clear();
        wavePatternsAlreadySummonedCounts.Clear();
        wavePatternsInitDelayCtrs.Clear();
        wavePatternsIntervalCtrs.Clear();

        activeEnemies.Clear();
        deadEnemies.Clear();
    }

    public void SummonWave()
    {
        return;
    }
}
