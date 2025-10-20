using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public WeaponScriptableObjects weaponData;

    protected PlayerMovement pm;
    protected PlayerStats player;
    protected float cooldownTimer;

    // Allow InventoryManager to inject references
    public void SetData(WeaponScriptableObjects data, PlayerStats p)
    {
        weaponData = data;
        player = p;
        pm = p.GetComponent<PlayerMovement>();
        cooldownTimer = weaponData != null ? weaponData.CooldownDuration : 0f;
    }

    protected virtual void Start()
    {
        // Fallback if SetData wasn't called (keeps older content working)
        if (player == null) player = FindObjectOfType<PlayerStats>();
        if (pm == null && player != null) pm = player.GetComponent<PlayerMovement>();
        if (weaponData != null) cooldownTimer = weaponData.CooldownDuration;
    }

    protected virtual void Update()
    {
        if (weaponData == null) return;
        cooldownTimer -= Time.deltaTime;
        if (cooldownTimer <= 0f)
        {
            Attack();
            cooldownTimer = weaponData.CooldownDuration;
        }
    }

    protected virtual void Attack() { }
}
