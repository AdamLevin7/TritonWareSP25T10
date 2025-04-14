using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed;
    public float range;
    public bool pierces = false;
    public float baseDamage = 5f;

    private float maxLifetime;
    private float remainingLifetime;

    public Vector2 direction = Vector2.right;
    public GameObject parentTower;
    public TowerBehavior parentTowerClass;
    public float damageScaleFactor;
    public float effectiveDamage;

    private void Awake()
    {
        remainingLifetime = speed == 0 ? 0 : range / speed;
    }

    private void Update()
    {
        float dt = Time.deltaTime;

        remainingLifetime -= dt;
        if (remainingLifetime < 0)
        {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        float fdt = Time.fixedDeltaTime;

        transform.position += (Vector3)(speed * fdt * direction );
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
            EnemyManager.Instance.EnemyTakeDamage(collision.gameObject, baseDamage * damageScaleFactor);
            if (!pierces) 
            {
                parentTowerClass.tower.totalDamageDealt += baseDamage * damageScaleFactor;
                Destroy(gameObject);
            }
        }
    }

    public void UpdateEffectiveDamage(float scaleFactor)
    {
        effectiveDamage = baseDamage * scaleFactor;
    }
}
