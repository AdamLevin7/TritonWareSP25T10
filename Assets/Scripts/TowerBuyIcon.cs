using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using Unity.VisualScripting;
using TMPro;
using UnityEngine.UI;

public class TowerBuyIcon : MonoBehaviour, IBeginDragHandler, IDragHandler, IPointerClickHandler
{
    [SerializeField] private GameObject towerToSpawn;
    [SerializeField] private TowerData towerData;

    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private Image background;
    [SerializeField] private Image icon;

    [SerializeField] private Color canAffordPriceColor;
    [SerializeField] private Color cannotAffordPriceColor;
    [SerializeField] private Color canAffordBgColor;
    [SerializeField] private Color cannotAffordBgColor;

    private void Start()
    {
        UpdateUI();
    }

    private void Update()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        priceText.text = "$" + towerData.price.ToString();
        icon.sprite = towerData.icon;
        if (GameManager.Instance.money >= towerData.price)
        {
            icon.color = canAffordBgColor;
            background.color = canAffordBgColor;
            priceText.color = canAffordPriceColor;
        }
        else
        {
            icon.color = cannotAffordBgColor;
            background.color = cannotAffordBgColor;
            priceText.color = cannotAffordPriceColor;
        }
    }

    public void OnBeginDrag(PointerEventData eventData){
        if (GameManager.Instance.money < towerData.price) return;
        SpawnTower();
    }

    // IDragHandler is necessary for OnBeginDrag, so this is a dummy function
    public void OnDrag(PointerEventData eventData)
    {
        return;
    }

    public void OnPointerClick(PointerEventData eventData){
        if (GameManager.Instance.money < towerData.price) return;
        SpawnTower();
    }

    void SpawnTower()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        GameObject newTower = Instantiate(towerToSpawn);
        newTower.transform.position = mousePos;
        Tower newTowerScript = newTower.GetComponent<Tower>();
        newTowerScript.data = towerData;
    }
}
