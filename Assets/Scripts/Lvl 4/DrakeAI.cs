using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrakeAI : MonoBehaviour
{
    public Transform rayCast;
    public float baseDetectDistance;
    public LayerMask playerLayer;
    public float maxTeleportTimes;
    public Transform teleportPos1, teleportPos2;
    public float teleportDistance;
    public Transform target;
    public float moveSpeed;
    public float baseAttackDistance;
    public float timer;             // attack cooldown timer

    private float currentTeleportTimes;
    private Animator animator;
    private bool facingRight;
    private bool onCooldown;
    private float initialTimer;

    // Start is called before the first frame update
    void Start()
    {
        currentTeleportTimes = 0;
        animator = GetComponent<Animator>();
        facingRight = false;
        initialTimer = timer;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (IsDetectPlayer() && !onCooldown)
        {
            ChasePlayer();
        }
        else
        {
            StopChasingPlayer();
        }

        if (IsInAttackRange() && !onCooldown)
        {
            if (currentTeleportTimes < maxTeleportTimes)
            {
                StopChasingPlayer();
                Teleport();
                currentTeleportTimes += 1;
            }
            else
            {
                AttackPlayer();
            }
        }

        if (onCooldown)
        {
            Cooldown();
            StopAttackingPlayer();
        }
    }

    bool IsDetectPlayer()
    {
        bool condition = false;

        float detectDistance = baseDetectDistance;

        Vector3 targetLeftPos = rayCast.position;
        targetLeftPos.x -= detectDistance;
        Vector3 targetRightPos = rayCast.position;
        targetRightPos.x += detectDistance;

        Debug.DrawLine(rayCast.position, targetLeftPos, Color.red);
        Debug.DrawLine(rayCast.position, targetRightPos, Color.red);

        if (Physics2D.Linecast(rayCast.position, targetLeftPos, playerLayer) ||
            Physics2D.Linecast(rayCast.position, targetRightPos, playerLayer))
        {
            condition = true;
        }
        else
        {
            condition = false;
        }

        if (Physics2D.Linecast(rayCast.position, targetLeftPos, playerLayer) && facingRight) 
        {
            FlipEnemy();
            facingRight = false;
        }

        if (Physics2D.Linecast(rayCast.position, targetRightPos, playerLayer) && !facingRight)
        {
            FlipEnemy();
            facingRight = true;
        }

        return condition;
    }

    bool IsInAttackRange()
    {
        bool condition = false;

        float attackDistance = baseAttackDistance;

        if (!facingRight)
        {
            attackDistance = -baseAttackDistance;
        }

        Vector3 targetPos = rayCast.position;
        targetPos.x += attackDistance;

        Debug.DrawLine(rayCast.position, targetPos, Color.blue);

        if (Physics2D.Linecast(rayCast.position, targetPos, playerLayer))
        {
            condition = true;
        }
        else
        {
            condition = false;
        }

        return condition;
    }

    void Teleport()
    {
        int rand = Random.Range(1, 3);

        if (rand == 1)
        {
            transform.position = teleportPos1.position;
        }
        else if (rand == 2)
        {
            transform.position = teleportPos2.position;
        }
    }

    void ChasePlayer()
    {
        animator.SetBool("Run", true);

        Vector2 targetPos = new Vector2(target.position.x, transform.position.y);
        transform.position = Vector2.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
    }

    void StopChasingPlayer()
    {
        animator.SetBool("Run", false);
    }

    void AttackPlayer()
    {
        animator.SetBool("Attack", true);

        Invoke("StopAttackingPlayer", 0.5f);
    }

    void StopAttackingPlayer()
    {
        animator.SetBool("Attack", false);
    }

    void FlipEnemy()
    {
        transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
    }

    void Cooldown()
    {
        timer -= Time.deltaTime;

        if (timer <= 0 && onCooldown)
        {
            onCooldown = false;
            timer = initialTimer;
        }
    }

    void TriggerCooldown()
    {
        onCooldown = true;
    }
}
