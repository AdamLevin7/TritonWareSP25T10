using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed;
    public float range;
    public bool pierces = false;

    private float maxLifetime;
    private float remainingLifetime;

    public Vector2 direction = Vector2.right;

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            if (!pierces) Destroy(gameObject);
        }
    }
}
