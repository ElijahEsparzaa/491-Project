using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public EnemyScriptableObjects enemyData;

    //Stats
    [HideInInspector]
    public float currentMoveSpeed;
    [HideInInspector]
    public float currentHealth;
    [HideInInspector]
    public float currentDamage;

    public float despawnDistance = 20f;
    Transform player;

    void Awake()
    {
        currentMoveSpeed = enemyData.MoveSpeed;
        currentHealth = enemyData.MaxHealth;
        currentDamage = enemyData.Damage;
    }

    void Start()
    {
        //Cache player reference once globally
        if (player == null)
        player = GameObject.FindObjectOfType<PlayerStats>().transform;
    }

    void Update()
    {
        if(UnityEngine.Vector2.Distance(transform.position, player.position) >= despawnDistance)
        {
            ReturnEnemy();
        }
    }

    public void TakeDamage(float dmg)
    {
        currentHealth -= dmg;

        if(currentHealth <= 0)
        {
            Kill();
        }
    }

    public virtual void Kill()
    {
        Destroy(gameObject);
    }

    private void OnCollisionStay2D(Collision2D col)
    {
        if(col.gameObject.CompareTag("Player"))
        {
            PlayerStats player = col.gameObject.GetComponent<PlayerStats>();
            player.TakeDamage(currentDamage);
        }
    }
    
    private void OnDestroy()
    {
        EnemySpawner es = FindObjectOfType<EnemySpawner>();
        if (es != null)
        {
            es.OnEnemyKilled();
        }
        else
        {
            Debug.LogWarning("EnemySpawner not found when destroying " + gameObject.name);
        }
    }

    void ReturnEnemy()
    {
        EnemySpawner es = FindObjectOfType<EnemySpawner>();
        if (es != null)
        {
            //Ensure relativeSpawnPoints uses localPosition for offsets
            int randomIndex = Random.Range(0, es.relativeSpawnPoints.Count);
            UnityEngine.Vector3 spawnOffset = es.relativeSpawnPoints[randomIndex].localPosition;
            transform.position = player.position + spawnOffset;
        }
        else
        {
            Debug.LogWarning("EnemySpawner not found!");
        }
    }
}
