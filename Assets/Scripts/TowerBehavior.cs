using UnityEngine;

public abstract class TowerBehavior : MonoBehaviour
{
    [SerializeField] public Tower tower;

    public void Start()
    {
        tower.behavior = this;
    }

    public abstract void Fire();

    public abstract void OnTier3Upgrade();
}
