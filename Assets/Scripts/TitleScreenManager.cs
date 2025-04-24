using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TitleScreenManager : MonoBehaviour
{
    [Header("Start Menu")]
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private GameObject credits;
    [SerializeField] private StartGame gameStarter;

    private InputSystem_Actions inputActions;

    [Header("Settings Menu")]
    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        Time.timeScale = 1.0f;
        inputActions = new();

        // gameManager should be present and disabled in here (to sync settings)
        // DontDestroyOnLoad(GameManager.Instance);

        StartCoroutine(StartMusic());

        masterVolumeSlider.value = AudioManager.Instance.masterVolume;
        sfxVolumeSlider.value = AudioManager.Instance.sfxVolume;
        musicVolumeSlider.value = AudioManager.Instance.musicVolume;
    }

    private IEnumerator StartMusic()
    {
        if (!FMODUnity.RuntimeManager.HaveAllBanksLoaded) yield return null;

        AudioManager.Instance.InitializeMusic(AudioManager.Instance.titleScreenMusic);
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

    public void UpdateVolumes()
    {
        AudioManager.Instance.masterVolume = masterVolumeSlider.value;
        AudioManager.Instance.sfxVolume = sfxVolumeSlider.value;
        AudioManager.Instance.musicVolume = musicVolumeSlider.value;
    }
}
