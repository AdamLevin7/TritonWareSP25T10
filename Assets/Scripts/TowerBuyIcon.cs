using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class TowerBuyIcon : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    public GameObject towerToSpawn;
    public InputActionReference dragAndDropReference;

    private InputSystem_Actions inputActions;

    private bool mouseHeld;
    private bool mouseOver;
    private GameObject currentDraggableTower;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inputActions = new();
        mouseOver = false;
    }

    private RectTransform rectTransform;
    private void Awake(){
        rectTransform = GetComponent<RectTransform>();
    }
    public void OnBeginDrag(PointerEventData eventData){
        SpawnTower();
    }
    public void OnDrag(PointerEventData eventData){
    }
    public void OnEndDrag(PointerEventData eventData){
    }
    public void OnPointerDown(PointerEventData eventData){
    }
    public void OnPointerClick(PointerEventData eventData){
        SpawnTower();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnTower()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        GameObject newTower = Instantiate(towerToSpawn);
        newTower.transform.position = mousePos;
    }

    void OnMouseEnter()
    {
        mouseOver = true;        
    }

    void OnMouseExit()
    {
        mouseOver = false;
    }
}
