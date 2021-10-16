using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    [SerializeField]
    private float maxSpeed;
    [SerializeField]
    private float jumpPower;
    [SerializeField]
    private float slopeCheckDistance;
    [SerializeField]
    private float groundCheckRadius;
    [SerializeField]
    private Transform groundCheck;
    [SerializeField]
    private LayerMask whatIsGround;
    [SerializeField]
    private PhysicsMaterial2D noFriction;
    [SerializeField]
    private PhysicsMaterial2D fullFriction;

    private float h;
    private float slopeDownAngle;
    private float slopeDownAngleOld;
    private float slopeSideAngle;

    private bool isGrounded;
    private bool canJump;
    private bool isJumping;
    private bool isOnSlope;

    private Vector2 colliderSize;
    private Vector2 slopeNormalPerp;
    private Vector2 newVelocity;

    //component
    Rigidbody2D rigid;
    Animator anim;
    SpriteRenderer spriteRenderer;
    CapsuleCollider2D capsule;
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        capsule = GetComponent<CapsuleCollider2D>();

        colliderSize = capsule.size;
    }

    void Update() {
        if (Input.GetButtonDown("Jump")) {
            Jump();
        }
        //animation
        if (Mathf.Abs(rigid.velocity.x) > 0.2f)
            anim.SetBool("isWalk", true);
        else
            anim.SetBool("isWalk", false);
    }
    void FixedUpdate()
    {
        h = Input.GetAxisRaw("Horizontal");
        //rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);


        if (rigid.velocity.x > maxSpeed)
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
        if (rigid.velocity.x < maxSpeed * -1)
            rigid.velocity = new Vector2(maxSpeed * -1, rigid.velocity.y);


        if (Input.GetButton("Horizontal"))
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;

        Move();
        CheckGround();
        SlopeCheck();
    }
    private void Jump() {
        if (!canJump)
            return;

        canJump = false;
        isJumping = true;

        rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
    }
    private void Move() {
        if (isGrounded && !isOnSlope && !isJumping) { //평지
            newVelocity.Set(maxSpeed * h, 0.0f);
            rigid.velocity = newVelocity;
        }
        else if(isGrounded && isOnSlope && !isJumping) { // 경사면
            newVelocity.Set(maxSpeed * slopeNormalPerp.x * -h, maxSpeed * slopeNormalPerp.y * -h);
            rigid.velocity = newVelocity;
        }
        else if (!isGrounded){ // 점프중
            newVelocity.Set(maxSpeed * h, rigid.velocity.y);
            rigid.velocity = newVelocity;
        }
    }
    private void CheckGround() {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);

        if (rigid.velocity.y <= 0.0f)
            isJumping = false;
        if (isGrounded && !isJumping)
            canJump = true;
    }
    private void SlopeCheck() {
        Vector2 checkPos = transform.position - new Vector3(0.0f, colliderSize.y / 2);

        SlopeCheckHorizontal(checkPos);
        SlopeCheckVertical(checkPos);
    }
    private void SlopeCheckHorizontal(Vector2 checkPos) {
        RaycastHit2D slopeHitFront = Physics2D.Raycast(checkPos, transform.right, slopeCheckDistance, whatIsGround);
        RaycastHit2D slopeHitBack = Physics2D.Raycast(checkPos, -transform.right, slopeCheckDistance, whatIsGround);

        if (slopeHitFront) {
            isOnSlope = true;
            slopeSideAngle = Vector2.Angle(slopeHitFront.normal, Vector2.up);
        }
        else if (slopeHitBack) {
            isOnSlope = true;
            slopeSideAngle = Vector2.Angle(slopeHitBack.normal, Vector2.up);
        }
        else {
            slopeSideAngle = 0.0f;
            isOnSlope = false;
        }
    }
    private void SlopeCheckVertical(Vector2 checkPos) {
        RaycastHit2D rayHit = Physics2D.Raycast(checkPos, Vector2.down, slopeCheckDistance, whatIsGround);

        if (rayHit) {
            slopeNormalPerp = Vector2.Perpendicular(rayHit.normal).normalized;

            slopeDownAngle = Vector2.Angle(rayHit.normal, Vector2.up);

            if(slopeDownAngle != slopeDownAngleOld) {
                isOnSlope = true;
            }
            slopeDownAngleOld = slopeDownAngle;

            Debug.DrawRay(rayHit.point, slopeNormalPerp, Color.red);
            Debug.DrawRay(rayHit.point, rayHit.normal, Color.green);

            Debug.Log(isOnSlope);
        }

        if(isOnSlope && h == 0.0f) {
            rigid.sharedMaterial = fullFriction;
        }
        else {
            rigid.sharedMaterial = noFriction;
        }
    }


    private void OnDrawGizmos() {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
