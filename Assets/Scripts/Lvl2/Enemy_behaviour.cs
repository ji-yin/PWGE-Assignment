<<<<<<< HEAD
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_behaviour : MonoBehaviour
{
    #region Public Variables
    public float attackDistance; //Minimum distance for attack
    public float moveSpeed;
    public float timer; //Timer for cooldown between attacks
    public Transform leftLimit;
    public Transform rightLimit;
    [HideInInspector] public Transform target;
    [HideInInspector] public bool inRange; //Check if Player is in range
    [SerializeField] private int enemyDamage;
    [SerializeField] private int playerDamage;
    public GameObject hotZone;
    public GameObject triggerArea;
    public GameObject hitBox;
    public float attackRange = 0.5f;
    public LayerMask playerLayers;
    public int maxHealth = 100;
    int currentHealth;
    public float cameraShakeIntensity = 5f;
    public float cameraShakeTime = 0.1f;
    public bool isHurt=false;
    #endregion

    #region Private Variables
    protected Animator anim;
    private float distance; //Store the distance b/w enemy and player
    protected bool attackMode;
    protected bool cooling; //Check if Enemy is cooling after attack
    protected float intTimer;
    protected Transform hitBoxTransform;

    #endregion

=======
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_behaviour : MonoBehaviour
{
    #region Public Variables
    public float attackDistance; //Minimum distance for attack
    public float moveSpeed;
    public float timer; //Timer for cooldown between attacks
    public Transform leftLimit;
    public Transform rightLimit;
    [HideInInspector] public Transform target;
    [HideInInspector] public bool inRange; //Check if Player is in range
    [SerializeField] private int enemyDamage;
    [SerializeField] private int playerDamage;
    public GameObject hotZone;
    public GameObject triggerArea;
    public GameObject hitBox;
    public float attackRange = 0.5f;
    public LayerMask playerLayers;
    public int maxHealth = 100;
    int currentHealth;
    public float cameraShakeIntensity = 5f;
    public float cameraShakeTime = 0.1f;
    public bool isHurt=false;
    #endregion

    #region Private Variables
    protected Animator anim;
    private float distance; //Store the distance b/w enemy and player
    protected bool attackMode;
    protected bool cooling; //Check if Enemy is cooling after attack
    protected float intTimer;
    protected Transform hitBoxTransform;

    #endregion

>>>>>>> parent of b93abbce (lvl3)
    protected virtual void Start()
    {
        currentHealth = maxHealth;

<<<<<<< HEAD
    }

    void Awake()
    {
        SelectTarget();
        intTimer = timer; //Store the inital value of timer
        anim = GetComponent<Animator>();
    }

    protected virtual void Update()
    {
        if (!attackMode)
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

        if (distance > attackDistance)
        { 
            StopAttack();
        }
        else if (attackDistance >= distance && cooling == false)
        {
            Attack();
        }

        if (cooling)
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

            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }
    }

    void Attack()
    {
        
        hitBoxTransform = hitBox.transform;
        timer = intTimer; //Reset Timer when Player enter Attack Range
        attackMode = true; //To check if Enemy can still attack or not

        anim.SetBool("canWalk", false);
=======
    }

    void Awake()
    {
        SelectTarget();
        intTimer = timer; //Store the inital value of timer
        anim = GetComponent<Animator>();
    }

    protected virtual void Update()
    {
        if (!attackMode)
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

        if (distance > attackDistance)
        { 
            StopAttack();
        }
        else if (attackDistance >= distance && cooling == false)
        {
            Attack();
        }

        if (cooling)
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

            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }
    }

    void Attack()
    {
        
        hitBoxTransform = hitBox.transform;
        timer = intTimer; //Reset Timer when Player enter Attack Range
        attackMode = true; //To check if Enemy can still attack or not

        anim.SetBool("canWalk", false);
>>>>>>> parent of b93abbce (lvl3)
        anim.SetBool("Attack", true);

        //Detect player in range of attack
        Collider2D[] player = Physics2D.OverlapCircleAll(hitBoxTransform.position, attackRange, playerLayers);

        foreach(Collider2D hitPlayer in player)
        {
            if (!hitPlayer.GetComponent<Animator>().GetBool("isDead"))
            {
                hitPlayer.GetComponent<PlayerControllerDemo>().DamagePlayer(playerDamage);
            }

        }
        

<<<<<<< HEAD
    }

    void Cooldown()
    {
        timer -= Time.deltaTime;

        if (timer <= 0 && cooling && attackMode)
        {
            cooling = false;
            timer = intTimer;
        }
    }

    void StopAttack()
    {
        cooling = false;
        attackMode = false;
        anim.SetBool("Attack", false);
    }

    public void TriggerCooling()
    {
        cooling = true;
    }

    private bool InsideOfLimits()
    {
        return transform.position.x > leftLimit.position.x && transform.position.x < rightLimit.position.x;
    }

    public void SelectTarget()
    {
        float distanceToLeft = Vector3.Distance(transform.position, leftLimit.position);
        float distanceToRight = Vector3.Distance(transform.position, rightLimit.position);

        if (distanceToLeft > distanceToRight)
        {
            target = leftLimit;
        }
        else
        {
            target = rightLimit;
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



=======
    }

    void Cooldown()
    {
        timer -= Time.deltaTime;

        if (timer <= 0 && cooling && attackMode)
        {
            cooling = false;
            timer = intTimer;
        }
    }

    void StopAttack()
    {
        cooling = false;
        attackMode = false;
        anim.SetBool("Attack", false);
    }

    public void TriggerCooling()
    {
        cooling = true;
    }

    private bool InsideOfLimits()
    {
        return transform.position.x > leftLimit.position.x && transform.position.x < rightLimit.position.x;
    }

    public void SelectTarget()
    {
        float distanceToLeft = Vector3.Distance(transform.position, leftLimit.position);
        float distanceToRight = Vector3.Distance(transform.position, rightLimit.position);

        if (distanceToLeft > distanceToRight)
        {
            target = leftLimit;
        }
        else
        {
            target = rightLimit;
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



>>>>>>> parent of b93abbce (lvl3)
    public void TakeDamage()
    {
        currentHealth -= enemyDamage;
        anim.SetTrigger("Hurt");
        Debug.Log("enemy hurt");
        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            isHurt = true;
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
<<<<<<< HEAD

    }

    private void OnAttackStart()
    {
        CinemachineShake.Instance.ShakeCamera(cameraShakeIntensity, cameraShakeTime);
    }

    private void OnAttackEnd()
    {
        hitBox.SetActive(false);
    }
}
=======
    }

    private void OnAttackStart()
    {
        CinemachineShake.Instance.ShakeCamera(cameraShakeIntensity, cameraShakeTime);
    }

    private void OnAttackEnd()
    {
        hitBox.SetActive(false);
    }
}
>>>>>>> parent of b93abbce (lvl3)
