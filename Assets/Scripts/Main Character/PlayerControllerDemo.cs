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
    private bool isGrounded;

    //FSM
    private enum State {idle, running, jumping, falling, hurt, death, attack, climb}
    private State state = State.idle;

    [Header("Ladder")]
    [HideInInspector] public bool canClimb = false;
    [HideInInspector] public bool bottomLadder = false;
    [HideInInspector] public bool topLadder = false;
    public Ladder ladder;
    private float naturalGravity;

    [Header("InspectorVar")]
    [NamedArrayAttribute(new string[] { "Speed", "CimbSpeed", "JumpForce", "HurtForce","AttackRange","AttackRate", "NextAttackTime" })]
    [SerializeField] private float[] playerVar;
    [NamedArrayAttribute(new string[] { "GroundLayer", "EnemyLayer"})]
    [SerializeField] private LayerMask[] layers;
    [NamedArrayAttribute(new string[] { "Gem", "Footstep", "Hurt" ,"Dead"})]
    [SerializeField] private AudioSource[] playerAudio;
    [NamedArrayAttribute(new string[] { "AttackPoint", "GroundDetection", "GroundDetectionL", "GroundDetectionR"})]
    [SerializeField] private Transform[] playerDetect;
    [NamedArrayAttribute(new string[] { "Gems", "MaxHealth", "CurrentHealth"})]
    [SerializeField] private int[] playerPoints;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI gemText;
    [SerializeField] private Text healthAmount;
    public GameOverMenu gameOverMenu;

    

    // Start is called before the first frame update
    private void Start()
    {
        Time.timeScale = 1f;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        playerPoints[2] = playerPoints[1];
        healthAmount.text = playerPoints[2].ToString();
        naturalGravity = rb.gravityScale;
        isGrounded = true;
    }

    // Update is called once per frame
    private void Update()
    {
        if (playerPoints[0] >= 10 && playerPoints[2] <= 90)
        {
            playerPoints[2] += 10;
            playerPoints[0] -= 10; 
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

        if (Time.time >= playerVar[6])
        {
            if (Input.GetKeyDown(KeyCode.H))
            {
                Attack();
                playerVar[6] = Time.time + 1f / playerVar[5];
            }


        }

        void Attack()
        {
            anim.SetTrigger("Attack");

            //Detect enemies in range of attack
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(playerDetect[0].position, playerVar[4], layers[1]);

            foreach (Collider2D enemy in hitEnemies)
            {
                enemy.GetComponentInParent<Enemy_behaviour>().TakeDamage();
            }
        }

    }

    private void FixedUpdate()
    {
        if (Physics2D.Linecast(transform.position, playerDetect[1].position, layers[0]) ||
            Physics2D.Linecast(transform.position, playerDetect[2].position, layers[0]) ||
            Physics2D.Linecast(transform.position, playerDetect[3].position, layers[0]))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Collectable")
        {
            playerAudio[0].Play();
            Destroy(collision.gameObject);
            playerPoints[0] += 1;
            gemText.text = playerPoints[0].ToString();
        }

        if(collision.tag == "PowerUp")
        {
            Destroy(collision.gameObject);
            playerVar[2] = 120f;
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
                playerAudio[2].Play();
                state = State.hurt;
                HandleHealth();//Deals with health, updating ui, will reset level if health is <=0

                if (other.gameObject.transform.position.x > transform.position.x)
                {
                    //Enemy is to my right therefore i should damaged and move left
                    rb.velocity = new Vector2(-playerVar[3], rb.velocity.y);
                }
                else
                {
                    //Enemy is to my left therefore i should damaged and move right
                    rb.velocity = new Vector2(playerVar[3], rb.velocity.y);
                }
            }
        }
    }

    private void HandleHealth()
    {
        playerPoints[2] -= 1;
        healthAmount.text = playerPoints[2].ToString();
        if (playerPoints[2] <= 0)
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
            rb.velocity = new Vector2(-playerVar[0], rb.velocity.y);
            transform.localScale = new Vector2(-1, 1);
        }
        //Moving Right
        else if (hDirection > 0)
        {
            rb.velocity = new Vector2(playerVar[0], rb.velocity.y);
            transform.localScale = new Vector2(1, 1);
        }
        //Jumping
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, playerVar[2]);
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
            if (isGrounded)
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

        else if (Mathf.Abs(rb.velocity.x) > 2f && isGrounded)
        {
            //Moving
            state = State.running;
        }

        else
        {
            if (isGrounded)
            {
                state = State.idle;
            }
            else
            {
                state = State.falling;
            }
            
        }

    }

    private void FootStep()
    {
        playerAudio[1].Play();
    }

    private IEnumerator ResetPower()
    {
        yield return new WaitForSeconds(5);
        playerVar[2] = 60;
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    private void OnDrawGizmosSelected()
    {
        while (playerDetect[0] == null)
        {
            return;
        }
        Gizmos.DrawWireSphere(playerDetect[0].position, playerVar[4]);
    }

    public void DamagePlayer(int damage)
    {
        playerPoints[2] -= damage;
        healthAmount.text = playerPoints[2].ToString();
        anim.SetInteger("state", 4);
        if (playerPoints[2] <= 0)
        {
            PlayerDie();
        }
    }

    void PlayerDie()
    {
        Debug.Log("Player Died");
        anim.SetBool("isDead", true);
        GetComponent<Collider2D>().enabled = false;
        string gemsText = playerPoints[0].ToString();
        gameOverMenu.Setup(gemsText);
        Time.timeScale = 0f;
        gameOverMenu.GameOver();
        Destroy(gameObject);
    }

    private void Climb()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            canClimb = false;
            rb.gravityScale = naturalGravity;
            anim.speed = 1f;
            return;
        }

        float vDirection = Input.GetAxis("Vertical");
        //Climbing up
        if (vDirection> .1f && !topLadder)
        {
            rb.velocity = new Vector2(0f, vDirection * playerVar[1]);
            anim.speed = 1f;
        }
        //Climbing down
        else if (vDirection < -.1f && !bottomLadder)
        {
            rb.velocity = new Vector2(0f, vDirection * playerVar[1]);
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
