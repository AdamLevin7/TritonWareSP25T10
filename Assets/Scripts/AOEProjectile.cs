using UnityEngine;
using UnityEngine.Rendering;

public class AOEProjectile : MonoBehaviour
{
    public float baseDamage;
    
    public float speed;
    public float range;
    public bool pierces = false;

    private float maxLifetime;
    private float remainingLifetime;

    private bool alreadyExploded = false;
    public TowerBehavior parentTower;

    public Vector2 direction = Vector2.right;
    [SerializeField] private GameObject explosion;
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
        if (collision.gameObject.CompareTag("Enemy"))
        {
            EnemyManager.Instance.EnemyTakeDamage(collision.gameObject, baseDamage * damageScaleFactor);
            CreateExplosion();

            if (!pierces)
            {
                Destroy(gameObject);
            }
            // enemy.hp -= damage;
        }
    }

    private void CreateExplosion()
    {
        if (alreadyExploded) return;

        GameObject projectileExplosion = Instantiate(explosion);
        projectileExplosion.transform.position = transform.position;
        projectileExplosion.GetComponent<ExplosionEffect>().parentTower = this.parentTower;
        projectileExplosion.SetActive(true);

        AudioManager.Instance.PlayOneShot(AudioManager.Instance.explosionSound);

        alreadyExploded = true;
    }

    public void UpdateEffectiveDamage(float scaleFactor)
    {
        effectiveDamage = baseDamage * scaleFactor;
    }
}
