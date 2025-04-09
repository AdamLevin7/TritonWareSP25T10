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

    void Update()
    {
        explosionCtr += Time.deltaTime;
        if (explosionCtr > explosionDuration)
        {
            Destroy(gameObject);
        }   
    }

    void FixedUpdate()
    {
        if (!colliderActive)
        {
            circleCollider.enabled = false;
        }
    }

    // explosion should only hit every object once
    void OnCollisionEnter2D(Collision2D collision)
    {
        // collision.gameObject.GetComponent<Enemy>().hp -= dmg;
        colliderActive = false;
        Debug.Log("collided with object");
    }
}
