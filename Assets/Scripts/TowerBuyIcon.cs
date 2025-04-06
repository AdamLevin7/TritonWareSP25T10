using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class TowerBuyIcon : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public GameObject towerToSpawn;
    public InputActionReference dragAndDropReference;

    private bool mouseHeld;
    private bool mouseOver;
    private GameObject currentDraggableTower;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mouseOver = false;
    }

    private RectTransform rectTransform;
    private void Awake(){
        rectTransform = GetComponent<RectTransform>();
    }
    public void OnBeginDrag(PointerEventData eventData){
        Debug.Log("OnBeginDrag");
    }
    public void OnDrag(PointerEventData eventData){
        Debug.Log("OnDrag");
        rectTransform.anchoredPosition += eventData.delta;
    }
    public void OnEndDrag(PointerEventData eventData){
        Debug.Log("OnEndDrag");
    }
    public void OnPointerDown(PointerEventData eventData){
        Debug.Log("OnPointerDown");
    }

    // Update is called once per frame
    void Update()
    {
        
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
