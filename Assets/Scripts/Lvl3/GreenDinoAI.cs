using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenDinoAI : MonoBehaviour
{
    Rigidbody2D rb;

    public float moveSpeed;
    public Transform castPos;
    public float baseCastDistance;
    public LayerMask platformLayer;
    public LayerMask playerLayer;

    private bool facingRight;
    private Vector3 baseScale;
    private Animator animator;
    private bool onAttackMode;

    private void Start()
    {
        facingRight = true;
        baseScale = transform.localScale;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        onAttackMode = false;
    }
    private void FixedUpdate()
    {
        float velocityX = moveSpeed;

        if (!facingRight)
        {
            velocityX = -moveSpeed;
        }

        rb.velocity = new Vector2(velocityX, rb.velocity.y);

        if (isMeetPlatformEdge())
        {
            FlipEnemy();
        }

        if (IsInRange() && !onAttackMode)
        {
            AttackPlayer();
        }
    }

    bool isMeetPlatformEdge()
    {
        bool condition = true;

        float castDist = baseCastDistance;

        // set the target destination based on cast distance
        Vector3 targetPos = castPos.position;
        targetPos.y -= castDist;

        Debug.DrawLine(castPos.position, targetPos, Color.blue);

        if (Physics2D.Linecast(castPos.position, targetPos, platformLayer))
        {
            condition = false;
        }
        else
        {
            condition = true;
        }

        return condition;
    }

    void FlipEnemy()
    {
        Vector3 newScale = baseScale;

        if (!facingRight)
        {
            newScale.x = baseScale.x;
            facingRight = true;
        }
        else
        {
            newScale.x = -baseScale.x;
            facingRight = false;
        }

        transform.localScale = newScale;
    }

    bool IsInRange()
    {
        bool condition = false;


        float castDist = baseCastDistance * 3;

        if (!facingRight)
        {
            castDist = -baseCastDistance * 3;
        }

        // set the target destination based on cast distance
        Vector3 targetPos = castPos.position;
        targetPos.x += castDist;

        Debug.DrawLine(castPos.position, targetPos, Color.red);

        if (Physics2D.Linecast(castPos.position, targetPos, playerLayer))
        {
            condition = true;
        }
        else
        {
            condition = false;
        }

        return condition;
    }

    void AttackPlayer()
    {
        onAttackMode = true;

        animator.SetBool("Attack", true);
        Invoke("StopAttackingPlayer", 0.5f);
    }

    void StopAttackingPlayer()
    {
        onAttackMode = false;
        animator.SetBool("Attack", false);
    }
}
