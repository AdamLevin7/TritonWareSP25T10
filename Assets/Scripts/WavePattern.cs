using UnityEngine;

[CreateAssetMenu(fileName = "WavePattern", menuName = "ScriptableObjects/WavePattern")]
public class WavePattern: ScriptableObject
{
    public EnemyData enemyToSummon; // enemy to Summon
    public int numberToSummon;
    public float summonInterval; // seconds
    public float initialDelay; // initial delay before starting summoning in seconds
}
