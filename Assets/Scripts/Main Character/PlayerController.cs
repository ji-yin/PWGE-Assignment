using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public float jumpForce;
    public Transform groundDetection;
    public Transform groundDetectionL;
    public Transform groundDetectionR;
    public LayerMask groundLayer;
    public GameObject hitBox;

    private bool isAttack;
    private bool isBeingAttacked;
    private Vector3 facingRightScale;
    private Vector3 facingLeftScale;
    private bool isGrounded;
    private Animator animator;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent <Rigidbody2D>();
        isGrounded = true;
        facingRightScale = transform.localScale;
        facingLeftScale = transform.localScale;
        facingLeftScale.x *= -1;
        hitBox.SetActive(false);
    }

    private void Update()
    {
        isAttack = animator.GetBool("Attack");
        isBeingAttacked = animator.GetBool("BeingAttacked");

        if (Input.GetKeyDown(KeyCode.Mouse0) && !isAttack && !isBeingAttacked)
        {
            animator.SetBool("Run", false);
            animator.SetBool("Attack", true);
            StartCoroutine(DoAttack());
        }
        else
        {
            animator.SetBool("Attack", false);
        }
    }

    private void FixedUpdate()
    {
        if (Physics2D.Linecast(transform.position, groundDetection.position, groundLayer) ||
            Physics2D.Linecast(transform.position, groundDetectionL.position, groundLayer) ||
            Physics2D.Linecast(transform.position, groundDetectionR.position, groundLayer))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }

        if (Input.GetKey(KeyCode.D))
        {
            rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
            if (isGrounded)
            {
                animator.SetBool("Run", true);
            }

            transform.localScale = facingRightScale;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);

            if (isGrounded)
            {
                animator.SetBool("Run", true);
            }

            transform.localScale = facingLeftScale;
        }
        else
        {
            if (isGrounded)
            {
                animator.SetBool("Run", false);
            }

            rb.velocity = new Vector2(0, rb.velocity.y);
        }

        if (Input.GetKey(KeyCode.Space) && isGrounded && !isAttack)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            animator.SetBool("Jump", true);
        }
        else
        {
            animator.SetBool("Jump", false);
        }
    }

    IEnumerator DoAttack()
    {
        hitBox.SetActive(true);
        yield return new WaitForSeconds(1f);
        hitBox.SetActive(false);
    }
}
