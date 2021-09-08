using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frog : Enemy
{
    [SerializeField] private float leftCap;
    [SerializeField] private float rightCap;

    [SerializeField] private float jumpLength;
    [SerializeField] private float jumpHeight;
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
            if (transform.position.x > leftCap)
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
                    rb.velocity = new Vector2(-jumpLength, jumpHeight);
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
            if (transform.position.x < rightCap)
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
                    rb.velocity = new Vector2(jumpLength, jumpHeight);
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
