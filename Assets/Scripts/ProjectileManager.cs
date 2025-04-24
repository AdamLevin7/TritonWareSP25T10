using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ProjectileManager : MonoBehaviour
{
    public static ProjectileManager Instance;

    [SerializeField] private Projectile basicProjectilePrefab;
    public ObjectPool<Projectile> basicProjectilePool;

    [SerializeField] private AOEProjectile aoeProjectilePrefab;
    public ObjectPool<AOEProjectile> aoeProjectilePool;

    private const int MAX_PROJECTILES = 100;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;

        basicProjectilePool = new(
            () => {
                Projectile projectile = Instantiate(basicProjectilePrefab);
                projectile.InitializeProjectile();
                return projectile;
            },
            projectile => {
                projectile.gameObject.SetActive(true);
                projectile.InitializeProjectile();
            },
            projectile => projectile.gameObject.SetActive(false),
            projectile => Destroy(projectile.gameObject),
            false,
            MAX_PROJECTILES
        );

        aoeProjectilePool = new(
            () => {
                AOEProjectile projectile = Instantiate(aoeProjectilePrefab);
                projectile.InitializeProjectile();
                return projectile;
            },
            projectile => {
                projectile.gameObject.SetActive(true);
                projectile.InitializeProjectile();
            },
            projectile => projectile.gameObject.SetActive(false),
            projectile => Destroy(projectile.gameObject),
            false,
            MAX_PROJECTILES
        );
    }
}
