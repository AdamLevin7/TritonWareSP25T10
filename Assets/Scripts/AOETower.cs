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
    }
}
