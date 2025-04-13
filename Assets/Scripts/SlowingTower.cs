using UnityEngine;

public class SlowingTower : TowerBehavior
{
    [SerializeField] private uint slowDuration;
    [SerializeField] private float slowFactor;
    [SerializeField] private uint slowRadius;
    public override void Fire()
    {
        Collider[] overlapColliders = Physics.OverlapSphere(tower.transform.position, slowRadius);
        Debug.Log("Position: " + transform.position.ToString());

        foreach (var collider in overlapColliders)
        {
            Debug.Log(collider.gameObject);
            if (collider.CompareTag("Enemy"))
            {
                EnemyManager.Instance.EnemySlowed(collider.gameObject,
                        slowDuration, slowFactor);
                Debug.Log("Slowing something!");
            }
        }
    }
}
