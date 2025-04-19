using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using JetBrains.Annotations;

public class UIHandlerScript : MonoBehaviour
{

    public static UIHandlerScript Instance { get; private set; }

    [Header("Game UI")]
    [SerializeField] private GameObject canvas;
    public GameObject roundActiveComponent;
    [SerializeField] private GameObject livesTextObject;
    [SerializeField] private GameObject moneyTextObject;
    [SerializeField] private GameObject roundWaveTextObject;
    [SerializeField] private GameObject availableTowersUI;
    [SerializeField] private GameObject synergiesUI;
    [SerializeField] private GameObject nextWaveOrSpeedUpBtn;
    [SerializeField] private Image nextWaveOrSpeedUpImage;
    [SerializeField] private GameObject towerSelectUIBtn;

    [SerializeField] private Sprite waveStartSprite;
    [SerializeField] private Sprite twoXSpeedSprite;
    [SerializeField] private Sprite oneXSpeedSprite;

    [SerializeField] private Text upgradeNameText;
    [SerializeField] private Text upgradeDescriptionText;
    [SerializeField] private Text upgradePriceText;
    [SerializeField] private Text upgradeTierText;

    [Header("Loss UI")]
    public GameObject roundLossUI;
    [SerializeField] private GameObject loseRestartBtn;
    [SerializeField] private GameObject loseMenuBtn;

    [Header("Win UI")]
    public GameObject roundWinUI;
    [SerializeField] private GameObject winRestartBtn;
    [SerializeField] private GameObject winMenuBtn;

    [Header("Tower Selection UI")]
    public GameObject towerSelectionUI;
    [SerializeField] private Text selectedTowerName;
    [SerializeField] private Text selectedTowerDamage;
    [SerializeField] private Text selectedTowerValue;
    [SerializeField] private Image selectedTowerIcon;
    [SerializeField] private GameObject synergyManager;
    [SerializeField] private GameObject currentSelectedTower;
    [SerializeField] private TowerBehavior currentSelectedTowerClass;

    [SerializeField] private TowerData currentSelectedTowerData;

    private Text livesTextComponent;
    private Text moneyTextComponent;
    private Text roundWaveTextComponent;
    private UpgradeData nextUpgrade;

    private RectTransform towerSelectUIBtnRT;
    private bool hiddenTowerSelection = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;

        livesTextComponent = livesTextObject.GetComponent<Text>();
        moneyTextComponent = moneyTextObject.GetComponent<Text>();
        roundWaveTextComponent = roundWaveTextObject.GetComponent<Text>();
        synergyManager = GameObject.FindWithTag("synergy");
        // isWaveActive = false;

