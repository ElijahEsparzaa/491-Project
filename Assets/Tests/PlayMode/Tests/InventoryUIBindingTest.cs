using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UI;

public class InventoryUIBindingTest
{
    [UnityTest]
    public IEnumerator FirstWeaponSlotIcon_IsBoundToInventory()
    {
        // --- Arrange: create a GameObject to host the InventoryManager ---
        var owner = new GameObject("TestInventoryOwner");
        var inventory = owner.AddComponent<InventoryManager>();

        // Make sure internal lists are properly sized like in the inspector
        inventory.weaponSlots = new List<WeaponController>(new WeaponController[6]);
        inventory.weaponLevels = new int[6];

        inventory.weaponUISlots = new List<Image>(6);
        for (int i = 0; i < 6; i++)
        {
            var slotGO = new GameObject($"WeaponSlot_{i}", typeof(RectTransform), typeof(CanvasRenderer));
            var img = slotGO.AddComponent<Image>();
            img.enabled = false;              // start disabled, like an empty slot
            inventory.weaponUISlots.Add(img);
        }

        // Create minimal weapon data + controller
        var weaponData = ScriptableObject.CreateInstance<WeaponScriptableObjects>();
        // NOTE: we don't assign weaponData.Level here, it's read-only

        var weaponGO = new GameObject("TestWeapon");
        var weaponController = weaponGO.AddComponent<WeaponController>();
        weaponController.weaponData = weaponData;

        // --- Act: add weapon to slot 0 using the real InventoryManager logic ---
        inventory.AddWeapon(0, weaponController);

        // Wait a frame so any side-effects have a chance to run
        yield return null;

        // --- Assert: first UI slot is enabled and bound to the weapon icon ---
        Assert.IsNotNull(inventory.weaponUISlots[0], "First weapon UI slot should exist.");
        Assert.IsTrue(inventory.weaponUISlots[0].enabled,
            "First weapon UI slot should be enabled after adding a weapon.");
        Assert.AreEqual(weaponData.Icon, inventory.weaponUISlots[0].sprite,
            "First weapon UI slot sprite should match the weapon's icon.");
    }
}
