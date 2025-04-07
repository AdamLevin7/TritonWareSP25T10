using UnityEngine;

[CreateAssetMenu(fileName = "Tower", menuName = "ScriptableObjects/Tower")]
public class TowerData : ScriptableObject
{
    public string towerName;
    public int price;
    public Sprite icon;
}
