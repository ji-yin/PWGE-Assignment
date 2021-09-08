using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frog : Enemy
{
    [NamedArrayAttribute(new string[] { "leftCap", "rightCap", "jumpLength", "jumpHeight"})]
    [SerializeField] private float[] frogVar;
    [SerializeField] private LayerMask ground;
    private Collider2D coll;
    private bool facingLeft = true;

    protected override void Start()
    {
        base.Start();
        coll = GetComponent<Collider2D>();
    }

    private void Update()
    {
       //transition from Jump to Fall
       if (anim.GetBool("Jumping"))
        {
            if(rb.velocity.y < .1)
            {
                anim.SetBool("Falling", true);
                anim.SetBool("Jumping", false);
            }
        }

        //transition from Fall to Idle
        if (coll.IsTouchingLayers(ground)&& anim.GetBool("Falling"))
        {
            anim.SetBool("Falling", false);
        }
    }

    private void Move()
    {
        if (facingLeft)
        {
            //Test to see if we are beyond the leftCap
            if (transform.position.x > frogVar[0])
            {
                //make suresprite is facing right location, if it is not, then face the right direction
                if (transform.localScale.x != 1)
                {
                    transform.localScale = new Vector3(1, 1);
                }
                //test to see if i am on the ground, of so jump
                if (coll.IsTouchingLayers(ground))
                {
                    //Jump
                    rb.velocity = new Vector2(-frogVar[2], frogVar[3]);
                    anim.SetBool("Jumping", true);
                }
            }
            else
            {
                facingLeft = false;//if it is not, we are going to face Right
            }

        }

        else
        {
            if (transform.position.x < frogVar[1])
            {
                //make suresprite is facing right location, if it is not, then face the right direction
                if (transform.localScale.x != -1)
                {
                    transform.localScale = new Vector3(-1, 1);
                }
                //test to see if i am on the ground, of so jump
                if (coll.IsTouchingLayers(ground))
                {
                    //Jump
                    rb.velocity = new Vector2(frogVar[2], frogVar[3]);
                    anim.SetBool("Jumping", true);
                }
            }
            else
            {
                facingLeft = true;//if it is not, we are going to face Left
            }
        }
    }

    
}
