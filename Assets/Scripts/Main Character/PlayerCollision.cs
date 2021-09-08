using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCollision : MonoBehaviour
{
    public float healthAmount;
    public Slider healthBar;

    public float maxInvinsibleTime;

    private float defaultHealth;
    private Vector3 respawnPoint;
    private bool isBeingAttacked;
    private Animator animator;

    private SpriteRenderer player;
    private Color color;
    private float activiationTime;
    private bool invinsible;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        isBeingAttacked = false;
        respawnPoint = transform.position;
        defaultHealth = healthAmount;

        player = GetComponent<SpriteRenderer>();
        activiationTime = 0;
        invinsible = false;
        color = player.color;
    }

    private void Update()
    {
        healthBar.value = healthAmount;

        activiationTime += Time.deltaTime;

        if (invinsible && activiationTime >= maxInvinsibleTime)
        {
            invinsible = false;
            color.a = 1;
            player.color = color;
        }
    }

    // Update is called once per frame
    private void OnCollisionEnter2D(Collision2D collision)
    {
        CheckCollision(collision.gameObject);
    }

    void CheckCollision(GameObject collider)
    {
        if (collider.gameObject.tag == "Enemy" && !isBeingAttacked && !invinsible)
        {
            invinsible = true;
            activiationTime = 0;
            color.a = .1f;
            player.color = color;

            isBeingAttacked = true;
            healthAmount -= 20;
            TakeHit();
            Invoke("StopBeingAttacked", 0.5f);
        }

        if (collider.gameObject.tag == "OutOfTheWorld")
        {
            Respawn();
        }
    }

    void TakeHit()
    {
        if (healthAmount <= 0)
        {
            Respawn();
        }
        else
        {
            animator.SetBool("BeingAttacked", true);
            Debug.Log("Being Attacked");

            Invoke("StopBeingAttacked", 0.3f);
        }
    }

    void StopBeingAttacked()
    {
        animator.SetBool("BeingAttacked", false);
        isBeingAttacked = false;
    }

    void Respawn()
    {
        transform.position = respawnPoint;      // respawn
        healthAmount = defaultHealth;
        invinsible = false;
        activiationTime = 0;
        color.a = 1;
        player.color = color;

        CoinCountScript.coinAmount = 0;     // reset coin
    }
}
