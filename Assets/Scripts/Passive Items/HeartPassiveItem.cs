using UnityEngine;

public class HeartPassiveItem : PassiveItem
{
    protected override void ApplyModifier()
    {
        if(player == null || passiveItemData == null)
        {
            return;
        }

        float multiplier = passiveItemData.Multiplier / 100f;
        float previousMax = player.CurrentMaxHealth;
        player.CurrentMaxHealth *= 1 + multiplier;
        float additionalHealth = player.CurrentMaxHealth - previousMax;
        player.CurrentHealth += additionalHealth;
    }
}
