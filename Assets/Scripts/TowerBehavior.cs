using UnityEngine;

public abstract class TowerBehavior : MonoBehaviour
{
    [SerializeField] protected Tower tower;

    public void Start()
    {
        tower.behavior = this;
    }

    public abstract void Fire();
}
