using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    // public static SettingsManager Instance { get; private set; }

    [SerializeField] private GameObject settingsUI;

    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;


    [SerializeField] private Button resumeBtn;
    [SerializeField] private Button restartBtn;
    [SerializeField] private Button menuBtn;

    private float masterVolumeSliderValue;
    private float sfxVolumeSliderValue;
    private float musicVolumeSliderValue;
    private InputSystem_Actions inputActions;
    
    public float masterVolume;
    public float sfxVolume;
    public float musicVolume;

    // private void Awake()
    // {
    //     if (Instance != null && Instance != this) Destroy(this);
    //     else Instance = this;
    // }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // inputActions = new();

        masterVolumeSliderValue = masterVolumeSlider.value;
        sfxVolumeSliderValue = sfxVolumeSlider.value;
        musicVolumeSliderValue = musicVolumeSlider.value; 
    }

    private void OnEnable()
    {
        inputActions.Enable();
        inputActions.Player.Cancel.performed += Cancel;
    }

    private void OnDisable()
    {
        inputActions.Disable();
        inputActions.Player.Cancel.performed -= Cancel;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateMasterVolume()
    {
        masterVolumeSliderValue = masterVolumeSlider.value;
        UpdateVolumes();
        AudioListener.volume = masterVolume;
    }

    public void UpdateSFXVolume()
    {
        sfxVolumeSliderValue = masterVolumeSlider.value;
        UpdateVolumes();
    }

    public void UpdateMusicVolume()
    {
        musicVolumeSliderValue = musicVolumeSlider.value;
        UpdateVolumes();
    }

    public void ResumeGame()
    {
        Time.timeScale = (GameManager.Instance.isSpedUp) ? GameManager.Instance.waveSpeedUpFactor : 1.00f;
    }

    public void PauseGame()
    {
        Time.timeScale = 0.0f;
    }

    // try to pause game
    public void Cancel(InputAction.CallbackContext ctx)
    {
        if (GameManager.Instance.placingTower) return;

        if (settingsUI.activeSelf)
        {
            settingsUI.SetActive(false);
            ResumeGame();
        }
        else
        {
            settingsUI.SetActive(true);
            PauseGame();
        }
    }

    private void UpdateVolumes()
    {
        masterVolume = masterVolumeSliderValue;
        sfxVolume = masterVolume * sfxVolumeSliderValue;
        musicVolume = masterVolume * musicVolumeSliderValue;
    }

}
