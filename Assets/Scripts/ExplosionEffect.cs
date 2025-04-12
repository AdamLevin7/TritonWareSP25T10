using System.Linq;
using UnityEngine;

public class ExplosionEffect : MonoBehaviour
{
    public float damage;
    public float explosionRadiusScaleFactor;

    [SerializeField] float explosionCtr;
    public float explosionDuration;
    [SerializeField] private CircleCollider2D circleCollider;

    private bool colliderActive;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        colliderActive = true;
        Vector3 scale3D = new (explosionRadiusScaleFactor, explosionRadiusScaleFactor, 1);
        explosionCtr = 0.0f;
        transform.localScale = scale3D;
    }

    void DoExplodeDamage()
    {
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, circleCollider.radius);

        foreach (Collider enemy in hitEnemies)
        {
            if (enemy.CompareTag("Enemy"))
            {
                EnemyManager.Instance.EnemyTakeDamage(enemy.gameObject, (int)damage);
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

}
