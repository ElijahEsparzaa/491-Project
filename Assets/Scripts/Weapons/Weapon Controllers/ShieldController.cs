using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// ShieldController.cs
public class ShieldController : WeaponController
{
    protected override void Start()
    {
        base.Start();
    }

    protected override void Attack()
    {
        base.Attack();
        var spawnedGarlic = Instantiate(weaponData.Prefab);
        spawnedGarlic.transform.position = transform.position;
        spawnedGarlic.transform.SetParent(transform);
        spawnedGarlic.transform.localPosition = Vector3.zero; // keep it centered on player
    }
}
