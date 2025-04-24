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
    private float rbResonanceBuff = 1.25f;

    public bool boostedExplosion;
    public bool scaledDamage;

    public void InitializeProjectile()
    {
        remainingLifetime = speed == 0 ? 0 : range / speed;
        alreadyExploded = false;
    }

    private void Update()
    {
        float dt = Time.deltaTime;

        remainingLifetime -= dt;
        if (remainingLifetime < 0)
        {
            ProjectileManager.Instance.aoeProjectilePool.Release(this);
        }
    }

    private void FixedUpdate()
    {
        float fdt = Time.fixedDeltaTime;

        transform.position += (Vector3)(speed * fdt * direction );
        if (Synergy.Instance.rbSynergy && 
            parentTower.tower.synergyType == SynergyType.Red || 
            parentTower.tower.synergyType == SynergyType.Blue)
        {
                damageScaleFactor = rbResonanceBuff;
        }
        else
        {
            damageScaleFactor = 1.0f;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            EnemyManager.Instance.EnemyTakeDamage(collision.gameObject, baseDamage * damageScaleFactor);
            CreateExplosion();

            if (!pierces)
            {
                remainingLifetime = 10f;
                ProjectileManager.Instance.aoeProjectilePool.Release(this);
            }
            // enemy.hp -= damage;
        }
    }

    private void CreateExplosion()
    {
        if (alreadyExploded) return;

        GameObject projectileExplosion = Instantiate(explosion);
        projectileExplosion.transform.position = transform.position;
        ExplosionEffect exp =  projectileExplosion.GetComponent<ExplosionEffect>();
        exp.parentTower = this.parentTower;
        if (boostedExplosion) 
        {
            exp.baseDamage *= 3.0f;
            exp.explosionRadiusScaleFactor *= 1.5f;
        }
        if (scaledDamage)
        {
            exp.scaledDmg = true;
        }
        projectileExplosion.SetActive(true);

        AudioManager.Instance.PlayOneShot(AudioManager.Instance.explosionSound);

        alreadyExploded = true;
    }

    public void UpdateEffectiveDamage(float scaleFactor)
    {
        effectiveDamage = baseDamage * scaleFactor;
    }
}
