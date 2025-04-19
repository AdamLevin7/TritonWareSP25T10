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
    [SerializeField] private GameObject synergyManager;
    private float rbResonanceBuff = 1.25f;

    private void Awake()
    {
        damageScaleFactor = 1.0f;
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
            parentTowerClass.tower.synergyType.ToString() == "Red" || 
            parentTowerClass.tower.synergyType.ToString() == "Blue"){
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
                Destroy(gameObject);
            }
        }
    }

    public void UpdateEffectiveDamage(float scaleFactor)
    {
        effectiveDamage = baseDamage * scaleFactor;
    }
}
