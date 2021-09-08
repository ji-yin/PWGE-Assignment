using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FK_behaviour : Enemy_behaviour
{
    public GameObject jumpHitBox;
    [NamedArrayAttribute(new string[] { "jumpLength", "jumpHeight" })]
    [SerializeField] private float[] jumpVar;
    [SerializeField] private LayerMask ground;
    [SerializeField] private int jumpDamage;
    public Collider2D coll;
    private Rigidbody2D rb;
    private Transform jumpHitBoxTransform;

    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
    }

    protected override void EnemyLogic()
    {
        base.EnemyLogic();
        if ((bool)check["isHurt"] && !(bool)check["cooling"])
        {
            Debug.Log("JumpAttack");
            JumpAttack();
            check["isHurt"] = false;
        }
        if (!(bool)check["isHurt"])
        {
            StopJumpAttack();
        }
    }



    void JumpAttack()
    {
        jumpHitBoxTransform = jumpHitBox.transform;
        enemyVar[2] = intTimer; //Reset Timer when Player enter Attack Range
        check["attackMode"] = true; //To check if Enemy can still attack or not

        anim.SetBool("canWalk", false);
        rb.velocity = new Vector2(jumpVar[0], jumpVar[1]);
        anim.SetBool("JumpAttack", true);

        //Detect player in range of attack
        Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(jumpHitBoxTransform.position, enemyVar[3], playerLayers);


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
        check["cooling"] = false;
        check["attackMode"] = false;
        anim.SetBool("JumpAttack", false);
    }
}
