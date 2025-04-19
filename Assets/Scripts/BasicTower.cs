using System;
using UnityEngine;

public class BasicTower : TowerBehavior
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform cannon;

    private const float TRIPLE_SPREAD = (float)Math.PI / 36.0f; 

    // rotation matrix: (xcostheta - ysintheta, xsintheta + ycostheta)
    public override void Fire()
    {
        Vector2 direction = tower.target.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        cannon.eulerAngles = new Vector3(0, 0, angle - 90);

        GameObject newBullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        newBullet.transform.eulerAngles = new Vector3(0, 0, angle);
        Projectile newProjectile = newBullet.GetComponent<Projectile>();
        newProjectile.direction = direction.normalized;
        if (upgrade1Unlocked) newProjectile.baseDamage *= 2.0f;
        newProjectile.parentTowerClass = this;

        if (upgrade2Unlocked)
        {
            GameObject newBullet2 = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            GameObject newBullet3 = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

            newBullet2.transform.eulerAngles = new Vector3(0, 0, angle);
            newBullet3.transform.eulerAngles = new Vector3(0, 0, angle);
            Projectile newProjectile2 = newBullet2.GetComponent<Projectile>();
            Projectile newProjectile3 = newBullet3.GetComponent<Projectile>();
            
            newProjectile2.direction = MatrixRotate(direction.normalized, TRIPLE_SPREAD);
            newProjectile3.direction = MatrixRotate(direction.normalized, -TRIPLE_SPREAD);

            newProjectile2.baseDamage *= 2.0f;
            newProjectile3.baseDamage *= 2.0f;

            newProjectile2.parentTowerClass = this;
            newProjectile3.parentTowerClass = this;
        }
        
        AudioManager.Instance.PlayOneShot(AudioManager.Instance.whooshSound);
    }

    public override void OnTier1Upgrade()
    {
        upgrade1Unlocked = true;
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

    // in a tower class is nuts
    private Vector2 MatrixRotate(Vector2 dir, double angle)
    {
        return new(dir.x * (float)Math.Cos(angle) - dir.y * (float)Math.Sin(angle), dir.x * (float)Math.Sin(angle) + dir.y * (float)Math.Cos(angle));
    }
}
