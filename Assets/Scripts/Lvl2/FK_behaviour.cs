using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FK_behaviour : Enemy_behaviour
{
    public GameObject jumpHitBox;
    private Transform jumpHitBoxTransform;
    [SerializeField] private float jumpLength;
    [SerializeField] private float jumpHeight;
    [SerializeField] private LayerMask ground;
    [SerializeField] private int jumpDamage;
    public Collider2D coll;
    private Rigidbody2D rb;

    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
    }

    protected override void EnemyLogic()
    {
        base.EnemyLogic();
        if (isHurt && cooling==false)
        {
            Debug.Log("JumpAttack");
            JumpAttack();
            isHurt = false;
        }
        if (!isHurt)
        {
            StopJumpAttack();
        }
    }



    void JumpAttack()
    {
        jumpHitBoxTransform = jumpHitBox.transform;
        timer = intTimer; //Reset Timer when Player enter Attack Range
        attackMode = true; //To check if Enemy can still attack or not

        anim.SetBool("canWalk", false);
        rb.velocity = new Vector2(jumpLength, jumpHeight);
        anim.SetBool("JumpAttack", true);

        //Detect player in range of attack
        Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(jumpHitBoxTransform.position, attackRange, playerLayers);


        foreach (Collider2D player in hitPlayer)
        {
            if (!player.GetComponent<Animator>().GetBool("isDead"))
            {
                player.GetComponent<PlayerControllerDemo>().DamagePlayer(jumpDamage);
            }

        }

    }

    void StopJumpAttack()
    {
        cooling = false;
        attackMode = false;
        anim.SetBool("JumpAttack", false);
    }
}
