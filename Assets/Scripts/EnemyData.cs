using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "ScriptableObjects/Enemy")]
public class EnemyData : ScriptableObject
{
    public GameObject enemyPrefab;
    public int value;
    public int hp;
    public float speed;
}
