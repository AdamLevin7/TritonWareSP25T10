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

    // private void Awake()
    // {
    //     if (Instance != null && Instance != this) Destroy(this);
    //     else Instance = this;
    // }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inputActions = new();
        inputActions.Enable();
        inputActions.Player.Cancel.performed += Cancel;

        masterVolumeSliderValue = masterVolumeSlider.value;
        sfxVolumeSliderValue = sfxVolumeSlider.value;
        musicVolumeSliderValue = musicVolumeSlider.value;
        UpdateVolumes();
    }

    // private void OnDisable()
    // {
    //     inputActions.Disable();
    //     inputActions.Player.Cancel.performed -= Cancel;
    // }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateMasterVolume()
    {
        masterVolumeSliderValue = masterVolumeSlider.value;
        UpdateVolumes();
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
        AudioManager.Instance.PlayOneShot(AudioManager.Instance.uiClickSound);
        settingsUI.SetActive(false);
        Time.timeScale = (GameManager.Instance.isSpedUp) ? GameManager.Instance.waveSpeedUpFactor : 1.00f;
    }

    public void PauseGame()
    {
        AudioManager.Instance.PlayOneShot(AudioManager.Instance.uiClickSound);
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

    private void UpdateVolumes()
    {
        AudioManager.Instance.masterVolume = masterVolumeSliderValue;
        AudioManager.Instance.sfxVolume = sfxVolumeSliderValue;
        AudioManager.Instance.musicVolume = musicVolumeSliderValue;
    }

}
