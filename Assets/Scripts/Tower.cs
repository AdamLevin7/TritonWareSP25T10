using System.Data.SqlTypes;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Tower : MonoBehaviour
{
    [SerializeField] private CapsuleCollider col;
    [SerializeField] private SpriteRenderer rangeIndicator;
    [SerializeField] private LayerMask towerLayer;
    [SerializeField] private LayerMask placementObstructionLayer;
    public enum SynergyType{
        Red,
        Blue,
        Yellow,
        Green
    }

    public SynergyType synergyType;
    public GameObject synergyManager;
    [Header("Colors")]
    [SerializeField] private Color rangeIndicatorValidColor;
    [SerializeField] private Color rangeIndicatorInvalidColor;
    private InputSystem_Actions inputActions;

    public TowerBehavior behavior;
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

    // For storing collision for detecting enemies in range
    private readonly Collider[] inRange = new Collider[32];

    private void Awake()
    {
        inputActions = new();
        rangeIndicator.gameObject.SetActive(true);
        rangeIndicator.transform.localScale = new(range * 2, range * 2, range * 2);
        gameObject.layer = UI_LAYER_NUM;
        synergyManager.GetComponent<Synergy>().UpdateTowerSynergy(synergyType.ToString());
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
        Gizmos.DrawWireSphere(rangeIndicator.transform.position, range);
    }

    private void Update()
    {
        float dt = Time.deltaTime;

        if (isPlaced)
        {
            timeSinceLastShoot -= dt;

            GetTarget();

            if (timeSinceLastShoot < 0f && target != null)
            {
                behavior.Fire();

                timeSinceLastShoot = shootCooldown;
            }
        }
        else
        {
            Vector2 mousePos = inputActions.Player.MousePos.ReadValue<Vector2>();
            Ray ray = Camera.main.ScreenPointToRay(mousePos);
            GameManager.Instance.groundCollider.Raycast(ray, out RaycastHit hit, Mathf.Infinity);
            transform.position = hit.point + col.height / 2 * Vector3.back;

            Vector3 direction = new() { [col.direction] = 1 };
            float offset = col.height / 2 - col.radius;
            Vector3 point0 = transform.TransformPoint(col.center - direction * offset);
            Vector3 point1 = transform.TransformPoint(col.center + direction * offset);
            canBePlaced = Physics.OverlapCapsuleNonAlloc(point0, point1, col.radius, inRange, placementObstructionLayer) == 0;

            rangeIndicator.color = canBePlaced ? rangeIndicatorValidColor : rangeIndicatorInvalidColor;
        }
    }

    public void GetTarget()
    {
        int resultCount = Physics.OverlapSphereNonAlloc(rangeIndicator.transform.position, range, inRange);

        target = null;
        float minDistance = float.MaxValue;

        for (int i = 0; i < resultCount; ++i)
        {
            Collider collider = inRange[i];
            if (collider.CompareTag("Enemy"))
            {
                float squaredDistance = Vector3.SqrMagnitude(collider.transform.position - transform.position);
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
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        wasClicked = col.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity);
    }
    
    private void Interact(InputAction.CallbackContext ctx)
    {
        if (isPlaced)
        {
            Vector2 mousePos = inputActions.Player.MousePos.ReadValue<Vector2>();
            Ray ray = Camera.main.ScreenPointToRay(mousePos);
            bool hit = col.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity);

            if (hit && wasClicked) isSelected = !isSelected;
            else isSelected = false;
            rangeIndicator.gameObject.SetActive(isSelected);
        }
        else
        {
            if (canBePlaced)
            {
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
