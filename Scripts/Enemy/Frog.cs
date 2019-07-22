using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frog : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private GameObject player;
    [SerializeField] private Rigidbody2D rb2d;
    [SerializeField] private float playerRange;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float jumpForce = 300f;
    [SerializeField] private float moveSpeedMultiplier = 75f;
	public LayerMask playerLayer;
    private float groundedRadius = .2f;
    private bool isGrounded = true;
    void FixedUpdate()
    {
		Collider2D hitPlayer = Physics2D.OverlapCircle(transform.position, .6f, playerLayer);
		if(hitPlayer)
		{	        
			hitPlayer.GetComponent<PlayerInput>().Die();
		}

        if ((Mathf.Abs(player.transform.position.x - transform.position.x)) <= playerRange)
        {
            anim.SetBool("isPlayerInRange", true);
        }
        if (anim.GetBool("isPlayerInRange") && isGrounded)
        {
            float moveSpeed = (player.transform.position.x - transform.position.x) > 0 ? moveSpeedMultiplier : -moveSpeedMultiplier;
            rb2d.AddForce(new Vector2(moveSpeed, jumpForce));
        }
        anim.SetFloat("verticalSpeed", rb2d.velocity.y);
        if (rb2d.velocity.x > 0)
        {
            transform.eulerAngles = new Vector2(0, 180);
        }
        else
        {
            transform.eulerAngles = new Vector2(0, 0);
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.otherCollider.IsTouchingLayers(whatIsGround))
        {
            isGrounded = true;
        }
    }
    void OnCollisionExit2D()
    {
        isGrounded = false;
    }
}
