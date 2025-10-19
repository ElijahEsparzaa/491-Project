using System;
using System.Collections.Generic;
using UnityEngine;
public enum InventoryAddResult { AddedNew, LeveledUp, Replaced, FailedFull, FailedDuplicate }

[System.Serializable]
public class WeaponSlot {
    public WeaponController controller;   // instance in scene
    public WeaponScriptableObjects data;  // SO for this instance
}

[System.Serializable]
public class PassiveSlot {
    public PassiveItem instance;                     // instance in scene (if you spawn one)
    public PassiveItemScriptableObject data;         // SO
    public int level;
}

// pass data back to UI
public struct UpgradeOption {
    public Sprite Icon;
    public string Title;
    public string Description;
    public System.Action OnChoose;
}
public class InventoryManager : MonoBehaviour
{
    [Header("Slots & Limits")]
    [SerializeField] int maxWeaponSlots = 6;
    [SerializeField] int maxPassiveSlots = 6;

    [SerializeField] List<WeaponSlot> weaponSlots = new();   // populated at runtime
    [SerializeField] List<PassiveSlot> passiveSlots = new();

    [Header("Pools/Registries (optional)")]
    [SerializeField] List<WeaponScriptableObjects> allWeapons;      // for random level-up choices
    [SerializeField] List<PassiveItemScriptableObject> allPassives; // same

    [Header("Refs")]
    [SerializeField] Transform playerRoot; // where weapons/melee auras will be parented
    [SerializeField] PlayerStats player;

    // UI hooks
    public System.Action<List<WeaponSlot>, List<PassiveSlot>> OnInventoryChanged;
    public System.Action<List<UpgradeOption>> OnOfferUpgrades;

    // call this once from GameManager/PlayerStats after spawn
public void Initialize(PlayerStats stats, Transform playerTransform)
{
    this.player = stats;
    this.playerRoot = playerTransform;
    RaiseInventoryChanged();
}
    // Add or level a weapon by SO
    public InventoryAddResult TryAddWeapon(WeaponScriptableObjects so)
    {
        // already have it?
        var slot = weaponSlots.Find(w => w.data == so);
        if (slot != null)
        {
            // evolve (if NextLevelPrefab exists) OR just replace with next level prefab
            if (so.NextLevelPrefab != null)
            {
                ReplaceWeapon(slot, so.NextLevelPrefab);
                CheckEvolutions(); // in case the new level now meets a requirement
                RaiseInventoryChanged();
                return InventoryAddResult.LeveledUp;
            }
            return InventoryAddResult.FailedDuplicate;
        }

        if (weaponSlots.Count >= maxWeaponSlots) return InventoryAddResult.FailedFull;

        // spawn controller and init
        var go = Instantiate(so.Prefab, playerRoot.position, Quaternion.identity, playerRoot);
        var controller = go.GetComponent<WeaponController>();
        controller.SetData(so, player); // add this method to your base WeaponController
        weaponSlots.Add(new WeaponSlot { controller = controller, data = so });

        CheckEvolutions();
        RaiseInventoryChanged();
        return InventoryAddResult.AddedNew;
    }

    // Add or level a passive by SO
    public InventoryAddResult TryAddPassive(PassiveItemScriptableObject so)
{
    if (so == null)
    {
        Debug.LogError("TryAddPassive: SO was null.");
        return InventoryAddResult.FailedDuplicate;
    }
    if (player == null || playerRoot == null)
    {
        Debug.LogError("TryAddPassive: Inventory not initialized (player/playerRoot is null). Did you call inventory.Initialize(this, transform) in PlayerStats.Awake() before spawning?");
        return InventoryAddResult.FailedDuplicate;
    }

    // already have it?
    int idx = passiveSlots.FindIndex(p => p.data == so);
    if (idx >= 0)
    {
        var slot = passiveSlots[idx];

        // level up via NextLevelPrefab chain
        if (so.NextLevelPrefab != null)
        {
            GameObject nextPrefab = so.NextLevelPrefab;

            var nextComp = nextPrefab.GetComponent<PassiveItem>();
            var nextSO   = nextComp != null ? nextComp.passiveItemData : null;
            if (nextSO == null)
            {
                Debug.LogError("TryAddPassive: NextLevelPrefab is missing PassiveItem/passiveItemData.");
                return InventoryAddResult.FailedDuplicate;
            }

            var passive = Instantiate(nextPrefab, playerRoot.position, Quaternion.identity, playerRoot)
                          .GetComponent<PassiveItem>();
            if (passive == null)
            {
                Debug.LogError("TryAddPassive: Instantiated next level passive missing PassiveItem component.");
                return InventoryAddResult.FailedDuplicate;
            }

            passive.Initialize(player);

            slot.data     = nextSO;
            slot.level   += 1;
            slot.instance = passive;
            passiveSlots[idx] = slot;

            RaiseInventoryChanged();
            CheckEvolutions();
            return InventoryAddResult.LeveledUp;
        }

        // no next level -> duplicate
        return InventoryAddResult.FailedDuplicate;
    }

    // new passive
    if (passiveSlots.Count >= maxPassiveSlots) return InventoryAddResult.FailedFull;

    // Need a base prefab to spawn. Prefer a dedicated Prefab field on the SO.
    if (so.Prefab == null)
    {
        Debug.LogError("TryAddPassive: PassiveItemScriptableObject is missing Prefab. Assign a prefab on the SO (recommended).");
        return InventoryAddResult.FailedDuplicate;
    }

    var inst = Instantiate(so.Prefab, playerRoot.position, Quaternion.identity, playerRoot)
              .GetComponent<PassiveItem>();
    if (inst == null)
    {
        Debug.LogError("TryAddPassive: Prefab is missing PassiveItem component.");
        return InventoryAddResult.FailedDuplicate;
    }

    inst.Initialize(player);

    passiveSlots.Add(new PassiveSlot { data = so, instance = inst, level = 1 });
    RaiseInventoryChanged();
    CheckEvolutions();
    return InventoryAddResult.AddedNew;
}

