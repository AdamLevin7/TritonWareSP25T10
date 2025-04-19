using UnityEngine;

public abstract class TowerBehavior : MonoBehaviour
{
    [SerializeField] public Tower tower;

    public void Start()
    {
        tower.behavior = this;
    }

    public void UpgradeTower()
    {
        tower.currentUpgradeTier++;
        switch (tower.currentUpgradeTier)
        {
            case 1:
                OnTier1Upgrade();
                break;
            case 2:
                OnTier2Upgrade();
                break;
            case 3:
                OnTier3Upgrade();
                break;
        }
    }

    public abstract void Fire();

    public abstract void OnTier1Upgrade();
    public abstract void OnTier2Upgrade();
    public abstract void OnTier3Upgrade();
}
