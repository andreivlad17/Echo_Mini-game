using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D myRigidbody;

    private Animator myAnimator;

    [SerializeField]
    private float movementSpeed;

    private bool facingRight;

    [SerializeField]
    private Transform[] groundPoints;

    [SerializeField]
    private float groundRadius;

    [SerializeField]
    private LayerMask whatIsGround;

    private bool isGrounded;

    private bool jump;

    [SerializeField]
    private float jumpForce;

    [SerializeField]
    private bool airControl;

    

	// Use this for initialization
	void Start ()
    {
        facingRight = true;
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
	}

    void Update()
    {
        HandleInput();
    }

    // Update is called once per frame
    void FixedUpdate ()
    {
        float horizontal = Input.GetAxis("Horizontal");

        isGrounded = IsGrounded();
        
        HandleMovement(horizontal);

        Flip(horizontal);
	}

    private void HandleMovement(float horizontal)
    {
        if(isGrounded && jump)
        {
            isGrounded = false;
            myRigidbody.AddForce(new Vector2(0, jumpForce));
        }
        
        myRigidbody.velocity = new Vector2(horizontal * movementSpeed , myRigidbody.velocity.y); //(-1,0) 

        myAnimator.SetFloat("speed",Mathf.Abs(horizontal));
    }

    private void HandleInput()
    {
        if(Input.GetKeyDown(KeyCode.W))
        {
            jump = true;
        }
        else
        {
            jump = false;
        }
    }

    private void Flip(float horizontal)
    {
        if(horizontal>0 && !facingRight || horizontal<0 && facingRight)
        {
            facingRight = !facingRight;
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
    }

    private void ResetValues()
    {
        //jump = false;
    }

    private bool IsGrounded()
    {
        if(myRigidbody.velocity.y <=0 )
        {
            foreach (Transform point in groundPoints)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(point.position, groundRadius, whatIsGround);
                for (int i = 0; i < colliders.Length; i++)
                {
                    if (colliders[i].gameObject != gameObject)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

}
