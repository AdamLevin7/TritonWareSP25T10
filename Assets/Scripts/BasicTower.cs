using UnityEngine;

public class BasicTower : TowerBehavior
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform cannon;

    public override void Fire()
    {
        Vector2 direction = tower.target.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        cannon.eulerAngles = new Vector3(0, 0, angle - 90);

        GameObject newBullet = Instantiate(bulletPrefab);
        newBullet.transform.position = Tower.Get3DTargetPos(tower.target);
        newBullet.transform.eulerAngles = new Vector3(0, 0, angle);
        Projectile newProjectile = newBullet.GetComponent<Projectile>();
        newProjectile.direction = direction.normalized;
        newProjectile.parentTowerClass = this;
        if(tower.towerUpgrade != null)
        {
            newProjectile.UpdateEffectiveDamage(tower.towerUpgrade.damageIncrease);
        }
        AudioManager.Instance.PlayOneShot(AudioManager.Instance.whooshSound);
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
