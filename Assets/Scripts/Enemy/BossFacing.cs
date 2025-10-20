using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFacing : MonoBehaviour
{
    private Transform player;
    private bool facingRight = true;
    private EnemyStats stats;

    [Header("Assign in Inspector")]
    [SerializeField] private Transform visualRoot; //The Visual child
    [SerializeField] private List<Transform> ikTargets = new List<Transform>();

    void Start()
    {
        player = FindObjectOfType<PlayerStats>()?.transform;
        stats = GetComponent<EnemyStats>();
    }

    void Update()
    {
        if (stats == null || stats.enemyData == null || player == null)
            return;

        //Only run this logic for the boss
        if (stats.enemyData.name.Contains("Boss"))
        {
            bool playerIsLeft = player.position.x < transform.position.x;

            if (playerIsLeft && facingRight)
                Flip();
            else if (!playerIsLeft && !facingRight)
                Flip();
        }
    }

    void Flip()
    {
        facingRight = !facingRight;

        //Flip only the visual hierarchy
        if (visualRoot != null)
        {
            Vector3 scale = visualRoot.localScale;
            scale.x *= -1;
            visualRoot.localScale = scale;
        }

        //Mirror IK targets across the boss's center X
        foreach (var t in ikTargets)
        {
            if (t == null) continue;
            Vector3 worldPos = t.position;
            worldPos.x = transform.position.x - (worldPos.x - transform.position.x);
            t.position = worldPos;
        }

        Debug.Log("Boss flipped safely. Facing right: " + facingRight);
    }
}