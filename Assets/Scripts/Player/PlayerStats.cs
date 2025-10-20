using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
   CharacterScriptableObjects characterData;

   
   float currentHealth;
   
   float currentRecovery;
   
   float currentMoveSpeed;
   
   float currentMight;
   
   float currentProjectileSpeed;
   
   float currentMagnet;

   #region Current Stats Properties

   public float CurrentHealth
   {
      get { return currentHealth; }
      set 
      {
         //Checks if value has changed
         if (currentHealth != value)
         {
            currentHealth = value;
            if(GameManager.instance != null)
            {
               GameManager.instance.currentHealthDisplay.text = "Health: " + currentHealth;
            }
         }
      }
   }

   public float CurrentRecovery
   {
      get { return currentRecovery; }
      set 
      {
         //Checks if value has changed
         if (currentRecovery != value)
         {
            currentRecovery = value;
            if(GameManager.instance != null)
            {
               GameManager.instance.currentRecoveryDisplay.text = "Recovery: " + currentRecovery;
            }
         }
      }
   }

   public float CurrentMoveSpeed
   {
      get { return currentMoveSpeed; }
      set 
      {
         //Checks if value has changed
         if (currentMoveSpeed != value)
         {
            currentMoveSpeed = value;
            if(GameManager.instance != null)
            {
               GameManager.instance.currentMoveSpeedDisplay.text = "Move Speed: " + currentMoveSpeed;
            }
         }
      }
   }

   public float CurrentMight
   {
      get { return currentMight; }
      set 
      {
         //Checks if value has changed
         if (currentMight != value)
         {
            currentMight = value;
            if(GameManager.instance != null)
            {
               GameManager.instance.currentMightDisplay.text = "Might: " + currentMight;
            }
         }
      }
   }

   public float CurrentProjectileSpeed
   {
      get { return currentProjectileSpeed; }
      set 
      {
         //Checks if value has changed
         if (currentProjectileSpeed != value)
         {
            currentProjectileSpeed = value;
            if(GameManager.instance != null)
            {
               GameManager.instance.currentProjectileSpeedDisplay.text = "Projectile Speed: " + currentProjectileSpeed;
            }
         }
      }
   }

   public float CurrentMagnet
   {
      get { return currentMagnet; }
      set 
      {
         //Checks if value has changed
         if (currentMagnet != value)
         {
            currentMagnet = value;
            if(GameManager.instance != null)
            {
               GameManager.instance.currentMagnetDisplay.text = "Magnet: " + currentMagnet;
            }
         }
      }
   }
   #endregion

   [Header("Experience/Level")]
   public int experience = 0;
   public int level = 1;
   public int experienceCap;

   [System.Serializable]
   public class LevelRange
   {
      public int startLevel;
      public int endLevel;
      public int experienceCapIncrease;
   }

   [Header("I-Frames")]
   public float invincibilityDuration;
   float invincibilityTimer;
   bool isInvincible;

   public List<LevelRange> levelRanges;

   InventoryManager inventory;
   public int weaponIndex;
   public int passiveItemIndex;

   public GameObject secondWeaponTest;
   public GameObject firstPassiveItemTest, secondPassiveItemTest;

    void Awake()
{
    // 1) grab character data
    characterData = CharacterSelector.GetData();
    if (characterData == null)
    {
        Debug.LogError("PlayerStats: Character data was null. Did you load this scene from the Character Select?");
        return; // bail to avoid NREs
    }

    // 2) if you use a CharacterSelector singleton, it’s fine to destroy it
    if (CharacterSelector.instance != null)
        CharacterSelector.instance.DestroySingleton();

    // 3) wire inventory now (so anything that needs it has the refs)
    inventory = GetComponent<InventoryManager>();
    if (inventory != null)
        inventory.Initialize(this, transform);

    // 4) assign stats safely from characterData
    CurrentHealth          = characterData.MaxHealth;
    CurrentRecovery        = characterData.Recovery;
    CurrentMoveSpeed       = characterData.MoveSpeed;
    CurrentMight           = characterData.Might;
    CurrentProjectileSpeed = characterData.ProjectileSpeed;
    CurrentMagnet          = characterData.Magnet;

    // 5) spawn start items only if they exist
    if (characterData.StartingWeapon != null)
        SpawnWeapon(characterData.StartingWeapon);

    if (firstPassiveItemTest != null)
        SpawnPassiveItem(firstPassiveItemTest);

    if (secondPassiveItemTest != null)
        SpawnPassiveItem(secondPassiveItemTest);
}

   void Start()
{
    if (levelRanges != null && levelRanges.Count > 0)
        experienceCap = levelRanges[0].experienceCapIncrease;

    // Safely update HUD if GameManager & UI fields exist
    if (GameManager.instance != null)
    {
        if (GameManager.instance.currentHealthDisplay != null)
            GameManager.instance.currentHealthDisplay.text = "Health: " + CurrentHealth;
        if (GameManager.instance.currentRecoveryDisplay != null)
            GameManager.instance.currentRecoveryDisplay.text = "Recovery: " + CurrentRecovery;
        if (GameManager.instance.currentMoveSpeedDisplay != null)
            GameManager.instance.currentMoveSpeedDisplay.text = "Move Speed: " + CurrentMoveSpeed;
        if (GameManager.instance.currentMightDisplay != null)
            GameManager.instance.currentMightDisplay.text = "Might: " + CurrentMight;
        if (GameManager.instance.currentProjectileSpeedDisplay != null)
            GameManager.instance.currentProjectileSpeedDisplay.text = "Projectile Speed: " + CurrentProjectileSpeed;
        if (GameManager.instance.currentMagnetDisplay != null)
            GameManager.instance.currentMagnetDisplay.text = "Magnet: " + CurrentMagnet;

        // Only assign chosen character UI if we actually have data
        if (characterData != null)
            GameManager.instance.AssignChosenCharacterUI(characterData);
        else
            Debug.LogWarning("PlayerStats: characterData was null at Start(); skipped AssignChosenCharacterUI.");
    }
    else
    {
        Debug.LogWarning("PlayerStats: GameManager.instance missing; skipped HUD setup.");
    }
}


   public void IncreaseExperience(int amount)
   {
      experience += amount;
      LevelUpChecker();
   }

   void LevelUpChecker()
   {
      if(experience >= experienceCap)
      {
         level++;
         experience -= experienceCap;

         int experienceCapIncrease = 0;
         foreach(LevelRange range in levelRanges)
         {
            if(level >= range.startLevel && level <= range.endLevel)
            {
               experienceCapIncrease = range.experienceCapIncrease;
               break;
            }
         }
         experienceCap += experienceCapIncrease;

         GameManager.instance.StartLevelUp();
      }
   }

   void Update()
   {
      if(invincibilityTimer > 0)
      {
         invincibilityTimer -= Time.deltaTime;
      }
      else if (isInvincible)
      {
         isInvincible = false;
      }

      if (characterData != null)
        Recover();
   }

   public void TakeDamage(float dmg)
   {
      if(!isInvincible)
      {
         CurrentHealth -= dmg;

         invincibilityTimer = invincibilityDuration;
         isInvincible = true;

         if(CurrentHealth <= 0)
         {
            Kill();
         }
      }
   }

   public void Kill()
   {
      if(!GameManager.instance.isGameOver)
      {
         GameManager.instance.AssignLevelReachedUI(level);
         var weaponImages = inventory.weaponUISlots
            .Select(go => go ? go.GetComponent<Image>() : null)
            .Where(img => img != null)
            .ToList();

         var passiveImages = inventory.passiveItemUISlots
            .Select(go => go ? go.GetComponent<Image>() : null)
            .Where(img => img != null)
            .ToList();

         //AssignChosenWeaponsAndPassiveItemsUI(weaponImages, passiveImages); This code is technically not necessary anymore however I left it in just incase we find a bug in the future that requires it.

         GameManager.instance.GameOver();
      }
   }

   public void RestoreHealth(float amount)
   {
      if(CurrentHealth < characterData.MaxHealth)
      {
         CurrentHealth += amount;

         if(CurrentHealth > characterData.MaxHealth)
         {
            CurrentHealth = characterData.MaxHealth;
         }
      }
   }

   float recoverTimer;

