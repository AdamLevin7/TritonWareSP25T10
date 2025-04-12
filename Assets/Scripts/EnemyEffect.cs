using UnityEngine;

// effects limited to changing HP and movespeed for now.
[CreateAssetMenu(fileName = "EnemyEffect", menuName = "ScriptableObjects/EnemyEffect")]
public class EnemyEffect : ScriptableObject
{
    public uint timeLeft;
    public int hpChange;
    public float moveSpeedChange;
}
