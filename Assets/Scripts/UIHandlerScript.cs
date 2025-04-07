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

    private Text livesTextComponent;
    private Text moneyTextComponent;
    private Text roundWaveTextComponent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
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
}
