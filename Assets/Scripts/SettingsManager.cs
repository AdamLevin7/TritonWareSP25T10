using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    // public static SettingsManager Instance { get; private set; }
    public static SettingsManager Instance { get; private set; } 

    public GameObject settingsUI;

    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;


    [SerializeField] private Button resumeBtn;
    [SerializeField] private Button restartBtn;
    [SerializeField] private Button menuBtn;

    private InputSystem_Actions inputActions;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inputActions = new();

        masterVolumeSlider.value = AudioManager.Instance.masterVolume;
        sfxVolumeSlider.value = AudioManager.Instance.sfxVolume;
        musicVolumeSlider.value = AudioManager.Instance.musicVolume;
    }

    private void OnEnable()
    {
        inputActions.Enable();
        inputActions.Player.Cancel.performed += Cancel;
    }

    private void OnDisable()
    {
        inputActions.Player.Cancel.performed -= Cancel;
        inputActions.Disable();

        UpdateVolumes();
    }

    public void ResumeGame()
    {
        settingsUI.SetActive(false);
        Time.timeScale = (GameManager.Instance.isSpedUp) ? GameManager.Instance.waveSpeedUpFactor : 1.00f;
    }

    public void PauseGame()
    {
        Time.timeScale = 0.0f;
        settingsUI.SetActive(true);
    }

    // try to pause game
    public void Cancel(InputAction.CallbackContext ctx)
    {
        if (GameManager.Instance.placingTower) return;

        if (settingsUI.activeSelf)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    public void UpdateVolumes()
    {
        AudioManager.Instance.masterVolume = masterVolumeSlider.value;
        AudioManager.Instance.sfxVolume = sfxVolumeSlider.value;
        AudioManager.Instance.musicVolume = musicVolumeSlider.value;
    }

}
