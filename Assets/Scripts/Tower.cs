using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    public float shootCooldown = 0.5f;
    private float timeSinceLastShoot = 0f;

    public Transform target;

    private void Update()
    {
        float dt = Time.deltaTime;

        timeSinceLastShoot -= dt;

        if (timeSinceLastShoot < 0f)
        {
            Vector2 direction = target.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            GameObject newBullet = Instantiate(bulletPrefab);
            newBullet.transform.position = transform.position;
            newBullet.transform.eulerAngles = new Vector3(0, 0, angle);
            Projectile newProjectile = newBullet.GetComponent<Projectile>();
            newProjectile.direction = direction.normalized;

            timeSinceLastShoot = shootCooldown;
        }
    }
}
