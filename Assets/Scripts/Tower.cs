using Unity.VisualScripting;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    public float shootCooldown = 0.5f;
    public float range = 10;
    private float timeSinceLastShoot = 0f;

    public Transform target;

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, range);
    }

    private void Update()
    {
        float dt = Time.deltaTime;

        timeSinceLastShoot -= dt;

        if (timeSinceLastShoot < 0f && target != null)
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

    private void FixedUpdate()
    {
        Collider2D[] inRange = Physics2D.OverlapCircleAll(transform.position, range);

        target = null;
        float minDistance = float.MaxValue;

        foreach (Collider2D collider in inRange)
        {
            if (collider.CompareTag("Enemy"))
            {
                float squaredDistance = Vector2.SqrMagnitude(collider.transform.position - transform.position);
                if (squaredDistance < minDistance)
                {
                    minDistance = squaredDistance;
                    target = collider.transform;
                }
            }
        }
    }
}
