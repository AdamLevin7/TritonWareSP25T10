using System;
using UnityEngine;

public class BasicTower : TowerBehavior
{
    [SerializeField] private Transform cannon;
    [SerializeField] private Material material;

    private const float TRIPLE_SPREAD = (float)Math.PI / 36.0f; 

    private void CreateProjectile(float angle, Vector2 direction)
    {
        Projectile newProjectile = ProjectileManager.Instance.basicProjectilePool.Get();
        newProjectile.transform.position = transform.position;
        newProjectile.transform.eulerAngles = new Vector3(0, 0, angle);
        newProjectile.direction = direction.normalized;
        if (upgrade1Unlocked) newProjectile.baseDamage *= 2.0f;
        newProjectile.parentTowerClass = this;
        newProjectile.meshRenderer.material = material;
    }

    // rotation matrix: (xcostheta - ysintheta, xsintheta + ycostheta)
    public override void Fire()
    {
        Vector2 direction = tower.target.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        cannon.eulerAngles = new Vector3(0, 0, angle - 90);

        CreateProjectile(angle, direction);

        if (upgrade2Unlocked)
        {
            CreateProjectile(angle, MatrixRotate(direction.normalized, TRIPLE_SPREAD));
            CreateProjectile(angle, MatrixRotate(direction.normalized, -TRIPLE_SPREAD));
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
