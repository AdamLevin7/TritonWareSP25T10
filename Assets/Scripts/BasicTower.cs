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

        GameObject newBullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        newBullet.transform.eulerAngles = new Vector3(0, 0, angle);
        Projectile newProjectile = newBullet.GetComponent<Projectile>();
        newProjectile.direction = direction.normalized;
        newProjectile.parentTowerClass = this;

        AudioManager.Instance.PlayOneShot(AudioManager.Instance.whooshSound);
    }
}
