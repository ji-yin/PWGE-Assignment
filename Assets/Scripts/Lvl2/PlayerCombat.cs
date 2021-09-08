using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerCombat : MonoBehaviour
{
    public Animator animator;

    public Transform AttackPoint;
    public LayerMask enemyLayers;

    public float attackRange = 0.5f;
    public int attackDamage = 40;

    public float attackRate = 2f;
    float nextAttackTime = 0f;

    public int maxHealth = 100;
    int currentHealth;
    [SerializeField] private Text healthAmount;

    void Start()
    {
        currentHealth = maxHealth;
        healthAmount.text = currentHealth.ToString();

    }

    void Update()
    {
        if(Time.time>= nextAttackTime)
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

            foreach(Collider2D enemy in hitEnemies)
            {
                enemy.GetComponentInParent<BigBoss>().TakeDamage(attackDamage);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        while(AttackPoint == null){
            return;
        }
        Gizmos.DrawWireSphere(AttackPoint.position, attackRange);
    }

    public void DamagePlayer(int damage)
    {
        currentHealth -= damage;
        animator.SetInteger("state", 4);
        if (currentHealth <= 0)
        {
            PlayerDie();
        }
    }

    void PlayerDie()
    {
        Debug.Log("Player Died");
        animator.SetBool("isDead",true);

        GetComponent<Collider2D>().enabled = false;
        GetComponent<PlayerControllerDemo>().enabled = false;
        this.enabled = false;
    }
}