        towerSelectUIBtnRT = towerSelectUIBtn.GetComponent<RectTransform>();
    }

    void Start()
    {
        roundWaveTextComponent.text = "Wave: " + GameManager.Instance.currentWave + "/" + GameManager.Instance.maxWaves;   

        currentSelectedTower = new();
        // currentSelectedTowerClass = new();
        currentSelectedTowerData = new();
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameObject.activeSelf) return;

        livesTextComponent.text = GameManager.Instance.lives.ToString();
        moneyTextComponent.text = GameManager.Instance.money.ToString();
        // roundWaveTextComponent.text = "Wave: " + GameManager.Instance.currentWave + "/" + GameManager.Instance.maxWaves;

    }

    // update tower selected ui information by reference
    public void UpdateTowerSelectedInformation(GameObject tower)
    {
        currentSelectedTower = tower;
        currentSelectedTowerClass = tower.GetComponent<TowerBehavior>();
        currentSelectedTowerData = currentSelectedTowerClass.tower.data;
        
        selectedTowerIcon.sprite = currentSelectedTowerData.icon;
        selectedTowerName.text = currentSelectedTowerData.towerName;
        selectedTowerDamage.text = "Damage: " + ((int)currentSelectedTowerClass.tower.totalDamageDealt).ToString();
        selectedTowerValue.text = "Value: " + currentSelectedTowerClass.tower.sellValue;

        UpdateUpgradeUI();
    }

    // sell currently selected tower as shown in ui
    public void SellCurrentlySelectedTower()
    {
        // maybe have a towermanager do this in the future, but works for now
        GameManager.Instance.money += currentSelectedTowerClass.tower.sellValue;
        synergyManager.GetComponent<Synergy>().UpdateTowerSynergy(currentSelectedTowerClass.tower.synergyType.ToString(), -1);
        Destroy(currentSelectedTower);
        SetTowerSelectedUIState(false);
    }

    // set state of tower selected ui
    public void SetTowerSelectedUIState(bool state)
    {
        towerSelectionUI.SetActive(state);
        availableTowersUI.SetActive(!state);
        synergiesUI.SetActive(!state);
    }

    public void SwitchWaveButton(bool waveActive, float timeScale)
    {
        if (waveActive)
        {
            nextWaveOrSpeedUpImage.sprite = (timeScale == 1.0f) ? oneXSpeedSprite : twoXSpeedSprite;
        }
        else
        {
            nextWaveOrSpeedUpImage.sprite = waveStartSprite;
        }
    }

    public void UpdateWaveNumber(int waveNumber, int lastPredeterminedWave)
    {
        string waveText = "Wave: " + waveNumber;
        if (waveNumber <= lastPredeterminedWave) waveText += " / " + lastPredeterminedWave;
        roundWaveTextComponent.text = waveText;
    }

    public void ControlTowerSelectUI()
    {
        hiddenTowerSelection = !hiddenTowerSelection;
        availableTowersUI.SetActive(!hiddenTowerSelection);

        float newX = (hiddenTowerSelection) ? -525 : 10;
        towerSelectUIBtnRT.anchoredPosition = new(newX, -225);
    }

    public void ContinueToFreeplay()
    {
        roundWinUI.SetActive(false);
        roundActiveComponent.SetActive(true);
    }

    public void GoToMenu()
    {
        AudioManager.Instance.PlayOneShot(AudioManager.Instance.uiClickSound);
        SceneManager.LoadScene(0);
    }

    public void UpdateUpgradeUI()
    {
        int upgradeTier = currentSelectedTowerClass.tower.currentUpgradeTier;

        upgradeTierText.text = upgradeTier + "/" + currentSelectedTowerClass.tower.maxUpgradeTiers;

        if (upgradeTier == currentSelectedTowerClass.tower.maxUpgradeTiers)
        {
            upgradeNameText.text = "Max Upgrades!";
            upgradeDescriptionText.text = "";
            upgradePriceText.text = "";
            return;
        }

        UpgradeData nextUpgrade = currentSelectedTowerClass.tower.upgrades[upgradeTier + 1];
        upgradeNameText.text = nextUpgrade.upgradeName;
        upgradeDescriptionText.text = nextUpgrade.upgradeDescription;
        upgradePriceText.text = "$" + nextUpgrade.price.ToString();
    }

    public void TryUpgradeTower()
    {
        Debug.Log(this.currentSelectedTower);
        if (this.currentSelectedTower == null) 
        {
            Debug.LogWarning("currentSelectedTower is not referenced");
            return;
        }
        currentSelectedTowerClass = this.currentSelectedTower.GetComponent<TowerBehavior>();
        int upgradeTier = currentSelectedTowerClass.tower.currentUpgradeTier;
        UpgradeData nextUpgrade = currentSelectedTowerClass.tower.upgrades[upgradeTier + 1];

        return;

        if (nextUpgrade.price > GameManager.Instance.money) return;
        if (currentSelectedTowerClass.tower.currentUpgradeTier == currentSelectedTowerClass.tower.maxUpgradeTiers) return;

        currentSelectedTowerClass.UpgradeTower();
        UpdateUpgradeUI();
    }
}
