using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    public EventInstance musicEventInstance;

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
    public EventReference uiHoverSound;
    public EventReference towerSelectSound;
    public EventReference whooshSound;
    public EventReference hummingSound;

    public EventReference loseSound;
    public EventReference winSound;

    [Header("Volume")]
    [Range(0, 1)] public float masterVolume;
    [Range(0, 1)] public float sfxVolume;
    [Range(0, 1)] public float musicVolume;

    private Bus masterBus;
    private Bus musicBus;
    private Bus sfxBus;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;

        GameObject[] objs = GameObject.FindGameObjectsWithTag("AudioManager");

        if (objs.Length > 1) Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        masterBus = RuntimeManager.GetBus("bus:/");
        sfxBus = RuntimeManager.GetBus("bus:/SFX");
        musicBus = RuntimeManager.GetBus("bus:/Music");
    }

    private void Update()
    {
        masterBus.setVolume(masterVolume);
        sfxBus.setVolume(sfxVolume);
        musicBus.setVolume(musicVolume);
    }

    public void PlayOneShot(EventReference sound)
    {
        RuntimeManager.PlayOneShot(sound);
    }

    public EventInstance CreateEventInstance(EventReference eventReference)
    {
        EventInstance newInstance = RuntimeManager.CreateInstance(eventReference);
        return newInstance;
    }

    public void InitializeMusic(EventReference music)
    {
        musicEventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        musicEventInstance = CreateEventInstance(music);
        musicEventInstance.start();
    }
}
