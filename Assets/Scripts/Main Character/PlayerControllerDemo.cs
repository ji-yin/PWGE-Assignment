using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerControllerDemo : MonoBehaviour
{
    //Start() variables
    private Rigidbody2D rb;
    private Animator anim;
    private Collider2D coll;

    //FSM
    private enum State {idle, running, jumping, falling, hurt, death, attack, climb}
    private State state = State.idle;

    //Ladder variables
    [HideInInspector] public bool canClimb = false;
    [HideInInspector] public bool bottomLadder = false;
    [HideInInspector] public bool topLadder = false;
    public Ladder ladder;
    private float naturalGravity;
    [SerializeField] float climbSpeed = 3f;

    //Inspector varibles
    [SerializeField] private LayerMask ground;
    [SerializeField] private float speed = 0.5f;
    [SerializeField] private float jumpForce = 20f;
    [SerializeField] private int gems = 0;
    [SerializeField] private TextMeshProUGUI gemText;
    [SerializeField] private float hurtForce = 5f;
    [SerializeField] private AudioSource gem;
    [SerializeField] private AudioSource footstep;
    [SerializeField] private AudioSource hurt;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform AttackPoint;
    [SerializeField] private LayerMask enemyLayers;
    [SerializeField] private float attackRange = 0.5f;
    [SerializeField] private float attackRate = 2f;
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private Text healthAmount;
    int currentHealth;
    private float nextAttackTime = 0f;


    // Start is called before the first frame update
    private void Start()
    { 
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
        currentHealth = maxHealth;
        healthAmount.text = currentHealth.ToString();
        naturalGravity = rb.gravityScale;
    }

    // Update is called once per frame
    private void Update()
    {
        if (gems >= 10 && currentHealth <=90)
        {
            currentHealth += 10;
            gems -= 10; 
        }

        if (state == State.climb)
        {
            Climb();
        }

        else if (state != State.hurt)
        {
            Movement();
        }
        AnimationState();
        anim.SetInteger("state", (int)state);//sets animation based on Enumerator state

        if (Time.time >= nextAttackTime)
        {
            if (Input.GetKeyDown(KeyCode.H))
            {
                Attack();
                nextAttackTime = Time.time + 1f / attackRate;
            }


        }

        void Attack()
        {
            animator.SetTrigger("Attack");

            //Detect enemies in range of attack
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(AttackPoint.position, attackRange, enemyLayers);

            foreach (Collider2D enemy in hitEnemies)
            {
                enemy.GetComponentInParent<Enemy_behaviour>().TakeDamage();
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Collectable")
        {
            gem.Play();
            Destroy(collision.gameObject);
            gems += 1;
            gemText.text = gems.ToString();
        }

        if(collision.tag == "PowerUp")
        {
            Destroy(collision.gameObject);
            jumpForce = 90f;
            GetComponent<SpriteRenderer>().color = Color.yellow;
            StartCoroutine(ResetPower());
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();

            if (state == State.falling)
            {
                enemy.JumpedOn();
                Jump();
            }
            else
            {
                hurt.Play();
                state = State.hurt;
                HandleHealth();//Deals with health, updating ui, will reset level if health is <=0

                if (other.gameObject.transform.position.x > transform.position.x)
                {
                    //Enemy is to my right therefore i should damaged and move left
                    rb.velocity = new Vector2(-hurtForce, rb.velocity.y);
                }
                else
                {
                    //Enemy is to my left therefore i should damaged and move right
                    rb.velocity = new Vector2(hurtForce, rb.velocity.y);
                }

            }
        }


    }

    private void HandleHealth()
    {
        currentHealth -= 1;
        healthAmount.text = currentHealth.ToString();
        if (currentHealth <= 0)
        { 
            PlayerDie();
        }
    }

    private void Movement()
    {
        float hDirection = Input.GetAxis("Horizontal");

        if(canClimb && Mathf.Abs(Input.GetAxis("Vertical")) > .1f)
        {
            state = State.climb;
            rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
            transform.position = new Vector3(ladder.transform.position.x, rb.position.y);
            rb.gravityScale = 0f;
        }
        //Moving left
        if (hDirection < 0)
        {
            rb.velocity = new Vector2(-speed, rb.velocity.y);
            transform.localScale = new Vector2(-1, 1);

        }
        //Moving Right
        else if (hDirection > 0)
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
            transform.localScale = new Vector2(1, 1);

        }
        //Jumping
        if (Input.GetKeyDown(KeyCode.Space) && coll.IsTouchingLayers(ground))
        {
            Jump();
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        state = State.jumping;
    }
    private void AnimationState()
    {
        if(state == State.climb)
        {

        }

        else if(state == State.jumping)
        {
            if(rb.velocity.y < 0.1f)
            {
                state = State.falling;
            }
        }

        else if(state == State.falling)
        {
            if (coll.IsTouchingLayers(ground))
            {
                state = State.idle;
            }
        }

        else if(state == State.hurt)
        {
            if(Mathf.Abs(rb.velocity.x) < 0.1f)
            {
                state = State.idle;
            }
        }

        else if (Mathf.Abs(rb.velocity.x) > 2f)
        {
            //Moving
            state = State.running;
        }

        else
        {
            state = State.idle;
        }

   
    }

    private void FootStep()
    {
        footstep.Play();
    }

    private IEnumerator ResetPower()
    {
        yield return new WaitForSeconds(5);
        jumpForce = 60;
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    private void OnDrawGizmosSelected()
    {
        while (AttackPoint == null)
        {
            return;
        }
        Gizmos.DrawWireSphere(AttackPoint.position, attackRange);
    }

    public void DamagePlayer(int damage)
    {
        currentHealth -= damage;
        healthAmount.text = currentHealth.ToString();
        animator.SetInteger("state", 4);
        if (currentHealth <= 0)
        {
            PlayerDie();
        }
    }

    void PlayerDie()
    {
        Debug.Log("Player Died");
        animator.SetBool("isDead", true);

        //GetComponent<Collider2D>().enabled = false;
        Destroy(this);
    }

    private void Climb()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            canClimb = false;
            rb.gravityScale = naturalGravity;
            anim.speed = 1f;
            Jump();
            return;
        }

        float vDirection = Input.GetAxis("Vertical");
        //Climbing up
        if (vDirection> .1f && !topLadder)
        {
            rb.velocity = new Vector2(0f, vDirection * climbSpeed);
            anim.speed = 1f;
        }
        //Climbing down
        else if (vDirection < -.1f && !bottomLadder)
        {
            rb.velocity = new Vector2(0f, vDirection * climbSpeed);
            anim.speed = 1f;
        }
        //Still
        else
        {
            anim.speed = 0f;
            rb.velocity = Vector2.zero;
        }
    }
}
