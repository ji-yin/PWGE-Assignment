using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class GruzMother : MonoBehaviour
{
    [Header("Idel")]
    [SerializeField] float idelMovementSpeed;
    [SerializeField] Vector2 idelMovementDirection;

    [Header("AttackUpNDown")]
    [SerializeField] float attackMovementSpeed;
    [SerializeField] Vector2 attackMovementDirection;

    [Header("AttackPlayer")]
    [SerializeField] float attackPlayerSpeed;
    [SerializeField] Transform player;

    [Header("Other")]
    [NamedArrayAttribute(new string[] { "groundCheckUp", "groundCheckDown" ,"groundCheckWall"})]
    [SerializeField] private Transform[] groundCheck;
    [NamedArrayAttribute(new string[] { "GroundLayer", "PlayerLayer" })]
    [SerializeField] private LayerMask[] layers;
    [SerializeField] float groundCheckRadius;
    [SerializeField] int playerDamage;
    [SerializeField] float attackRange;

    private Hashtable check = new Hashtable();
    private Vector2 playerPosition;
    private Rigidbody2D enemyRB;
    private Animator enemyAnim;


    void Start()
    {
        idelMovementDirection.Normalize();
        attackMovementDirection.Normalize();
        enemyRB = GetComponent<Rigidbody2D>();
        enemyAnim = GetComponent<Animator>();
        check.Add("isTouchingUp", false);
        check.Add("isTouchingDown", false);
        check.Add("isTouchingPlayer", false);
        check.Add("hasPlayerPosition", false);
        check.Add("facingLeft", true);
        check.Add("goingUp", true);

    }

    // Update is called once per frame
    void Update()
    {
        check["isTouchingUp"] = (bool)Physics2D.OverlapCircle(groundCheck[0].position, groundCheckRadius, layers[0]);
        check["isTouchingDown"] = (bool)Physics2D.OverlapCircle(groundCheck[1].position, groundCheckRadius, layers[0]);
        check["isTouchingWall"] = (bool)Physics2D.OverlapCircle(groundCheck[2].position, groundCheckRadius, layers[0]);
        check["isTouchingPlayer"] = (bool)Physics2D.OverlapCircle(groundCheck[1].position, attackRange, layers[1]);
        if ((bool)check["isTouchingPlayer"])
        {
            Attack();
        }
    }

    void Attack()
    {
        Collider2D[] player = Physics2D.OverlapCircleAll(groundCheck[1].transform.position, attackRange, layers[1]);

        foreach (Collider2D hitPlayer in player)
        {
            if (!hitPlayer.GetComponent<Animator>().GetBool("isDead"))
            {
                hitPlayer.GetComponent<PlayerControllerDemo>().DamagePlayer(playerDamage);
            }

        }
    }


    void RandomStatePicker()
    {
        int randomState = Random.Range(0, 2);
        if (randomState == 0)
        {
            enemyAnim.SetTrigger("AttackUpNDown");
        }
        else if (randomState == 1)
        {
            enemyAnim.SetTrigger("AttackPlayer");
        }
    }

   public void IdelState()
    {
        if ((bool)check["isTouchingUp"] && (bool)check["goingUp"])
        {
            ChangeDirection();
        }
        else if ((bool)check["isTouchingDown"] && !(bool)check["goingUp"])
        {
            ChangeDirection();
        }

        if ((bool)check["isTouchingWall"])
        {
            if ((bool)check["facingLeft"])
            {
                Flip();
            }
            else if (!(bool)check["facingLeft"])
            {
                Flip();
            }
        }
        enemyRB.velocity = idelMovementSpeed * idelMovementDirection;
    }

   public void AttackUpNDownState()
    {
        if ((bool)check["isTouchingUp"] && (bool)check["goingUp"])
        {
            ChangeDirection();
        }
        else if ((bool)check["isTouchingDown"] && !(bool)check["goingUp"])
        {
            ChangeDirection();
        }

        if ((bool)check["isTouchingWall"])
        {
            if ((bool)check["facingLeft"])
            {
                Flip();
            }
            else if (!(bool)check["facingLeft"])
            {
                Flip();
            }
        }
        enemyRB.velocity = attackMovementSpeed * attackMovementDirection;
    }

    public void AttackPlayerState()
    {
       
        if (!(bool)check["hasPlayerPosition"])
        {
            FlipTowardsPlayer();
             playerPosition = player.position - transform.position;
            playerPosition.Normalize();
            check["hasPlayerPosition"] = (bool)true;
        }
        if ((bool)check["hasPlayerPosition"])
        {
            enemyRB.velocity = attackPlayerSpeed * playerPosition;
           
        }

        if ((bool)check["isTouchingWall"] || (bool)check["isTouchingDown"]||(bool)check["isTouchingPlayer"])
        {
            //play Slam animation
            enemyAnim.SetTrigger("Slamed");
            enemyRB.velocity = Vector2.zero;
            check["hasPlayerPosition"] = (bool)false;
        }
    }

    void FlipTowardsPlayer()
    {
        float playerDirection = player.position.x - transform.position.x;

        if (playerDirection>0 && (bool)check["facingLeft"])
        {
            Flip();
        }
        else if (playerDirection<0 && !(bool)check["facingLeft"])
        {
            Flip();
        }
    }

    void ChangeDirection()
    {
        check["goingUp"] = !(bool)check["goingUp"];
        idelMovementDirection.y *= -1;
        attackMovementDirection.y *= -1;
    }

    void Flip()
    {
        check["facingLeft"] = !(bool)check["facingLeft"];
        idelMovementDirection.x *= -1;
        attackMovementDirection.x *= -1;
        transform.Rotate(0, 180, 0);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(groundCheck[0].position, groundCheckRadius);
        Gizmos.DrawWireSphere(groundCheck[1].position, groundCheckRadius);
        Gizmos.DrawWireSphere(groundCheck[2].position, groundCheckRadius);
    }
}
