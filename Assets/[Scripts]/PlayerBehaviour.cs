using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    [Header("Movement")] 
    public float horizontalForce;
    public float verticalForce;

    public Vector2 maxVelocity;
    [Range(0.0f, 1.0f)] public float airMoveFactor = 0.5f;

    [Header("Ground")]
    public bool isGrounded;
    public Transform groundOrigin;
    public float groundRadius;
    public LayerMask groundLayerMask;

    private Animator animationControler;
    private Rigidbody2D rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animationControler = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
        CheckIfGrounded();
    }

    private void Move()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float horizontalMoveForce = x * horizontalForce * Time.deltaTime;

        float mass = rigidbody.mass * rigidbody.gravityScale;

        if (isGrounded)
        {
            // Keyboard Input
            float y = Input.GetAxisRaw("Vertical");
            float jump = Input.GetAxisRaw("Jump");

            // Touch Input
            Vector2 worldTouch = new Vector2();
            foreach (var touch in Input.touches)
            {
                worldTouch = Camera.main.ScreenToWorldPoint(touch.position);
            }

            float jumpMoveForce = jump * verticalForce * Time.deltaTime;

            rigidbody.AddForce(new Vector2(horizontalMoveForce, jumpMoveForce) * mass);
            rigidbody.velocity *= 0.99f; // scaling / stopping hack

            if (rigidbody.velocity.sqrMagnitude > 0.1f)
            {
                animationControler.SetInteger("AnimationState", (int)PlayerAnimationEnum.RUN);
            }
            else
            {
                animationControler.SetInteger("AnimationState", (int)PlayerAnimationEnum.IDLE);
            }

        }
        else
        {
            animationControler.SetInteger("AnimationState", (int)PlayerAnimationEnum.JUMP);

            rigidbody.AddForce(new Vector2(horizontalMoveForce * airMoveFactor, 0.0f) * mass);
        }

        if (x != 0)
        {
            x = FlipAnimation(x);
        }

        rigidbody.velocity = new Vector2(Mathf.Clamp(rigidbody.velocity.x, -maxVelocity.x, maxVelocity.x),
                Mathf.Clamp(rigidbody.velocity.y, -maxVelocity.y, maxVelocity.y));
    }

    private void CheckIfGrounded()
    {
        RaycastHit2D hit = Physics2D.CircleCast(groundOrigin.position, groundRadius, Vector2.down, groundRadius, groundLayerMask);

        isGrounded = (hit) ? true : false;
    }

    private float FlipAnimation(float x)
    {
        // depending on direction scale across the x-axis either 1 or -1
        x = (x > 0) ? 1 : -1;

        transform.localScale = new Vector3(x, 1.0f);
        return x;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            transform.SetParent(collision.transform);
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            transform.SetParent(null);
        }
    }

    // UTILITIES

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundOrigin.position, groundRadius);
    }

}
