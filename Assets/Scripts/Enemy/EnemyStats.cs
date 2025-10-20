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
        //Guard against missing data
        if (enemyData != null)
        {
            currentMoveSpeed = enemyData.MoveSpeed;
            currentHealth = enemyData.MaxHealth;
            currentDamage = enemyData.Damage;
        }
        else
        {
            Debug.LogWarning($"{name}: EnemyScriptableObjects is missing.");
        }
    }

    void Start()
    {
        //Cache Player reference once (supports both implementations)
        if (player == null)
        {
            var ps = FindObjectOfType<PlayerStats>();
            player = ps ? ps.transform : null;
        }

        //Cache EnemySpawner once (needed by both versions)
        if (spawner == null)
        {
            spawner = FindObjectOfType<EnemySpawner>();
        }
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
        if (currentHealth <= 0)
            Kill();
    }

    public virtual void Kill()
    {
        //Notify spawner when enemy dies
        if (spawner)
            spawner.OnEnemyKilled();

        Destroy(gameObject);
    }

    private void OnCollisionStay2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            var ps = col.gameObject.GetComponent<PlayerStats>();
            if (ps)
                ps.TakeDamage(currentDamage);
        }
    }

    void OnApplicationQuit() => isQuitting = true;

    private void OnDestroy()
    {
        //Prevent running logic during teardown/domain reload
        if (!Application.isPlaying || isQuitting)
            return;

        //Safely notify spawner if available
        if (spawner)
        {
            spawner.OnEnemyKilled();
        }
        else
        {
            var es = FindObjectOfType<EnemySpawner>();
            if (es != null)
            {
                es.OnEnemyKilled();
            }
            else
            {
                Debug.LogWarning($"EnemySpawner not found when destroying {gameObject.name}");
            }
        }
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