    private void CheckEvolutions()
    {
        // for each weapon that has an evolution requirement met, evolve it once
        for (int i = 0; i < weaponSlots.Count; i++)
        {
            var slot = weaponSlots[i];
            var req = slot.data.RequiredPassive;
            var evo = slot.data.EvolvedWeapon;
            if (req == null || evo == null) continue;

            bool havePassive = passiveSlots.Exists(p => p.data == req);
            if (!havePassive) continue;

            ReplaceWeapon(slot, evo.Prefab);
            slot.data = evo;
            weaponSlots[i] = slot;
        }
        RaiseInventoryChanged();
    }

    private void ReplaceWeapon(WeaponSlot slot, GameObject newPrefab)
    {
        if (slot.controller != null) Destroy(slot.controller.gameObject);
        var go = Instantiate(newPrefab, playerRoot.position, Quaternion.identity, playerRoot);
        var controller = go.GetComponent<WeaponController>();
        controller.SetData(controller.weaponData, player); // weaponData should be on the new prefab
        slot.controller = controller;
        slot.data = controller.weaponData;
    }
    // Build N upgrade options for the UI (mix of new items and level-ups)
    public void OfferUpgrades(int count)
    {
        var options = new List<UpgradeOption>();

        // 1) prefer level-ups for owned items that still have NextLevelPrefab
        foreach (var w in weaponSlots)
        {
            if (w.data.NextLevelPrefab == null) continue;
            var nextSO = w.data.NextLevelPrefab.GetComponent<WeaponController>().weaponData;
            options.Add(new UpgradeOption {
                Icon = nextSO.Icon,
                Title = $"{nextSO.Name} (Lv {nextSO.Level})",
                Description = nextSO.Description,
                OnChoose = () => TryAddWeapon(nextSO)
            });
        }
        foreach (var p in passiveSlots)
        {
            if (p.data.NextLevelPrefab == null) continue;
            var nextSO = p.data.NextLevelPrefab.GetComponent<PassiveItem>().passiveItemData;
            options.Add(new UpgradeOption {
                Icon = nextSO.Icon,
                Title = $"{nextSO.Name} (Lv {p.level + 1})",
                Description = nextSO.Description,
                OnChoose = () => TryAddPassive(nextSO)
            });
        }

        // 2) add some new items from the registries (not already owned)
        var freeWeapons = allWeapons.FindAll(w => !weaponSlots.Exists(s => s.data == w));
        var freePassives = allPassives.FindAll(p => !passiveSlots.Exists(s => s.data == p));

        // simple random fill
        System.Random rng = new();
        while (options.Count < count && (freeWeapons.Count > 0 || freePassives.Count > 0))
        {
            bool pickWeapon = freePassives.Count == 0 || (freeWeapons.Count > 0 && rng.NextDouble() < 0.5);
            if (pickWeapon)
            {
                int i = rng.Next(freeWeapons.Count);
                var so = freeWeapons[i];
                options.Add(new UpgradeOption {
                    Icon = so.Icon,
                    Title = $"{so.Name} (Lv {so.Level})",
                    Description = so.Description,
                    OnChoose = () => TryAddWeapon(so)
                });
                freeWeapons.RemoveAt(i);
            }
            else
            {
                int i = rng.Next(freePassives.Count);
                var so = freePassives[i];
                options.Add(new UpgradeOption {
                    Icon = so.Icon,
                    Title = $"{so.Name}",
                    Description = so.Description,
                    OnChoose = () => TryAddPassive(so)
                });
                freePassives.RemoveAt(i);
            }
        }

        // tell UI to render buttons; each button calls its OnChoose then closes
        OnOfferUpgrades?.Invoke(options);
    }
    private void RaiseInventoryChanged()
    {
        OnInventoryChanged?.Invoke(weaponSlots, passiveSlots);
    }
    // temporary public getters so old code can read them

// (Optional) temporary legacy fields if PlayerStats expects them:
public List<GameObject> weaponUISlots;        // TODO: delete after wiring OnInventoryChanged
public List<GameObject> passiveItemUISlots;   // TODO: delete after wiring OnInventoryChanged
public IReadOnlyList<WeaponSlot> WeaponSlots => weaponSlots;
public IReadOnlyList<PassiveSlot> PassiveSlots => passiveSlots;

}
