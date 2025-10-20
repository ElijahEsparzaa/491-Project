using UnityEngine;

public abstract class PassiveItem : MonoBehaviour
{
    public PassiveItemScriptableObject passiveItemData;
    protected PlayerStats player;

    // Called by InventoryManager after instantiation
    public void Initialize(PlayerStats p)
    {
        player = p;
        ApplyModifier();
    }

    protected abstract void ApplyModifier();
}
