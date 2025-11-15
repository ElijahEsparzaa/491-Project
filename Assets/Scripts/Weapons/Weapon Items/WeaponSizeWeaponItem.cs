using UnityEngine;

public class WeaponSizeWeaponItem : WeaponController, IWeaponStatModifier
{
    [SerializeField]
    [Tooltip("Multiplier applied to weapon hitbox size.")]
    float weaponSizeMultiplier = 1.15f;

    public void ApplyModifier(PlayerStats player)
    {
        if(player == null)
        {
            return;
        }

        player.CurrentWeaponSizeMultiplier *= Mathf.Max(0.1f, weaponSizeMultiplier);
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        // Stat items do not perform weapon attacks.
    }

    protected override void Attack()
    {
        // Intentionally empty. This item only modifies weapon size.
    }
}
