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

    public override void OnTier1Upgrade()
    {
        upgrade1Unlocked = true;
        tower.baseRange *= 1.25f;
        return;
    }
    public override void OnTier2Upgrade()
    {
        upgrade2Unlocked = true;
        slowFactor = 2.0f;
        return;
    }
    public override void OnTier3Upgrade()
    {
        upgrade3Unlocked = true;
        slowDuration = 2000; // it's in frames???
        return;
    }
}
