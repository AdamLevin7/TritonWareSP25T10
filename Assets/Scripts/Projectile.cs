using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed;
    public float range;
    public bool pierces = false;
    public float baseDamage = 1f;

    private float maxLifetime;
    private float remainingLifetime;

    public Vector2 direction = Vector2.right;
    public GameObject parentTower;
    public TowerBehavior parentTowerClass;
    public float damageScaleFactor;
    public float effectiveDamage;
    private float rbResonanceBuff = 1.25f;

    public MeshRenderer meshRenderer;

    public void InitializeProjectile()
    {
        damageScaleFactor = 1.0f;
        remainingLifetime = speed == 0 ? 0 : range / speed;
    }

    private void Update()
    {
        float dt = Time.deltaTime;

        remainingLifetime -= dt;
        if (remainingLifetime < 0)
        {
            ProjectileManager.Instance.basicProjectilePool.Release(this);
        }
    }

    private void FixedUpdate()
    {
        float fdt = Time.fixedDeltaTime;

        transform.position += (Vector3)(speed * fdt * direction );

        if(Synergy.Instance.rbSynergy && 
            parentTowerClass.tower.synergyType == SynergyType.Red || 
            parentTowerClass.tower.synergyType == SynergyType.Blue)
        {
                damageScaleFactor += rbResonanceBuff - 1.0f;
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
            EnemyManager.Instance.EnemyTakeDamage(collision.gameObject, baseDamage * damageScaleFactor);
            if (!pierces) 
            {
                parentTowerClass.tower.totalDamageDealt += baseDamage * damageScaleFactor;
                remainingLifetime = 10f;
                ProjectileManager.Instance.basicProjectilePool.Release(this);
            }
        }
    }

    public void UpdateEffectiveDamage(float scaleFactor)
    {
        effectiveDamage = baseDamage * scaleFactor;
    }
}
