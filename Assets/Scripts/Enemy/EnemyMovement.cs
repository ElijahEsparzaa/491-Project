using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    EnemyStats enemy;
    Transform player;

    //New variables
    private Animator anim;
    private Rigidbody2D rb;
    private bool isBoss = false;

    void Start()
    {
        enemy = GetComponent<EnemyStats>();
        player = FindAnyObjectByType<PlayerMovement>().transform;

        //Find Animator (in children — e.g., under "Visual")
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();

        //Detect if this enemy is a Boss
        if (enemy != null && enemy.enemyData != null && enemy.enemyData.name.Contains("Boss"))
            isBoss = true;
    }

    void Update()
    {
        //Movement towards player
        transform.position = Vector2.MoveTowards(
            transform.position,
            player.position,
            enemy.currentMoveSpeed * Time.deltaTime
        );

        //Animate only if Boss
        if (isBoss && anim != null)
        {
            float distance = Vector2.Distance(transform.position, player.position);

            //Rough check for movement direction
            if (enemy.currentMoveSpeed > 0.1f && distance > 0.1f)
                anim.SetInteger("State", 1); //Walk
            else
                anim.SetInteger("State", 0); //Idle
        }
    }
}