using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    [NamedArrayAttribute(new string[] { "delay", "radius", "force" })]
    [SerializeField] private float[] granadeVar;
    [SerializeField] private GameObject explosionEffect;
    [SerializeField] private LayerMask playerLayers;
    [SerializeField] private int explodeDamage;

    float countdown;
    bool hasExploded = false;
    
    // Start is called before the first frame update
    void Start()
    {
        countdown = granadeVar[0];
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
        Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(transform.position, granadeVar[1], playerLayers);

        foreach (Collider2D player in hitPlayer)
        {
            Vector2 direction = player.transform.position - transform.position;

            player.GetComponent<Rigidbody2D>().AddForce(direction * granadeVar[2]);

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
        Gizmos.DrawWireSphere(transform.position, granadeVar[0]);
    }
}
