using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

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

    [Header("Loss UI")]
    public GameObject roundLossComponent;
    [SerializeField] private GameObject restartButton;

    [Header("Tower Selection UI")]
    [SerializeField] private GameObject towerSelectionUI;
    [SerializeField] private Text selectedTowerName;
    [SerializeField] private Text selectedTowerDamage;
    [SerializeField] private Text selectedTowerValue;
    private GameObject currentSelectedTower;
    private Tower currentSelectedTowerClass;

    private TowerData currentSelectedTowerData;

    private Text livesTextComponent;
    private Text moneyTextComponent;
    private Text roundWaveTextComponent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;

        livesTextComponent = livesTextObject.GetComponent<Text>();
        moneyTextComponent = moneyTextObject.GetComponent<Text>();
        roundWaveTextComponent = roundWaveTextObject.GetComponent<Text>();

    }

    // Update is called once per frame
    void Update()
    {
        if (!gameObject.activeSelf) return;

        livesTextComponent.text = "Lives: " + GameManager.Instance.lives;
        moneyTextComponent.text = "Money: " + GameManager.Instance.money;
        roundWaveTextComponent.text = "Wave: " + GameManager.Instance.currentRound + "/" + GameManager.Instance.maxRounds;

    }

    // update tower selected ui information by reference
    public void UpdateTowerSelectedInformation(GameObject tower)
    {
        currentSelectedTower = tower;
        currentSelectedTowerClass = tower.GetComponent<Tower>();
        currentSelectedTowerData = currentSelectedTowerClass.data;

        selectedTowerName.text = currentSelectedTowerData.towerName;
        selectedTowerDamage.text = "Damage: " + ((int)currentSelectedTowerClass.totalDamageDealt).ToString();
        selectedTowerValue.text = "Value: " + currentSelectedTowerClass.sellValue;
    }

    // sell currently selected tower as shown in ui
    public void SellCurrentlySelectedTower()
    {
        // maybe have a towermanager do this in the future, but works for now
        GameManager.Instance.money += currentSelectedTowerClass.sellValue;
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

}
