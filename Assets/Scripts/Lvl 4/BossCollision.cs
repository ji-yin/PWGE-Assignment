using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossCollision : MonoBehaviour
{
    public GameObject hitBox;
    public float healthAmount;
    public Slider healthBar;
    public KeyScript keyDrop;

    private bool isBeingAttacked;
    private Animator animator;


    private void Start()
    {
        animator = GetComponent<Animator>();
        isBeingAttacked = false;

    }

    private void Update()
    {
        healthBar.value = healthAmount;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        CheckCollision(collision.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CheckCollision(collision.gameObject);
    }

    void CheckCollision(GameObject collider)
    {
        if (collider.gameObject == hitBox && !isBeingAttacked)
        {
            isBeingAttacked = true;
            healthAmount -= 2;
            TakeHit();
            Invoke("StopBeingAttacked", 0.5f);
        }
    }

    void StopBeingAttacked()
    {
        isBeingAttacked = false;
    }

    public void TakeHit()
    {
        if (healthAmount <= 0)
        {
            Destroy(gameObject);
            keyDrop.gameObject.SetActive(true);
        }
    }
}
