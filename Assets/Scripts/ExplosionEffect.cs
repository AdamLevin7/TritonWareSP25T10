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
    [SerializeField] private LayerMask ignoreSelfMask;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        colliderActive = true;
        Vector3 scale3D = new (explosionRadiusScaleFactor, explosionRadiusScaleFactor, 1);
        explosionCtr = 0.0f;
        transform.localScale = scale3D;
    }

    void OnEnable()
    {
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, circleCollider.radius, ignoreSelfMask);
        Debug.Log("enemies hit: " + hitEnemies.Length);

        foreach (Collider enemy in hitEnemies)
        {
            return;
            // enemy.SendMessage("TakeDamage", damage);
        }
    }

    void Update()
    {
        explosionCtr += Time.deltaTime;
        if (explosionCtr > explosionDuration)
        {
            Destroy(gameObject);
        }   
    }

}
