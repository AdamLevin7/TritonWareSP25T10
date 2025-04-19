using System.Linq;
using UnityEngine;

public class ExplosionEffect : MonoBehaviour
{
    public float baseDamage;
    public float explosionRadiusScaleFactor;

    [SerializeField] float explosionCtr;
    public float explosionDuration;
    [SerializeField] private CircleCollider2D circleCollider;

    public TowerBehavior parentTower;
    public float damageScaleFactor = 1;
    public float effectiveDamage;

    private bool colliderActive;
    [SerializeField] private GameObject synergyManager;
    private float rbResonanceBuff = 1.25f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        colliderActive = true;
        circleCollider.radius *= explosionRadiusScaleFactor;
        Vector3 scale3D = new (explosionRadiusScaleFactor, explosionRadiusScaleFactor, 1);
        explosionCtr = 0.0f;
        transform.localScale = scale3D;
    }
    void Awake(){
        synergyManager = GameObject.FindWithTag("synergy");
    }
    void DoExplodeDamage()
    {
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, circleCollider.radius);

        foreach (Collider enemy in hitEnemies)
        {
            if (enemy.CompareTag("Enemy"))
            {
                if(synergyManager.GetComponent<Synergy>().rbSynergy && 
                    parentTower.tower.synergyType.ToString() == "Red" || 
                    parentTower.tower.synergyType.ToString() == "Blue"){
                        damageScaleFactor = rbResonanceBuff;
                }
                else{
                    damageScaleFactor = 1.0f;
                }
                parentTower.tower.totalDamageDealt += baseDamage * damageScaleFactor;
                EnemyManager.Instance.EnemyTakeDamage(enemy.gameObject, baseDamage * damageScaleFactor);
            }
        }

        colliderActive = false;
    }

    void Update()
    {
        if (colliderActive) DoExplodeDamage();

        explosionCtr += Time.deltaTime;
        if (explosionCtr > explosionDuration)
        {
            Destroy(gameObject);
        }   
    }

    public void UpdateEffectiveDamage(float scaleFactor)
    {
        effectiveDamage = baseDamage * scaleFactor;
    }
}
