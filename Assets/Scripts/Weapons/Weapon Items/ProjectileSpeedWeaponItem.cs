using UnityEngine;

public class ProjectileSpeedWeaponItem : WeaponController, IWeaponStatModifier
{
    [SerializeField]
    [Tooltip("Multiplier applied to projectile travel speed.")]
    float projectileSpeedMultiplier = 1.1f;

    public void ApplyModifier(PlayerStats player)
    {
        if(player == null)
        {
            return;
        }

        player.CurrentProjectileSpeed *= Mathf.Max(0.1f, projectileSpeedMultiplier);
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        // Stat items do not spawn projectiles.
    }

    protected override void Attack()
    {
        // Intentionally empty. This item only modifies projectile stats.
    }
}
