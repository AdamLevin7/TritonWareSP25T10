using UnityEngine;

public class AOETower : TowerBehavior
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform cannon;

    public override void Fire()
    {
        Vector2 direction = tower.target.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        cannon.eulerAngles = new Vector3(0, 0, angle - 90);

        GameObject newBullet = Instantiate(bulletPrefab);
        newBullet.transform.position = (Vector2)transform.position;
        newBullet.transform.position += EnemyManager.Instance.TryGetEnemy(tower.target.gameObject).halfHeight * Vector3.back;
        newBullet.transform.eulerAngles = new Vector3(0, 0, angle);
        AOEProjectile newProjectile = newBullet.GetComponent<AOEProjectile>();
        newProjectile.direction = direction.normalized;
        newProjectile.parentTower = this;
        newProjectile.boostedExplosion = false;
        newProjectile.scaledDamage = false;
        if (upgrade2Unlocked) 
        {
            newProjectile.baseDamage *= 3.0f;
            newProjectile.boostedExplosion = true;
        }
        if (upgrade3Unlocked)
        {
            newProjectile.scaledDamage = true;
        }
    }

    public override void OnTier1Upgrade()
    {
        upgrade1Unlocked = true;
        tower.baseRange *= 1.25f;
        tower.shootCooldown *= 0.66f;
        return;
    }
    public override void OnTier2Upgrade()
    {
        upgrade2Unlocked = true;
        return;
    }
    public override void OnTier3Upgrade()
    {
        upgrade3Unlocked = true;
        tower.shootCooldown *= 0.5f;
        return;
    }
}
