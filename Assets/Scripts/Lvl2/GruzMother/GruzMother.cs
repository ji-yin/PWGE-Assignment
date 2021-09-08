<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD
﻿using System.Collections;
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
    [SerializeField] Transform groundCheckUp;
    [SerializeField] Transform groundCheckDown;
    [SerializeField] Transform groundCheckWall;
    [SerializeField] float groundCheckRadius;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] public int playerDamage;
    [SerializeField] float attackRange;
    [SerializeField] LayerMask playerLayers;

    private bool isTouchingUp;
    private bool isTouchingDown;
    private bool isTouchingWall;
    private bool isTouchingPlayer;
    private bool hasPlayerPositon;

    private Vector2 playerPosition;

    private bool facingLeft = true;
    private bool goingUp = true;
    private Rigidbody2D enemyRB;
    private Animator enemyAnim;


    void Start()
    {
        idelMovementDirection.Normalize();
        attackMovementDirection.Normalize();
        enemyRB = GetComponent<Rigidbody2D>();
        enemyAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        isTouchingUp = Physics2D.OverlapCircle(groundCheckUp.position, groundCheckRadius, groundLayer); 
        isTouchingDown = Physics2D.OverlapCircle(groundCheckDown.position, groundCheckRadius, groundLayer); 
        isTouchingWall = Physics2D.OverlapCircle(groundCheckWall.position, groundCheckRadius, groundLayer);
        isTouchingPlayer = Physics2D.OverlapCircle(groundCheckDown.position, attackRange, playerLayers);
        if (isTouchingPlayer)
        {
            Attack();
        }
    }

=======
=======
>>>>>>> parent of b93abbce (lvl3)
=======
>>>>>>> parent of b93abbce (lvl3)
﻿using System.Collections;
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
    [SerializeField] Transform groundCheckUp;
    [SerializeField] Transform groundCheckDown;
    [SerializeField] Transform groundCheckWall;
    [SerializeField] float groundCheckRadius;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] public int playerDamage;
    [SerializeField] float attackRange;
    [SerializeField] LayerMask playerLayers;

    private bool isTouchingUp;
    private bool isTouchingDown;
    private bool isTouchingWall;
    private bool isTouchingPlayer;
    private bool hasPlayerPositon;

    private Vector2 playerPosition;

    private bool facingLeft = true;
    private bool goingUp = true;
    private Rigidbody2D enemyRB;
    private Animator enemyAnim;


    void Start()
    {
        idelMovementDirection.Normalize();
        attackMovementDirection.Normalize();
        enemyRB = GetComponent<Rigidbody2D>();
        enemyAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        isTouchingUp = Physics2D.OverlapCircle(groundCheckUp.position, groundCheckRadius, groundLayer); 
        isTouchingDown = Physics2D.OverlapCircle(groundCheckDown.position, groundCheckRadius, groundLayer); 
        isTouchingWall = Physics2D.OverlapCircle(groundCheckWall.position, groundCheckRadius, groundLayer);
        isTouchingPlayer = Physics2D.OverlapCircle(groundCheckDown.position, attackRange, playerLayers);
        if (isTouchingPlayer)
        {
            Attack();
        }
    }

