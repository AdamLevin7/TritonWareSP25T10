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
    [SerializeField] private GameObject synergyManager;
    private float rbResonanceBuff = 1.25f;

    private void Awake()
    {
        remainingLifetime = speed == 0 ? 0 : range / speed;
        synergyManager = GameObject.FindWithTag("synergy");
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
        if(synergyManager.GetComponent<Synergy>().rbSynergy && 
            parentTower.tower.synergyType.ToString() == "Red" || 
            parentTower.tower.synergyType.ToString() == "Blue"){
                damageScaleFactor = rbResonanceBuff;
        }
        else{
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
