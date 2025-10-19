using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public EnemyScriptableObjects enemyData;

    [HideInInspector] public float currentMoveSpeed;
    [HideInInspector] public float currentHealth;
    [HideInInspector] public float currentDamage;

    public float despawnDistance = 20f;

    Transform player;
    EnemySpawner spawner;
    bool isQuitting;

    void Awake()
    {
        // guard against missing data to avoid future NREs
        if (enemyData != null)
        {
            currentMoveSpeed = enemyData.MoveSpeed;
            currentHealth   = enemyData.MaxHealth;
            currentDamage   = enemyData.Damage;
        }
        else
        {
            Debug.LogWarning($"{name}: EnemyScriptableObjects is missing.");
        }
    }

    void Start()
    {
        var ps = FindObjectOfType<PlayerStats>();
        player = ps ? ps.transform : null;

        spawner = FindObjectOfType<EnemySpawner>(); // cache once
    }

    void Update()
    {
        if (player && Vector2.Distance(transform.position, player.position) >= despawnDistance)
        {
            ReturnEnemy();
        }
    }

    public void TakeDamage(float dmg)
    {
        currentHealth -= dmg;
        if (currentHealth <= 0) Kill();
    }

    public void Kill()
    {
        // if you need to notify spawner on kill, do it here (while refs still valid)
        if (spawner) spawner.OnEnemyKilled();

        Destroy(gameObject);
    }

    private void OnCollisionStay2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            var ps = col.gameObject.GetComponent<PlayerStats>();
            if (ps) ps.TakeDamage(currentDamage);
        }
    }

    void OnApplicationQuit() => isQuitting = true;

    private void OnDestroy()
    {
        // Don’t run gameplay logic during teardown/domain reload
        if (!Application.isPlaying || isQuitting) return;

        // If you keep notifications here, guard them:
        if (spawner) spawner.OnEnemyKilled();
    }

    void ReturnEnemy()
    {
        if (spawner && player)
        {
            int randomIndex = Random.Range(0, spawner.relativeSpawnPoints.Count);
            Vector3 spawnOffset = spawner.relativeSpawnPoints[randomIndex].localPosition;
            transform.position = player.position + spawnOffset;
        }
        else
        {
            Debug.LogWarning("EnemySpawner or Player not found!");
        }
    }
}
