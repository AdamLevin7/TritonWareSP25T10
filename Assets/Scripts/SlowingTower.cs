using UnityEngine;

public class SlowingTower : TowerBehavior
{
    [SerializeField] private uint slowDuration;
    [SerializeField] private float slowFactor;
	[SerializeField] private int damage;

    public override void Fire()
    {
        Collider[] overlapColliders = Physics.OverlapSphere(tower.transform.position, tower.effectiveRange);

        foreach (var collider in overlapColliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                EnemyManager.Instance.EnemySlowed(collider.gameObject, slowDuration, slowFactor);
				EnemyManager.Instance.EnemyTakeDamage(collider.gameObject, damage);
            }
        }
    }
}
