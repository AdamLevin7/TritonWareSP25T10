using UnityEngine;
using UnityEngine.Rendering;

public class AOEProjectile : MonoBehaviour
{
    public float damage;
    
    public float speed;
    public float range;
    public bool pierces = false;

    private float maxLifetime;
    private float remainingLifetime;

    public Vector2 direction = Vector2.right;
    [SerializeField] private GameObject explosion;

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

        if (Input.GetKeyUp(KeyCode.B))
        {
            CreateExplosion();
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
            CreateExplosion();

            if (!pierces) Destroy(gameObject);
            // enemy.hp -= damage;
        }
    }

    private void CreateExplosion()
    {
        Debug.Log("kaboom");
        GameObject projectileExplosion = Instantiate(explosion);
        projectileExplosion.transform.position = this.transform.position;
        explosion.SetActive(true);
    }
}
