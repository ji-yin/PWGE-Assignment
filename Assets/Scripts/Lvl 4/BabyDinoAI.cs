using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BabyDinoAI : MonoBehaviour
{
    public float moveSpeed;
    public float jumpForce;
    public Transform target;
    public LayerMask playerLayer;
    public float baseDetectDistance;
    public float baseAttackDistance;
    public Transform rayCast;

    private bool facingRight;
    private bool isGrounded;
    private Animator animator;
    private Rigidbody2D rb;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        facingRight = false;
        rb = GetComponent<Rigidbody2D>();
        isGrounded = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (IsDetectPlayer())
        {
            ChasePlayer();
        }
        else
        {
            StopChasingPlayer();
        }

        if (IsInAttackRange() && isGrounded)
        {
            StopChasingPlayer();
            JumpAttack();
        }
        else
        {
            StopJumpAttack();
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

    void JumpAttack()
    {
        float distanceFromPlayer = target.position.x - transform.position.x;
        rb.velocity = new Vector2(distanceFromPlayer, jumpForce);
        isGrounded = false;
    }

    void StopJumpAttack()
    {
        isGrounded = true;
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

    void FlipEnemy()
    {
        transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
    }
}
