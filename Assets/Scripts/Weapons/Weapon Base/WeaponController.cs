using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class WeaponController : MonoBehaviour
{
    [Header("Weapon Stats")]
    public WeaponScriptableObjects weaponData;
    float currentCooldown;

    protected PlayerStats playerStats;

    protected PlayerMovement pm;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        pm = FindObjectOfType<PlayerMovement>();
        playerStats = FindObjectOfType<PlayerStats>();
        currentCooldown = GetModifiedCooldown();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        currentCooldown -= Time.deltaTime;
        if(currentCooldown <= 0f)
        {
            Attack();
        }
    }

    protected virtual void Attack()
    {
        currentCooldown = GetModifiedCooldown();
    }

    protected virtual float GetModifiedCooldown()
    {
        float cooldownMultiplier = 1f;
        if (playerStats != null)
        {
            cooldownMultiplier = playerStats.CurrentAttackSpeedMultiplier;
        }

        return weaponData.CooldownDuration * cooldownMultiplier;
    }
}
