using System.Collections;
using UnityEngine;


public class BigBoss : MonoBehaviour
{
    public Animator animator;
    public int maxHealth = 100;
    int currentHealth;
    public Transform hitBox;
    public LayerMask playerLayers;
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

         GetComponentInChildren<Collider2D>().enabled = false;
         GetComponent<Enemy_behaviour>().enabled = false;
         GetComponentInChildren<HotZoneCheck>().enabled = false;
         this.enabled = false;
     }



}