<<<<<<< HEAD
<<<<<<< HEAD
>>>>>>> parent of b93abbce (lvl3)
=======
>>>>>>> parent of b93abbce (lvl3)
=======
>>>>>>> parent of b93abbce (lvl3)
    void Attack()
    {
        Collider2D[] player = Physics2D.OverlapCircleAll(groundCheckDown.transform.position, attackRange, playerLayers);

        foreach (Collider2D hitPlayer in player)
        {
            if (!hitPlayer.GetComponent<Animator>().GetBool("isDead"))
            {
                hitPlayer.GetComponent<PlayerControllerDemo>().DamagePlayer(playerDamage);
            }

        }
<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD
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
        if (isTouchingUp && goingUp)
        {
            ChangeDirection();
        }
        else if (isTouchingDown && !goingUp)
        {
            ChangeDirection();
        }

        if (isTouchingWall)
        {
            if (facingLeft)
            {
                Flip();
            }
            else if (!facingLeft)
            {
                Flip();
            }
        }
        enemyRB.velocity = idelMovementSpeed * idelMovementDirection;
    }

   public void AttackUpNDownState()
    {
        if (isTouchingUp && goingUp)
        {
            ChangeDirection();
        }
        else if (isTouchingDown && !goingUp)
        {
            ChangeDirection();
        }

        if (isTouchingWall)
        {
            if (facingLeft)
            {
                Flip();
            }
            else if (!facingLeft)
            {
                Flip();
            }
        }
        enemyRB.velocity = attackMovementSpeed * attackMovementDirection;
    }

    public void AttackPlayerState()
    {
       
        if (!hasPlayerPositon)
        {
            FlipTowardsPlayer();
             playerPosition = player.position - transform.position;
            playerPosition.Normalize();
            hasPlayerPositon = true;
        }
        if (hasPlayerPositon)
        {
            enemyRB.velocity = attackPlayerSpeed * playerPosition;
           
        }

        if (isTouchingWall || isTouchingDown||isTouchingPlayer)
        {
            //play Slam animation
            enemyAnim.SetTrigger("Slamed");
            enemyRB.velocity = Vector2.zero;
            hasPlayerPositon = false;
        }
    }

    void FlipTowardsPlayer()
    {
        float playerDirection = player.position.x - transform.position.x;

        if (playerDirection>0 && facingLeft)
        {
            Flip();
        }
        else if (playerDirection<0 && !facingLeft)
        {
            Flip();
        }
    }

    void ChangeDirection()
    {
        goingUp = !goingUp;
        idelMovementDirection.y *= -1;
        attackMovementDirection.y *= -1;
    }

    void Flip()
    {
        facingLeft = !facingLeft;
        idelMovementDirection.x *= -1;
        attackMovementDirection.x *= -1;
        transform.Rotate(0, 180, 0);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(groundCheckUp.position, groundCheckRadius);
        Gizmos.DrawWireSphere(groundCheckDown.position, groundCheckRadius);
        Gizmos.DrawWireSphere(groundCheckWall.position, groundCheckRadius);
    }
}
=======
=======
>>>>>>> parent of b93abbce (lvl3)
=======
>>>>>>> parent of b93abbce (lvl3)
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
        if (isTouchingUp && goingUp)
        {
            ChangeDirection();
        }
        else if (isTouchingDown && !goingUp)
        {
            ChangeDirection();
        }

        if (isTouchingWall)
        {
            if (facingLeft)
            {
                Flip();
            }
            else if (!facingLeft)
            {
                Flip();
            }
        }
        enemyRB.velocity = idelMovementSpeed * idelMovementDirection;
    }

   public void AttackUpNDownState()
    {
        if (isTouchingUp && goingUp)
        {
            ChangeDirection();
        }
        else if (isTouchingDown && !goingUp)
        {
            ChangeDirection();
        }

        if (isTouchingWall)
        {
            if (facingLeft)
            {
                Flip();
            }
            else if (!facingLeft)
            {
                Flip();
            }
        }
        enemyRB.velocity = attackMovementSpeed * attackMovementDirection;
    }

    public void AttackPlayerState()
    {
       
        if (!hasPlayerPositon)
        {
            FlipTowardsPlayer();
             playerPosition = player.position - transform.position;
            playerPosition.Normalize();
            hasPlayerPositon = true;
        }
        if (hasPlayerPositon)
        {
            enemyRB.velocity = attackPlayerSpeed * playerPosition;
           
        }

        if (isTouchingWall || isTouchingDown||isTouchingPlayer)
        {
            //play Slam animation
            enemyAnim.SetTrigger("Slamed");
            enemyRB.velocity = Vector2.zero;
            hasPlayerPositon = false;
        }
    }

    void FlipTowardsPlayer()
    {
        float playerDirection = player.position.x - transform.position.x;

        if (playerDirection>0 && facingLeft)
        {
            Flip();
        }
        else if (playerDirection<0 && !facingLeft)
        {
            Flip();
        }
    }

    void ChangeDirection()
    {
        goingUp = !goingUp;
        idelMovementDirection.y *= -1;
        attackMovementDirection.y *= -1;
    }

    void Flip()
    {
        facingLeft = !facingLeft;
        idelMovementDirection.x *= -1;
        attackMovementDirection.x *= -1;
        transform.Rotate(0, 180, 0);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(groundCheckUp.position, groundCheckRadius);
        Gizmos.DrawWireSphere(groundCheckDown.position, groundCheckRadius);
        Gizmos.DrawWireSphere(groundCheckWall.position, groundCheckRadius);
    }
}
<<<<<<< HEAD
<<<<<<< HEAD
>>>>>>> parent of b93abbce (lvl3)
=======
>>>>>>> parent of b93abbce (lvl3)
=======
>>>>>>> parent of b93abbce (lvl3)
