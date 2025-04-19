using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;

public class LightBeamTower : TowerBehavior
{
    public float chainRange = 5f;
    public int maxChainCount = 25;
    public float damage = 1f;
    public LineRenderer beam;
    public float beamWidth = 0.1f;
    [Range(0f, 0.9f), Tooltip("The fraction of the tower's shoot cooldown by which the beam visual will last")]
    public float beamDurationFraction = 1f;
    [SerializeField] private LayerMask enemyLayer;

    private Transform beamTarget;
    private Collider[] chainRangeResults;
    private readonly HashSet<GameObject> targetsAlreadyHit = new();

    private int shotCtr;

    private void Awake()
    {
        chainRangeResults = new Collider[maxChainCount];
        shotCtr = 0;
        beam.enabled = false;
    }

    public override void Fire()
    {
        if (upgrade3Unlocked) shotCtr++;
        beamTarget = tower.target;

        List<Vector3> beamTargets = new(maxChainCount + 1) { transform.position };

        int currentChain;
        if (shotCtr >= 3) 
        {
            currentChain = EnemyManager.Instance.totalEnemiesInWave;
            chainRange = float.MaxValue;
        }
        else
        {
            currentChain = maxChainCount;
            chainRange = 0.5f;
        }

        for (int i = 0; i < currentChain; ++i)
        {
            targetsAlreadyHit.Add(beamTarget.gameObject);
            beamTargets.Add(beamTarget.position + Vector3.back);

            Vector3 beamTargetPos = Tower.Get3DTargetPos(beamTarget);

            EnemyManager.Instance.EnemyTakeDamage(beamTarget.gameObject, (int)damage);
            tower.totalDamageDealt += damage;

            int resultCount = Physics.OverlapSphereNonAlloc(beamTargetPos, chainRange, chainRangeResults, enemyLayer);

            Transform nextTarget = null;
            float minDistance = float.MaxValue;

            for (int j = 0; j < resultCount; ++j)
            {
                Collider collider = chainRangeResults[j];
                float squaredDistance = Vector3.SqrMagnitude(collider.transform.position - transform.position);
                if (squaredDistance < minDistance && !targetsAlreadyHit.Contains(collider.gameObject))
                {
                    minDistance = squaredDistance;
                    nextTarget = collider.transform;
                }
            }

            if (nextTarget == null) break;

            beamTarget = nextTarget;
        }

        targetsAlreadyHit.Clear();

        if (beamTargets.Count == 1) return; // No enemies hit if only target is the beam origin

        StartCoroutine(DoBeamVisual(beamTargets.ToArray()));
        // stuff
        AudioManager.Instance.PlayOneShot(AudioManager.Instance.beamFireSound);
        if (shotCtr >= 3) shotCtr = 0;
    }

    private IEnumerator DoBeamVisual(Vector3[] targetPositions)
    {
        beam.enabled = true;
        beam.startWidth = beamWidth;
        beam.endWidth = beamWidth;

        beam.positionCount = targetPositions.Length;
        beam.SetPositions(targetPositions);

        yield return new WaitForSeconds(tower.shootCooldown * beamDurationFraction);

        beam.enabled = false;
    }

    public override void OnTier1Upgrade()
    {
        upgrade1Unlocked = true;
        damage *= 2.0f;
        maxChainCount = (int)((float)maxChainCount * 1.5f);
        return;
    }
    public override void OnTier2Upgrade()
    {
        upgrade2Unlocked = true;
        tower.shootCooldown *= 0.66f;
        return;
    }
    public override void OnTier3Upgrade()
    {
        upgrade3Unlocked = true;
        return;
    }
}
