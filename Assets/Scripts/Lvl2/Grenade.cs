using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public float delay = 3f;
    public float radius = 5f;
    public float force = 20f;

    public GameObject explosionEffect;
    public LayerMask playerLayers;

    float countdown;
    bool hasExploded = false;
    [SerializeField] private int explodeDamage;
    // Start is called before the first frame update
    void Start()
    {
        countdown = delay;
    }

    // Update is called once per frame
    void Update()
    {
        countdown -= Time.deltaTime;
        if (countdown <= 0f && !hasExploded)
        {
            Explode();
            hasExploded = true;
        }
    }

    void Explode()
    {
        //Show effect
        Instantiate(explosionEffect, transform.position, Quaternion.identity);
        //Get player
        Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(transform.position, radius, playerLayers);

        foreach (Collider2D player in hitPlayer)
        {
            Vector2 direction = player.transform.position - transform.position;

            player.GetComponent<Rigidbody2D>().AddForce(direction * force);

            if (!player.GetComponent<Animator>().GetBool("isDead"))
            {
                player.GetComponent<PlayerControllerDemo>().DamagePlayer(explodeDamage);
            }

        }

        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
