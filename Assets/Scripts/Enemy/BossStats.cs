using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStats : EnemyStats
{
    public override void Kill()
    {
        base.Kill();

        // Notify spawner that the boss was defeated
        EnemySpawner spawner = FindObjectOfType<EnemySpawner>();
        if (spawner != null)
        {
            spawner.BossDefeated();
        }

        Debug.Log("Boss defeated!");
    }
}