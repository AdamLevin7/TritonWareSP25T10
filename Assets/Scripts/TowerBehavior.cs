using UnityEngine;

public abstract class TowerBehavior : MonoBehaviour
{
    [SerializeField] public Tower tower;

    public void Start()
    {
        tower.behavior = this;
    }

    protected bool upgrade1Unlocked = false;
    protected bool upgrade2Unlocked = false;
    protected bool upgrade3Unlocked = false;

    public abstract void Fire();

    public void UpgradeTower()
    {
        tower.currentUpgradeTier++;
        GameManager.Instance.money -= tower.upgrades[tower.currentUpgradeTier - 1].price;
        tower.sellValue += (int)((float)tower.upgrades[tower.currentUpgradeTier - 1].price * 0.75f);
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

    public abstract void OnTier1Upgrade();
    public abstract void OnTier2Upgrade();
    public abstract void OnTier3Upgrade();
}
