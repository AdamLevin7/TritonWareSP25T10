using UnityEngine;
using UnityEngine.UI;

public class UIHandlerScript : Singleton
{
    [SerializeField] private GameObject canvas;
    [SerializeField] private GameObject livesText;
    [SerializeField] private GameObject moneyText;
    [SerializeField] private GameObject roundWaveText;

<<<<<<< Updated upstream
    [SerializeField] private GameObject gameManager;
=======
    [SerializeField] private Sprite waveStartSprite;
    [SerializeField] private Sprite twoXSpeedSprite;
    [SerializeField] private Sprite oneXSpeedSprite;

    [Header("Loss UI")]
    public GameObject roundLossUI;
    [SerializeField] private GameObject loseRestartBtn;
    [SerializeField] private GameObject loseMenuBtn;

    [Header("Win UI")]
    public GameObject roundWinUI;
    [SerializeField] private GameObject winRestartBtn;
    [SerializeField] private GameObject winMenuBtn;

    [Header("Tower Selection UI")]
    [SerializeField] private GameObject towerSelectionUI;
    [SerializeField] private Text selectedTowerName;
    [SerializeField] private Text selectedTowerDamage;
    [SerializeField] private Text selectedTowerValue;
    [SerializeField] private Image selectedTowerIcon;
    [SerializeField] private GameObject synergyManager;
    [SerializeField] private GameObject towerTypes;
    private GameObject currentSelectedTower;
    private Tower currentSelectedTowerClass;

    private TowerData currentSelectedTowerData;
>>>>>>> Stashed changes

    private Text livesTextComponent;
    private Text moneyTextComponent;
    private Text roundWaveTextComponent;
    private GameHandlerScript gameManagerClass;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        livesTextComponent = livesText.GetComponent<Text>();
        moneyTextComponent = moneyText.GetComponent<Text>();
        roundWaveTextComponent = roundWaveText.GetComponent<Text>();
        gameManagerClass = gameManager.GetComponent<GameHandlerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        livesTextComponent.text = "Lives: " + gameManagerClass.lives;
        moneyTextComponent.text = "Money: " + gameManagerClass.money;
        roundWaveTextComponent.text = "Round: " + gameManagerClass.currentRound + "/" + gameManagerClass.maxRounds;

    }
    public void UpdateTowerNumbers(){
        towerTypes.GetComponent<TMPro.TextMeshProUGUI>().text = "<color=\"red\">Red Towers: " + synergyManager.GetComponent<Synergy>().redTowers + 
                                                              "\n<color=\"green\">Green Towers: " + synergyManager.GetComponent<Synergy>().greenTowers + 
                                                              "\n<color=#03a1fc>Blue Towers: " + synergyManager.GetComponent<Synergy>().blueTowers;
    }
}