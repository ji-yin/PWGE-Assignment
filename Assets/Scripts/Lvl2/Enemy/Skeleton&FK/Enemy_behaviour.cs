using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_behaviour : MonoBehaviour
{
    [Header("InspectorVar")]
    //AttackDistance = Minimum distance for attack
    //Timer = Timer for cooldown between attacks
    [NamedArrayAttribute(new string[] { "attackDistance", "moveSpeed", "timer", "attackRange"})]
    [SerializeField] protected float[] enemyVar;
    [NamedArrayAttribute(new string[] { "leftLimit", "rightLimit"})]
    [SerializeField] private Transform[] enemyLimit;
    [NamedArrayAttribute(new string[] { "enemyDamage", "playerDamage" })]
    [SerializeField] private int[] damage;
    [NamedArrayAttribute(new string[] { "hitBox", "hotZone" ,"triggerArea"})]
    [SerializeField] public GameObject[] enemyZone;
    [SerializeField] protected LayerMask playerLayers;

    //HideInInsepctor Variables
    [HideInInspector] public Transform target;
    [HideInInspector] public bool inRange; //Check if Player is in range
    protected Hashtable check = new Hashtable();
    private Hashtable health = new Hashtable();
    protected Animator anim;
    private float distance; //Store the distance b/w enemy and player
    protected float intTimer;
    protected Transform hitBoxTransform;

    protected virtual void Start()
    {
        check.Add("isHurt", false);
        check.Add("attackMode", false);
        check.Add("cooling", false);//Check if Enemy is cooling after attack
        health.Add("maxHealth", (int)100);
        health.Add("currentHealth", (int)100);
    }

    void Awake()
    {
        SelectTarget();
        intTimer = enemyVar[2]; //Store the inital value of timer
        anim = GetComponent<Animator>();
    }

    protected virtual void Update()
    {
        if (!(bool)check["attackMode"])
        {
            Move();
        }

        if (!InsideOfLimits() && !inRange && !anim.GetCurrentAnimatorStateInfo(0).IsName("Enemy_attack"))
        {
            SelectTarget();
        }

        if (inRange)
        {
            EnemyLogic();
        }
    }

    protected virtual void EnemyLogic()
    {
        distance = Vector2.Distance(transform.position, target.position);

        if (distance > enemyVar[0])
        {
            StopAttack();
        }
        else if (enemyVar[0] >= distance && (bool)check["cooling"] == false)
        {
            Attack();
        }

        if ((bool)check["cooling"])
        {
            Cooldown();
            anim.SetBool("Attack", false);
        }
    }

    void Move()
    {
        anim.SetBool("canWalk", true);

        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Enemy_attack"))
        {
            Vector2 targetPosition = new Vector2(target.position.x, transform.position.y);

            transform.position = Vector2.MoveTowards(transform.position, targetPosition, enemyVar[1] * Time.deltaTime);
        }
    }

    void Attack()
    {

        hitBoxTransform = enemyZone[0].transform;
        enemyVar[2] = intTimer; //Reset Timer when Player enter Attack Range
        check["attackMode"] = true; //To check if Enemy can still attack or not

        anim.SetBool("canWalk", false);
        anim.SetBool("Attack", true);

        //Detect player in range of attack
        Collider2D[] player = Physics2D.OverlapCircleAll(hitBoxTransform.position, enemyVar[3], playerLayers);

        foreach (Collider2D hitPlayer in player)
        {
            if (!hitPlayer.GetComponent<Animator>().GetBool("isDead"))
            {
                hitPlayer.GetComponent<PlayerControllerDemo>().DamagePlayer(damage[1]);
            }

        }


    }

    void Cooldown()
    {
        enemyVar[2] -= Time.deltaTime;

        if (enemyVar[2] <= 0 && (bool)check["cooling"] && (bool)check["attackMode"])
        {
            check["cooling"] = false;
            enemyVar[2] = intTimer;
        }
    }

    void StopAttack()
    {
        check["cooling"] = false;
        check["attackMode"] = false;
        anim.SetBool("Attack", false);
    }

    public void TriggerCooling()
    {
        check["cooling"] = true;
    }

    private bool InsideOfLimits()
    {
        return transform.position.x > enemyLimit[0].position.x && transform.position.x < enemyLimit[1].position.x;
    }

    public void SelectTarget()
    {
        float distanceToLeft = Vector3.Distance(transform.position, enemyLimit[0].position);
        float distanceToRight = Vector3.Distance(transform.position, enemyLimit[1].position);

        if (distanceToLeft > distanceToRight)
        {
            target = enemyLimit[0];
        }
        else
        {
            target = enemyLimit[1];
        }

        //Ternary Operator
        //target = distanceToLeft > distanceToRight ? leftLimit : rightLimit;

        Flip();
    }

    public void Flip()
    {
        Vector3 rotation = transform.eulerAngles;
        if (transform.position.x > target.position.x)
        {
            rotation.y = 180;
        }
        else
        {
            rotation.y = 0;
        }

        //Ternary Operator
        //rotation.y = (currentTarget.position.x < transform.position.x) ? rotation.y = 180f : rotation.y = 0f;

        transform.eulerAngles = rotation;
    }



    public void TakeDamage()
    {
        health["currentHealth"] = (int)health["currentHealth"] - damage[0];
        anim.SetTrigger("Hurt");
        Debug.Log("enemy hurt");
        if ((int)health["currentHealth"] <= 0)
        {
            Die();
        }
        else
        {
            check["isHurt"] = true;
        }
    }

    void Die()
    {
        Debug.Log("Enemy Died");
        anim.SetBool("isDead", true);

        //GetComponentInChildren<Collider2D>().enabled = false;
        GetComponent<Enemy_behaviour>().enabled = false;
        GetComponentInChildren<HotZoneCheck>().enabled = false;
        this.enabled = false;

    }

}
