using UnityEngine;
using UnityEngine.Rendering;

public class EffectProjectile : MonoBehaviour
{
    public float damage;
    
    public float speed;
    public float range;
    public bool pierces = false;

    private float maxLifetime;
    private float remainingLifetime;

    public Vector2 direction = Vector2.right;
    [SerializeField] private GameObject explosion;
    [SerializeField] private EnemyEffect effect;
    [SerializeField] private bool isBuff;

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
            Enemy enemy = EnemyManager.Instance.TryGetEnemy(collision.gameObject);
            if(enemy == null) return;
            EnemyManager.Instance.EnemyTakeDamage(enemy, (int)damage);

            if(isBuff){
                enemy.passiveEffects.Add(effect);
            }
            else {
                enemy.activeEffects.Add(effect);
            }

            if (!pierces)
            {
                Destroy(gameObject);
            }
            // enemy.hp -= damage;
        }
    }
}
