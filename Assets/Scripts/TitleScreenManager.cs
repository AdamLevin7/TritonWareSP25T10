using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleScreenManager : MonoBehaviour
{

    [Header("Start Menu")]
    [SerializeField] private Button startBtn;
    [SerializeField] private Button creditsBtn;
    [SerializeField] private Button quitBtn;
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private GameObject credits;

    private InputSystem_Actions inputActions;

    [Header("Settings Menu")]
    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;
    private float masterVolumeSliderValue;
    private float sfxVolumeSliderValue;
    private float musicVolumeSliderValue;
    
    public float masterVolume;
    public float sfxVolume;
    public float musicVolume;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inputActions = new();
        inputActions.Enable();

        inputActions.Player.Interact.started += StartInteract;
        inputActions.Player.Cancel.performed += Cancel;

        // gameManager should be present and disabled in here (to sync settings)
        // DontDestroyOnLoad(GameManager.Instance);

        UpdateMasterVolume();
        UpdateSFXVolume();
        UpdateMusicVolume();
    }

    // Update is called once per frame
    void Update()
    {
        return;
    }

    public void StartGame()
    {
        AudioManager.Instance.PlayOneShot(AudioManager.Instance.uiClickSound);
        SceneManager.LoadScene(1);
    }

    public void OpenSettings()
    {
        settingsMenu.SetActive(true);
    }

    public void CloseSttings()
    {
        settingsMenu.SetActive(false);
    }

    public void OpenCredits()
    {
        credits.SetActive(true);
    }

    public void CloseCredits()
    {
        credits.SetActive(false);
    }

    public void Quit()
    {
        Application.Quit();
    }

    private void StartInteract(InputAction.CallbackContext ctx)
    {
        if (credits.activeSelf)
        {
            CloseCredits();
        }
    }

    private void Cancel(InputAction.CallbackContext ctx)
    {
        if (settingsMenu.activeSelf)
        {
            CloseSttings();
        }

        if (credits.activeSelf)
        {
            CloseCredits();
        }
    }

    /// <summary>
    /// For the settings to sync, it would be easier for GameManager to already exist so that audio settings can be synced directly. 
    /// In that case, GameManager should have DontDestroyOnLoad()
    /// - Aiden
    /// </summary>
    public void UpdateMasterVolume()
    {
        masterVolumeSliderValue = masterVolumeSlider.value;
        UpdateVolumes();
        AudioListener.volume = masterVolume;
    }

    public void UpdateSFXVolume()
    {
        sfxVolumeSliderValue = sfxVolumeSlider.value;
        UpdateVolumes();
    }

    public void UpdateMusicVolume()
    {
        musicVolumeSliderValue = musicVolumeSlider.value;
        UpdateVolumes();
    }

    private void UpdateVolumes()
    {
        masterVolume = masterVolumeSliderValue;
        sfxVolume = masterVolume * sfxVolumeSliderValue;
        musicVolume = masterVolume * musicVolumeSliderValue;
    }
}
