using System;
using System.Linq;
using UnityEngine;

public class ExplosionEffect : MonoBehaviour
{
    public float baseDamage;
    public float explosionRadiusScaleFactor;

    [SerializeField] float explosionCtr;
    public float explosionDuration;
    [SerializeField] private CircleCollider2D circleCollider;
    [SerializeField] private Transform visual;

    public TowerBehavior parentTower;
    public float damageScaleFactor = 1.0f;
    public float effectiveDamage;

    [SerializeField] private GameObject synergyManager;
    private float rbResonanceBuff = 1.25f;

    public bool scaledDmg;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    void OnEnable()
    {
        circleCollider.radius *= explosionRadiusScaleFactor;
        Vector3 scale3D = new (explosionRadiusScaleFactor, explosionRadiusScaleFactor, 1);
        explosionCtr = 0.0f;
        transform.localScale = scale3D;
        visual.localScale *= explosionRadiusScaleFactor;
        DoExplodeDamage();
    }

    void Awake(){
        synergyManager = GameObject.FindWithTag("synergy");
    }

    void DoExplodeDamage()
    {
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, 0.5f * explosionRadiusScaleFactor * 1.5f);

        foreach (Collider enemy in hitEnemies)
        {
            if (enemy.CompareTag("Enemy"))
            {
                if(synergyManager.GetComponent<Synergy>().rbSynergy && 
                    parentTower.tower.synergyType.ToString() == "Red" || 
                    parentTower.tower.synergyType.ToString() == "Blue"){
                        damageScaleFactor *= rbResonanceBuff;
                }
                float damageDoneToEnemy = baseDamage *= damageScaleFactor;
                if (scaledDmg)
                {
                    damageDoneToEnemy += Math.Max(enemy.gameObject.GetComponent<Enemy>().hp * 0.2f, 5);
                }
                parentTower.tower.totalDamageDealt += damageDoneToEnemy;
                EnemyManager.Instance.EnemyTakeDamage(enemy.gameObject, damageDoneToEnemy);
            }
        }
    }

    public void EndAnim()
    {
        Destroy(gameObject);
    }

    public void UpdateEffectiveDamage(float scaleFactor)
    {
        effectiveDamage = baseDamage * scaleFactor;
    }
}
