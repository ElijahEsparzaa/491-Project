using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;

public class InventoryAndStatPlayModeTests
{
    [UnityTest]
    public IEnumerator AddingWeaponStatItemAppliesAttackSpeedMultiplier()
    {
        using (var context = new InventoryTestContext())
        {
            context.Player.CurrentAttackSpeedMultiplier = 1f;

            var weaponData = ScriptableObject.CreateInstance<WeaponScriptableObjects>();
            ReflectionTestHelpers.SetPrivateField(weaponData, "level", 1);
            ReflectionTestHelpers.SetPrivateField(weaponData, "name", "Attack Speed Trinket");
            ReflectionTestHelpers.SetPrivateField(weaponData, "description", "Improves attack speed");

            var weaponGO = new GameObject("AttackSpeedItem");
            var weapon = weaponGO.AddComponent<AttackSpeedWeaponItem>();
            weapon.weaponData = weaponData;

            context.Inventory.AddWeapon(0, weapon);

            Assert.AreSame(weapon, context.Inventory.weaponSlots[0]);
            Assert.AreEqual(0.9f, context.Player.CurrentAttackSpeedMultiplier, 0.001f);

            yield return null;

            UnityEngine.Object.DestroyImmediate(weaponGO);
            UnityEngine.Object.DestroyImmediate(weaponData);
        }
    }

    [UnityTest]
    public IEnumerator HeartPassiveItemBoostsMaxHealthAndHealsPlayer()
    {
        using (var context = new InventoryTestContext())
        {
            context.Player.CurrentMaxHealth = 100f;
            context.Player.CurrentHealth = 50f;

            var heartData = PassiveItemTestFactory.CreatePassiveItemData(20f, "Heart", "Boosts health");

            var heartGO = new GameObject("HeartPassive");
            var heart = heartGO.AddComponent<HeartPassiveItem>();
            heart.passiveItemData = heartData;

            context.Inventory.AddPassiveItem(0, heart);

            yield return null;

            Assert.AreSame(heart, context.Inventory.passiveItemSlots[0]);
            Assert.AreEqual(120f, context.Player.CurrentMaxHealth, 0.001f);
            Assert.AreEqual(70f, context.Player.CurrentHealth, 0.001f);

            UnityEngine.Object.DestroyImmediate(heartGO);
            UnityEngine.Object.DestroyImmediate(heartData);
        }
    }

    [UnityTest]
    public IEnumerator RegenerationPassiveItemImprovesRecovery()
    {
        using (var context = new InventoryTestContext())
        {
            context.Player.CurrentRecovery = 2f;

            var regenData = PassiveItemTestFactory.CreatePassiveItemData(50f, "Regeneration", "Boosts recovery");

            var regenGO = new GameObject("RegenerationPassive");
            var regen = regenGO.AddComponent<RegenerationPassiveItem>();
            regen.passiveItemData = regenData;

            context.Inventory.AddPassiveItem(1, regen);

            yield return null;

            Assert.AreSame(regen, context.Inventory.passiveItemSlots[1]);
            Assert.AreEqual(3f, context.Player.CurrentRecovery, 0.001f);

            UnityEngine.Object.DestroyImmediate(regenGO);
            UnityEngine.Object.DestroyImmediate(regenData);
        }
    }
}

internal static class PassiveItemTestFactory
{
    public static PassiveItemScriptableObject CreatePassiveItemData(float multiplier, string name, string description)
    {
        var data = ScriptableObject.CreateInstance<PassiveItemScriptableObject>();
        ReflectionTestHelpers.SetPrivateField(data, "multiplier", multiplier);
        ReflectionTestHelpers.SetPrivateField(data, "level", 1);
        ReflectionTestHelpers.SetPrivateField(data, "name", name);
        ReflectionTestHelpers.SetPrivateField(data, "description", description);
        return data;
    }
}

internal sealed class InventoryTestContext : IDisposable
{
    public GameObject PlayerObject { get; }
    public TestPlayerStats Player { get; }
    public InventoryManager Inventory { get; }

    readonly List<GameObject> uiObjects = new List<GameObject>();

    public InventoryTestContext()
    {
        PlayerObject = new GameObject("TestPlayer");
        Player = PlayerObject.AddComponent<TestPlayerStats>();
        Inventory = PlayerObject.AddComponent<InventoryManager>();

        InitializeLists();

        Player.CurrentMaxHealth = 100f;
        Player.CurrentHealth = 100f;
        Player.CurrentAttackSpeedMultiplier = 1f;
        Player.CurrentWeaponSizeMultiplier = 1f;
        Player.CurrentProjectileSpeed = 1f;
        Player.CurrentRecovery = 1f;
    }

    void InitializeLists()
    {
        Inventory.weaponSlots.Clear();
        Inventory.passiveItemSlots.Clear();
        Inventory.weaponUISlots.Clear();
        Inventory.passiveItemUISlots.Clear();

        for (int i = 0; i < 6; i++)
        {
            Inventory.weaponSlots.Add(null);
            Inventory.passiveItemSlots.Add(null);

            var weaponUi = new GameObject($"WeaponSlot_{i}");
            uiObjects.Add(weaponUi);
            Inventory.weaponUISlots.Add(weaponUi.AddComponent<Image>());

            var passiveUi = new GameObject($"PassiveSlot_{i}");
            uiObjects.Add(passiveUi);
            Inventory.passiveItemUISlots.Add(passiveUi.AddComponent<Image>());
        }
    }

    public void Dispose()
    {
        UnityEngine.Object.DestroyImmediate(PlayerObject);
        foreach (var go in uiObjects)
        {
            UnityEngine.Object.DestroyImmediate(go);
        }
        uiObjects.Clear();
    }
}

internal class TestPlayerStats : PlayerStats
{
    public new void Awake() { }
    public new void Start() { }
}

internal static class ReflectionTestHelpers
{
    public static void SetPrivateField<T>(T target, string fieldName, object value)
    {
        var field = typeof(T).GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
        if (field == null)
        {
            throw new MissingFieldException(typeof(T).Name, fieldName);
        }

        field.SetValue(target, value);
    }
}
