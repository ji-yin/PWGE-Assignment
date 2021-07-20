using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float jumpForce = 20.0f;
    public float moveSpeed = 2.0f;
    public GameObject sprite;
    public Rigidbody rigidBody;
    private Animator animator;
    private Vector3 thisScale;
    private bool facingRight = true;

    // Start is called before the first frame update
    void Start()
    {
        animator = sprite.GetComponent<Animator>();
        thisScale = sprite.transform.localScale;
        rigidBody = rigidBody.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        MoveObject(moveSpeed);

        // if the character is moving, set move speed to 2
        if (Input.GetAxisRaw("Horizontal") != 0.0f)
        {
            if (Input.GetAxisRaw("Horizontal") < 0.0f && facingRight)
            {
                facingRight = false;
                FlipChar();
            }
            else if (Input.GetAxisRaw("Horizontal") > 0.0f && !facingRight)
            {
                facingRight = true;
                FlipChar();
            }

            animator.SetInteger("MoveSpeed", 2);
        }
        // if the character stops moving, set move speed to 0
        else
        {
            animator.SetInteger("MoveSpeed", 0);
        }

        // if space bar is pressed, the character will jump
        if (Input.GetKeyUp(KeyCode.Space))
        {
            animator.SetTrigger("JumpTrigger");
            rigidBody.AddForce(transform.up * jumpForce);
        }

        // if left click of the mouse is being clicked, the character will attack
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            animator.SetTrigger("AttackTrigger");
        }
    }

    void MoveObject(float moveSpeed)
    {
        // change the position of the object by a set number of floats in the vector
        transform.Translate(new Vector3(1.0f * moveSpeed * Time.deltaTime * Input.GetAxisRaw("Horizontal"), 0.0f, 0.0f));
    }

    void FlipChar()
    {
        thisScale.x *= -1.0f;   // change direction
        sprite.transform.localScale = thisScale;    // insert this new modification as the new localScale
    }
}
