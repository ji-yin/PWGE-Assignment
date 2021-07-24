using System.Collections;
using UnityEngine;


    public class BigBoss : MonoBehaviour
    {
        public Animator animator;
        public int maxHealth = 100;
        int currentHealth;
        // Use this for initialization
        void Start()
        {
            currentHealth = maxHealth;

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void TakeDamage(int damage)
        {
            currentHealth -= damage;
        animator.SetTrigger("Hurt");

            if (currentHealth <= 0)
            {
                Die();
            }
        }

        void Die()
        {
            Debug.Log("Enemy Died");
        animator.SetBool("isDead", true);

        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
    }
    }
