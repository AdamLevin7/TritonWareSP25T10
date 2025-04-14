using UnityEngine;

// effects limited to changing HP and movespeed for now.
// Disabled for now. Uncomment when ready.
/*[CreateAssetMenu(fileName = "EnemyEffect", menuName = "ScriptableObjects/EnemyEffect")]*/
public class EnemyEffect : ScriptableObject
{
    public uint timeLeft;
    public int hpChange;
    public float moveSpeedChange;
}
