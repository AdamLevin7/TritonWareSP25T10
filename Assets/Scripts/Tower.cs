using System.Data.SqlTypes;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Tower : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private CircleCollider2D col;
    [SerializeField] private SpriteRenderer rangeIndicator;
    [SerializeField] private LayerMask towerLayer;
    [SerializeField] private LayerMask placementObstructionLayer;
    [Header("Colors")]
    [SerializeField] private Color defaultColor;
    [SerializeField] private Color placingValidColor;
    [SerializeField] private Color placingInvalidColor;
    [SerializeField] private Color rangeIndicatorValidColor;
    [SerializeField] private Color rangeIndicatorInvalidColor;
    private InputSystem_Actions inputActions;

    public float shootCooldown = 0.5f;
    public float range = 10;
    private float timeSinceLastShoot = 0f;

    private bool isPlaced = false;
    private bool canBePlaced = false;
    private bool isSelected = false;
    private bool wasClicked = false;
    public Transform target;

    private const int UI_LAYER_NUM = 5;
    private const int TOWER_LAYER_NUM = 6;

    public TowerData data;

    private void Awake()
    {
        inputActions = new();
        rangeIndicator.gameObject.SetActive(true);
        rangeIndicator.transform.localScale = new(range * 2, range * 2, range * 2);
        gameObject.layer = UI_LAYER_NUM;
    }

    private void OnEnable()
    {
        inputActions.Enable();
        inputActions.Player.Interact.started += StartInteract;
        inputActions.Player.Interact.canceled += Interact;
    }

    private void OnDisable()
    {
        inputActions.Player.Interact.canceled -= Interact;
        inputActions.Disable();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, range);
    }

    private void Update()
    {
        float dt = Time.deltaTime;

        if (isPlaced)
        {
            timeSinceLastShoot -= dt;

            if (timeSinceLastShoot < 0f && target != null)
            {
                Vector2 direction = target.position - transform.position;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

                GameObject newBullet = Instantiate(bulletPrefab);
                newBullet.transform.position = transform.position;
                newBullet.transform.eulerAngles = new Vector3(0, 0, angle);
                Projectile newProjectile = newBullet.GetComponent<Projectile>();
                newProjectile.direction = direction.normalized;

                timeSinceLastShoot = shootCooldown;
            }
        }
        else
        {
            Vector2 worldMousePos = Camera.main.ScreenToWorldPoint(inputActions.Player.MousePos.ReadValue<Vector2>());
            transform.position = worldMousePos;

            canBePlaced = !Physics2D.OverlapCircle(transform.position, col.radius, placementObstructionLayer);

            sprite.color = canBePlaced ? placingValidColor : placingInvalidColor;
            rangeIndicator.color = canBePlaced ? rangeIndicatorValidColor : rangeIndicatorInvalidColor;
        }
    }

    private void FixedUpdate()
    {
        if (!isPlaced) return;

        Collider2D[] inRange = Physics2D.OverlapCircleAll(transform.position, range);

        target = null;
        float minDistance = float.MaxValue;

        foreach (Collider2D collider in inRange)
        {
            if (collider.CompareTag("Enemy"))
            {
                float squaredDistance = Vector2.SqrMagnitude(collider.transform.position - transform.position);
                if (squaredDistance < minDistance)
                {
                    minDistance = squaredDistance;
                    target = collider.transform;
                }
            }
        }
    }

    private void StartInteract(InputAction.CallbackContext ctx)
    {
        Vector2 mousePos = inputActions.Player.MousePos.ReadValue<Vector2>();
        
        wasClicked = col.OverlapPoint(Camera.main.ScreenToWorldPoint(mousePos));
    }
    
    private void Interact(InputAction.CallbackContext ctx)
    {
        if (isPlaced)
        {
            Vector2 mousePos = inputActions.Player.MousePos.ReadValue<Vector2>();
            bool hit = col.OverlapPoint(Camera.main.ScreenToWorldPoint(mousePos));

            if (hit && wasClicked) isSelected = !isSelected;
            else isSelected = false;
            rangeIndicator.gameObject.SetActive(isSelected);
        }
        else
        {
            if (canBePlaced)
            {
                sprite.color = defaultColor;
                rangeIndicator.gameObject.SetActive(false);
                gameObject.layer = TOWER_LAYER_NUM;
                isPlaced = true;

                GameManager.Instance.money -= data.price;
            }
            else
            {
                Destroy(gameObject);
            }

        }
    }
}
