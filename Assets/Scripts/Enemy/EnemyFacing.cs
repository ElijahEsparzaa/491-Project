using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFacing : MonoBehaviour
{
    private Transform player;
    private bool facingRight = true;
    private EnemyStats stats;

    void Start()
    {
        player = FindObjectOfType<PlayerStats>().transform;
        stats = GetComponent<EnemyStats>();
    }

    void Update()
    {
        if (player == null || stats == null) return;

        //Flip for both Boss and Skeleton enemies
        string nameCheck = stats.enemyData != null ? stats.enemyData.name.ToLower() : "";

        if (nameCheck.Contains("boss") || nameCheck.Contains("skeleton"))
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

        //Flip entire body horizontally
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}