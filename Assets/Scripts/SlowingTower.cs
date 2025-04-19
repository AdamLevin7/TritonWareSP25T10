using UnityEngine;

public class SlowingTower : TowerBehavior
{
    [SerializeField] private uint slowDuration;
    [SerializeField] private float slowFactor;
	[SerializeField] private float damage;
	private int counter;

    public override void Fire()
    {
        Collider[] overlapColliders = Physics.OverlapSphere(tower.transform.position, tower.effectiveRange);

        foreach (var collider in overlapColliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                EnemyManager.Instance.EnemySlowed(collider.gameObject, slowDuration, slowFactor);
				if(counter % 10 == 0)
				{
					EnemyManager.Instance.EnemyTakeDamage(collider.gameObject, damage);
					counter = 0;
				}
				else 
				{
					counter++;
				}
            }
        }
    }

    public override void OnTier1Upgrade()
    {
        return;
    }
    public override void OnTier2Upgrade()
    {
        return;
    }
    public override void OnTier3Upgrade()
    {
        return;
    }
}
