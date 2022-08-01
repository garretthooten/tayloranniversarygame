using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpForce = 14f;
    [SerializeField] private LayerMask jumpableGround;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private LayerMask bounceBoxLayer;
    [SerializeField] private AudioClip jump;
    [SerializeField] private AudioClip collect;
    [SerializeField] private AudioClip enemyBounce;

    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private Animator anim;
    private SpriteRenderer sprite;
    private AudioSource audio;
    private PlayerHealth health;
    private DistanceJoint2D joint;
    private GameObject closestAnchor;
    private LineRenderer lineRenderer;

    private bool isConnected = false;
    private bool canGrapple = false;
    private bool canBounce = false;
    private float dirX = 0f;

    private enum MovementState { idle, running, jumping, falling }
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        audio = GetComponent<AudioSource>();
        health = GetComponent<PlayerHealth>();
        joint = GetComponent<DistanceJoint2D>();
        lineRenderer = GetComponent<LineRenderer>();

        joint.enabled = false;
        lineRenderer.enabled = false;

        
        lineRenderer.sortingOrder = 1;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.material.color = Color.red;
        
    }

    // Update is called once per frame
    void Update()
    {

        dirX = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(moveSpeed * dirX, rb.velocity.y);

        if (rb.velocity.y == 0f)
            canBounce = true;

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            audio.PlayOneShot(jump, 0.7f);
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        if (OnTopOfEnemy() && canBounce)
        {
            audio.PlayOneShot(enemyBounce, 0.7f);
            rb.velocity = new Vector2(rb.velocity.x, jumpForce * 0.75f);
            canBounce = false;
        }

        if (canGrapple)
        {
            GrappleEvent();
        }

        UpdateAnimationState();

    }

    private void UpdateAnimationState()
    {

        MovementState state;

        if (dirX > 0f)
        {
            state = MovementState.running;
            sprite.flipX = false;
        }
        else if (dirX < 0f)
        {
            state = MovementState.running;
            sprite.flipX = true;
        }
        else
        {
            state = MovementState.idle;
        }

        if (rb.velocity.y > 0.1f)
            state = MovementState.jumping;
        else if (rb.velocity.y < -0.1f)
            state = MovementState.falling;

        anim.SetInteger("state", (int)state);
    }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, 0.1f, jumpableGround);
    }

    private bool OnTopOfEnemy()
    {
        bool found = false;
        RaycastHit2D myHit = Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, 0.1f, bounceBoxLayer);
        if (myHit)
        {
            found = true;

            //must call parent because bouncebox above enemies is a child of the enemy
            GameObject enemyHit = myHit.collider.gameObject.transform.parent.gameObject;

            Debug.Log("Jumped on enemy " + enemyHit.name);

            if (canBounce)
            {
                EnemyBehavior enb = enemyHit.GetComponent<EnemyBehavior>();
                enb.DamageDealt();
            }
        }
        
        return found;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            health.Damage();
        }
        else if (collision.CompareTag("DeathBox"))
        {
            health.Die();
        }
    }

    public void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("GrappleAnchor"))
        {
            if (!isConnected)
            {
                closestAnchor = collider.transform.parent.gameObject;
            }
            canGrapple = true;
        }
        else
            canGrapple = false;
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("GrappleAnchor"))
        {
            canGrapple = false;
        }
    }

    private void GrappleEvent()
    {
        if (Input.GetButton("Fire1"))
        {
            isConnected = true;
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, closestAnchor.transform.position);
            joint.connectedAnchor = closestAnchor.transform.position;
            lineRenderer.enabled = true;
            joint.enabled = true;
        }
        else
        {
            isConnected = false;
            lineRenderer.enabled = false;
            joint.enabled = false;
        }
    }

}
