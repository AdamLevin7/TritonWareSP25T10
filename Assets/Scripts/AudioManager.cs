using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    private EventInstance musicEventInstance;

    [Header("Music")]

    public EventReference titleScreenMusic;
    public EventReference gameplayMusic;

    [Header("SFX")]

    public EventReference towerPlacementSound;
    public EventReference enemyHitSound;
    public EventReference enemyDeathSound;
    public EventReference beamFireSound;
    public EventReference explosionSound;
    public EventReference uiClickSound;
    public EventReference towerSelectSound;
    public EventReference whooshSound;

    public EventReference loseSound;
    public EventReference winSound;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;

        InitializeMusic(gameplayMusic);
    }

    public void PlayOneShot(EventReference sound)
    {
        RuntimeManager.PlayOneShot(sound);
    }

    public void InitializeMusic(EventReference music)
    {
        musicEventInstance = RuntimeManager.CreateInstance(music);
        musicEventInstance.start();
    }
}
