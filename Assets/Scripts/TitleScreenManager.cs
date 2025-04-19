using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TitleScreenManager : MonoBehaviour
{
    [Header("Start Menu")]
    [SerializeField] private Button startBtn;
    [SerializeField] private Button creditsBtn;
    [SerializeField] private Button quitBtn;
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
        inputActions = new();

        // gameManager should be present and disabled in here (to sync settings)
        // DontDestroyOnLoad(GameManager.Instance);

        UpdateVolumes();

        AudioManager.Instance.InitializeMusic(AudioManager.Instance.titleScreenMusic);
    }

    private void OnEnable()
    {
        inputActions.Enable();
        inputActions.Player.Interact.started += StartInteract;
        inputActions.Player.Cancel.performed += Cancel;
    }

    private void OnDisable()
    {
        inputActions.Player.Interact.started -= StartInteract;
        inputActions.Player.Cancel.performed -= Cancel;
        inputActions.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        return;
    }

    public void StartGame()
    {
        // should load game scene, but not linked yet
        return;
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

    private void UpdateVolumes()
    {
        AudioManager.Instance.masterVolume = masterVolumeSlider.value;
        AudioManager.Instance.sfxVolume = sfxVolumeSlider.value;
        AudioManager.Instance.musicVolume = musicVolumeSlider.value;
    }
}
