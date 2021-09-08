using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BabyDinoAI : MonoBehaviour
{
    public Transform rayCast;
    public LayerMask rcMask;
    public float rcLength;
    public float attackDistance;    // min distance for attack
    public float moveSpeed;
    public float timer;             // attack cooldown timer

    private RaycastHit2D hit;
    private GameObject target;
    private Animator animator;
    private float distance;     // distance between player and enemy
    private bool onAttackMode;
    private bool inRange;       // check if player is in range
    private bool onCooldown;     // check if enemy attack is on cooldown
    private float initialTimer;

    // Start is called before the first frame update
    void Awake()
    {
        initialTimer = timer;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (inRange)
        {
            hit = Physics2D.Raycast(rayCast.position, Vector2.left, rcLength, rcMask);
            RayCastDebugger();
        }

        // when player is detected
        if (hit.collider != null)
        {
            EnemyLogic();
        }
        else if (hit.collider == null)
        {
            inRange = false;
        }

        if (!inRange)
        {
            animator.SetBool("Run", false);
            StopAttackPlayer();
        }
    }

    void EnemyLogic()
    {
        distance = Vector2.Distance(transform.position, target.transform.position);

        if (distance > attackDistance)
        {
            MoveToPlayer();
            StopAttackPlayer();
        }
        else if (distance <= attackDistance && onCooldown == false)
        {
            AttackPlayer();
        }

        if (onCooldown)
        {
            Cooldown();
            animator.SetBool("Attack", false);
        }
    }

    void MoveToPlayer()
    {
        animator.SetBool("Run", true);

        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            Vector2 targetPos = new Vector2(target.transform.position.x, transform.position.y);
            transform.position = Vector2.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
        }
    }

    void AttackPlayer()
    {
        timer = initialTimer;
        onAttackMode = true;

        animator.SetBool("Run", false);
        animator.SetBool("Attack", true);
    }

    void StopAttackPlayer()
    {
        onCooldown = false;
        onAttackMode = false;

        animator.SetBool("Attack", false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            target = collision.gameObject;
            inRange = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            target = collision.gameObject;
            inRange = true;
        }
    }

    void RayCastDebugger()
    {
        if (distance > attackDistance)
        {
            Debug.DrawRay(rayCast.position, Vector2.left * rcLength, Color.red);
        }
        else if (distance < attackDistance)
        {
            Debug.DrawRay(rayCast.position, Vector2.left * rcLength, Color.green);
        }
    }

    void Cooldown()
    {
        timer -= Time.deltaTime;

        if (timer <= 0 && onCooldown && onAttackMode)
        {
            onCooldown = false;
            timer = initialTimer;
        }
    }

    public void TriggerCooldown()
    {
        onCooldown = true;
    }
}