void Recover()
{
    // If no character data, or no recovery set, do nothing
    if (characterData == null || CurrentRecovery <= 0f) return;

    // If already full HP, nothing to do
    if (CurrentHealth >= characterData.MaxHealth) return;

    recoverTimer += Time.deltaTime;
    if (recoverTimer < 1f) return; // tick once per second (or whatever you use)

    recoverTimer = 0f;
    CurrentHealth = Mathf.Min(CurrentHealth + CurrentRecovery, characterData.MaxHealth);

    // Safely update UI if it exists
    if (GameManager.instance != null && GameManager.instance.currentHealthDisplay != null)
    {
        GameManager.instance.currentHealthDisplay.text = "Health: " + CurrentHealth;
    }
}


public void SpawnWeapon(GameObject weaponPrefab)
{
    var wc = weaponPrefab.GetComponent<WeaponController>();
    if (wc == null || wc.weaponData == null)
    {
        Debug.LogError("Starting weapon prefab is missing WeaponController or weaponData.");
        return;
    }

    // Let InventoryManager handle instantiation & slots
    inventory.TryAddWeapon(wc.weaponData);
}


public void SpawnPassiveItem(GameObject passiveItemPrefab)
{
    var pi = passiveItemPrefab.GetComponent<PassiveItem>();
    if (pi == null || pi.passiveItemData == null)
    {
        Debug.LogError("Starting passive prefab is missing PassiveItem or passiveItemData.");
        return;
    }

    // Let InventoryManager handle instantiation & slots
    inventory.TryAddPassive(pi.passiveItemData);
}

}
