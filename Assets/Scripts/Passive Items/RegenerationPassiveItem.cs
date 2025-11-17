using UnityEngine;

public class RegenerationPassiveItem : PassiveItem
{
    protected override void ApplyModifier()
    {
        if(player == null || passiveItemData == null)
        {
            return;
        }

        player.CurrentRecovery *= 1 + passiveItemData.Multiplier / 100f;
    }
}
