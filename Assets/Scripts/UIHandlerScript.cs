using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIHandlerScript : Singleton
{
    [SerializeField] private GameObject canvas;
    public GameObject roundActiveComponent;
    [SerializeField] private GameObject livesTextObject;
    [SerializeField] private GameObject moneyTextObject;
    [SerializeField] private GameObject roundWaveTextObject;
    public GameObject roundLossComponent;
    [SerializeField] private GameObject restartButton;

    [SerializeField] private GameObject gameManager;

    private Text livesTextComponent;
    private Text moneyTextComponent;
    private Text roundWaveTextComponent;
    private GameHandlerScript gameManagerClass; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        livesTextComponent = livesTextObject.GetComponent<Text>();
        moneyTextComponent = moneyTextObject.GetComponent<Text>();
        roundWaveTextComponent = roundWaveTextObject.GetComponent<Text>();
        gameManagerClass = gameManager.GetComponent<GameHandlerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameObject.activeSelf) return;

        livesTextComponent.text = "Lives: " + gameManagerClass.lives;
        moneyTextComponent.text = "Money: " + gameManagerClass.money;
        roundWaveTextComponent.text = "Wave: " + gameManagerClass.currentRound + "/" + gameManagerClass.maxRounds;

    }
}
