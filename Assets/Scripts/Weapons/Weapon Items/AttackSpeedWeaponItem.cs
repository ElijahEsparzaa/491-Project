using UnityEngine;

public class AttackSpeedWeaponItem : WeaponController, IWeaponStatModifier
{
    [SerializeField]
    [Tooltip("Multiplier applied to weapon cooldown. Values below 1 speed up attacks.")]
    float cooldownMultiplier = 0.9f;

    public void ApplyModifier(PlayerStats player)
    {
        if(player == null)
        {
            return;
        }

        player.CurrentAttackSpeedMultiplier *= Mathf.Max(0.1f, cooldownMultiplier);
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        // Stat items do not trigger attacks every frame.
    }

    protected override void Attack()
    {
        // Intentionally empty. This item only modifies player stats.
    }
}
