using UnityEngine;

public abstract class TowerBehavior : MonoBehaviour
{
    [SerializeField] public Tower tower;

    public void Start()
    {
        tower.behavior = this;
    }

    public abstract void Fire();

    public void UpgradeTower()
    {
        tower.currentUpgradeTier++;
        GameManager.Instance.money -= tower.upgrades[tower.currentUpgradeTier].price;
        switch (tower.currentUpgradeTier)
        {
            case 0:
                OnTier1Upgrade();
                break;
            case 1:
                OnTier2Upgrade();
                break;
            case 2:
                OnTier3Upgrade();
                break;
        }
    }

    public abstract void OnTier1Upgrade();
    public abstract void OnTier2Upgrade();
    public abstract void OnTier3Upgrade();
}
