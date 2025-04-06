using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private Canvas canvas;
    private RectTransform rectTransform;
    private GameObject tower;
    private void Awake(){
        rectTransform = GetComponent<RectTransform>();
    }
    public void OnBeginDrag(PointerEventData eventData){
        Debug.Log("OnBeginDrag");
    }
    public void OnDrag(PointerEventData eventData){
        Debug.Log("OnDrag");
        rectTransform.anchoredPosition += eventData.delta  / canvas.scaleFactor;
    }
    public void OnEndDrag(PointerEventData eventData){
        Debug.Log("OnEndDrag");
        Tower towerToBuild = BuildManager.main.GetSelectedTower();
        tower = Instantiate(towerToBuild.prefab, transform.position, Quaternion.identity);
    }
    public void OnPointerDown(PointerEventData eventData){
        Debug.Log("OnPointerDown");
    }
}
