using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "ScriptableObjects/Enemy")]
public class EnemyData : ScriptableObject
{
    public GameObject enemyPrefab;
    public int value;
    public float hp;
    public float speed;
}
