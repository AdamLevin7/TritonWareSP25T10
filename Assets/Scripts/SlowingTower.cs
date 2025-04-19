using UnityEngine;
using FMOD.Studio;

public class SlowingTower : TowerBehavior
{
    [SerializeField] private uint slowDuration;
    [SerializeField] private float slowFactor;
	[SerializeField] private int damage;

    private EventInstance humming;

    private void Awake()
    {
        humming = AudioManager.Instance.CreateEventInstance(AudioManager.Instance.hummingSound);
    }

    public override void Fire()
    {
        Collider[] overlapColliders = Physics.OverlapSphere(tower.transform.position, tower.effectiveRange);

        foreach (var collider in overlapColliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                EnemyManager.Instance.EnemySlowed(collider.gameObject, slowDuration, slowFactor);
				EnemyManager.Instance.EnemyTakeDamage(collider.gameObject, damage);
            }
        }
    }

    public override void OnTier1Upgrade()
    {
        upgrade1Unlocked = true;
        tower.baseRange *= 1.25f;
        return;
    }
    public override void OnTier2Upgrade()
    {
        upgrade2Unlocked = true;
        slowFactor = 2.0f;
        return;
    }
    public override void OnTier3Upgrade()
    {
        upgrade3Unlocked = true;
        slowDuration = 2000; // it's in frames???
        return;
    }

    private void Update()
    {
        UpdateSound();
    }

    private void UpdateSound()
    {
        if (tower.target != null)
        {
            humming.getPlaybackState(out PLAYBACK_STATE playbackState);
            if (playbackState.Equals(PLAYBACK_STATE.STOPPED)) humming.start();
        }
        else
        {
            humming.stop(STOP_MODE.ALLOWFADEOUT);
        }
    }
}
