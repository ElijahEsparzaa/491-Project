using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private InventoryManager inventory; // drag Player's InventoryManager here

    [Header("Slots")]
    [SerializeField] private List<Image> weaponSlots;   // drag the 6 weapon Images
    [SerializeField] private List<Image> passiveSlots;  // drag the 6 passive Images

    [Header("Visuals")]
    [SerializeField] private Sprite emptySlotSprite; // frame icon for empty

    void Awake()
    {
        if (!inventory)
            inventory = FindObjectOfType<InventoryManager>();
    }

    void OnEnable()
    {
        // subscribe to inventory updates
        if (inventory != null) inventory.OnInventoryChanged += UpdateUI;
        // draw once at start (in case items exist)
        if (inventory != null) UpdateUI(
            new List<WeaponSlot>(inventory.WeaponSlots),
            new List<PassiveSlot>(inventory.PassiveSlots)
        );
    }

    void OnDisable()
    {
        if (inventory != null) inventory.OnInventoryChanged -= UpdateUI;
    }

    public void UpdateUI(List<WeaponSlot> weapons, List<PassiveSlot> passives)
    {
        // weapons
        for (int i = 0; i < weaponSlots.Count; i++)
        {
            var img = weaponSlots[i];
            if (i < weapons.Count && weapons[i]?.data?.Icon != null)
            {
                img.sprite = weapons[i].data.Icon;
                img.color = Color.white;
            }
            else
            {
                img.sprite = emptySlotSprite;
                img.color = new Color(1f,1f,1f,0.25f);
            }
        }

        // passives
        for (int i = 0; i < passiveSlots.Count; i++)
        {
            var img = passiveSlots[i];
            if (i < passives.Count && passives[i]?.data?.Icon != null)
            {
                img.sprite = passives[i].data.Icon;
                img.color = Color.white;
            }
            else
            {
                img.sprite = emptySlotSprite;
                img.color = new Color(1f,1f,1f,0.25f);
            }
        }
    }
}
