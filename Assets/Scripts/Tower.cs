using System.Data.SqlTypes;
using NUnit.Framework.Constraints;
using Unity.VisualScripting;
using UnityEditor;
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
    [SerializeField] private GameObject synergyManager;
    [Header("Colors")]
    [SerializeField] private Color rangeIndicatorValidColor;
    [SerializeField] private Color rangeIndicatorInvalidColor;
    private InputSystem_Actions inputActions;

    public TowerBehavior behavior;
    public float shootCooldown;
    public float range;
    private float timeSinceLastShoot = 0f;
    private float rgResonanceBuff = 0.25f;
    private float rbResonanceBuff = 0.25f;
    private float bgResonanceBuff = 0.1f;
    [Header("Upgrades")]
    public int currentUpgradeTier;
    public int maxUpgradeTiers;

    private bool isPlaced = false;
    private bool canBePlaced = false;
    private bool isSelected = false;
    private bool wasClicked = false;
    public Transform target;

    private const int UI_LAYER_NUM = 5;
    private const int TOWER_LAYER_NUM = 6;

    public TowerData data;
    public float totalDamageDealt;
    public int sellValue;
    public int damageScaleFactor = 1;
    public int firerateScaleFactor = 1;
    public int rangeScaleFactor = 1;

    // For storing collision for detecting enemies in range
    private readonly Collider[] inRange = new Collider[32];

    private void Awake()
    {
        inputActions = new();
        rangeIndicator.gameObject.SetActive(true);
        rangeIndicator.transform.localScale = new(range * 2, range * 2, range * 2);
        gameObject.layer = UI_LAYER_NUM;
        //Debug.Log("Hello there");
        synergyManager = GameObject.FindWithTag("synergy");
        GameManager.Instance.placingTower = true;
        // uiHandlerClass = UIHandler.GetComponent<UIHandlerScript>();
        effectiveRange = baseRange * rangeScaleFactor;
    }

    private void OnEnable()
    {
        inputActions.Enable();
        inputActions.Player.Interact.started += StartInteract;
        inputActions.Player.Interact.canceled += Interact;
        inputActions.Player.Cancel.performed += Cancel;
    }

    private void OnDisable()
    {
        inputActions.Player.Interact.started -= StartInteract;
        inputActions.Player.Interact.canceled -= Interact;
        inputActions.Player.Cancel.performed -= Cancel;
        inputActions.Disable();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(rangeIndicator.transform.position, baseRange);
    }

    private void Update()
    {
        float dt = Time.deltaTime;

        if (isPlaced)
        {
            timeSinceLastShoot -= dt;

            SetTarget();

            if(synergyManager.GetComponent<Synergy>().bgSynergy && (synergyType.ToString() == "Blue" || synergyType.ToString() == "Green")){
                range = buffedRange;
            }
            else{
                range = unbuffedRange;
            }

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

    /// <summary>
    /// Sets this tower's target to the nearest enemy in range;
    /// </summary>
    public void SetTarget()
    {
        int resultCount = Physics.OverlapSphereNonAlloc(rangeIndicator.transform.position, baseRange, inRange);

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

            if (hit && wasClicked) 
            {
                isSelected = !isSelected;

                // open tower selection ui
                UIHandlerScript.Instance.UpdateTowerSelectedInformation(this.gameObject);
                UIHandlerScript.Instance.SetTowerSelectedUIState(isSelected);
            }
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
                GameManager.Instance.placingTower = false;
                synergyManager.GetComponent<Synergy>().UpdateTowerSynergy(synergyType.ToString(), 1);

                GameManager.Instance.money -= data.price;
                sellValue = (int)((float)data.price * 0.75f);
            }
            else
            {
                Destroy(gameObject);
            }

        }
    }

    private void Cancel(InputAction.CallbackContext ctx)
    {
        if (isPlaced)
        {
            isSelected = false;
            rangeIndicator.gameObject.SetActive(false);

            // close tower selection ui
            // UIHandlerScript.Instance.SetTowerSelectedUIState(false);
        }
        else
        {
            Destroy(gameObject);
        }
        GameManager.Instance.placingTower = false;

    }

    /// <summary>
    /// Gets the proper target position of a <paramref name="target"/> Transform. 
    /// This is so the z-component is always half of the <paramref name="target"/> collider's height.
    /// </summary>
    /// <param name="target">The target's transform</param>
    /// <returns>The proper target position</returns>
    public static Vector3 Get3DTargetPos(Transform target)
    {
        Vector2 initialTargetPos = target.position;
        Vector3 targetHalfHeight = EnemyManager.Instance.TryGetEnemy(target.gameObject).halfHeight * Vector3.back;

        return (Vector3)initialTargetPos + targetHalfHeight;
    }
}
