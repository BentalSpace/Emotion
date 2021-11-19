using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRenewal : MonoBehaviour {
    [SerializeField]
    private float maxSpeed;
    [SerializeField]
    private float jumpPower;
    [SerializeField]
    private float slopeCheckDistance;
    [SerializeField]
    private PhysicsMaterial2D noFriction;
    [SerializeField]
    private PhysicsMaterial2D fullFriction;
    [SerializeField]
    private Transform groundCheck;
    [SerializeField]
    private float groundCheckRadius;
    [SerializeField]
    private LayerMask whatIsGround;

    private float h;
    private float slopeDownAngle;
    private float slopeDownAngleOld;

    private bool isSlope;
    private bool canJump;
    private bool isGround;
    private bool isJumping;

    private Vector2 slopeNormalPerp;
    private Vector2 colliderSize;

    //component
    Rigidbody2D rigid;
    CapsuleCollider2D capsule;
    SpriteRenderer spriteRenderer;
    Animator anim;

    void Awake() {
        rigid = GetComponent<Rigidbody2D>();
        capsule = GetComponent<CapsuleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        colliderSize = capsule.size;
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetButtonDown("Jump")) {
            Jump();
        }
    }
    void FixedUpdate() {
        h = Input.GetAxisRaw("Horizontal");

        EtcMove();

        Move();
        SlopeCheck();
        GroundCheck();

        Debug.Log(rigid.velocity.y);
    }

    void Move() {
        //Move
        if (!isSlope)
            rigid.velocity = new Vector2(maxSpeed * h, rigid.velocity.y);
        else if (isSlope && !isJumping)
            rigid.velocity = new Vector2(maxSpeed * slopeNormalPerp.x * -h, maxSpeed * slopeNormalPerp.y * -h);
    }
    void Jump() {
        if (!canJump)
            return;

        canJump = false;
        isJumping = true;
        rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
    }
    void GroundCheck() {
        isGround = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);

        if (rigid.velocity.y <= 0)
            isJumping = false;

        if (isGround && !isJumping)
            canJump = true;

        if (!isGround) {
            canJump = false;
        }
    }
    void EtcMove() {
        //flipX and friction and walk animation
        if (Input.GetButton("Horizontal")) {
            rigid.sharedMaterial = noFriction;
            spriteRenderer.flipX = h == -1;
            anim.SetBool("isWalk", true);
        }
        else {
            rigid.sharedMaterial = fullFriction;
            anim.SetBool("isWalk", false);
        }

        anim.SetBool("isJump", isJumping);
        anim.SetFloat("jumpValue", rigid.velocity.y); 

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("jumpStart")
                 && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f)
            anim.SetTrigger("jumpTrigger");

        anim.SetBool("isGround", isGround); 

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("jumpEnd")
             && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f)
            anim.SetTrigger("jumpTrigger");
    }

    void SlopeCheck() {
        // 경사인지 아닌지 체크하는 함수
        Vector2 checkPos = transform.position - new Vector3(0.0f, colliderSize.y / 2);

        SlopeCheckHorizontal(checkPos);
        SlopeCheckVertical(checkPos);
    }
    private void SlopeCheckHorizontal(Vector2 checkPos) {
        RaycastHit2D slopeHitFront = Physics2D.Raycast(checkPos, transform.right, slopeCheckDistance, whatIsGround);
        RaycastHit2D slopeHitBack = Physics2D.Raycast(checkPos, -transform.right, slopeCheckDistance, whatIsGround);
        // Debug.DrawRay(slopeHitFront.point, slopeHitFront.normal, Color.blue);
        // Debug.DrawRay(slopeHitBack.point, slopeHitBack.normal, Color.magenta);
        if (slopeHitFront) {
            isSlope = true;
        }
        else if (slopeHitBack) {
            isSlope = true;
        }
        else {
            isSlope = false;
        }

        //if (!slopeHitBack && !slopeHitFront)
        //    isSlope = false;
    }
    void SlopeCheckVertical(Vector2 checkPos) {
        RaycastHit2D rayHit = Physics2D.Raycast(checkPos, Vector2.down, slopeCheckDistance, whatIsGround);

        if (rayHit) {
            slopeNormalPerp = Vector2.Perpendicular(rayHit.normal).normalized;

            slopeDownAngle = Vector2.Angle(rayHit.normal, Vector2.up);

            if (slopeDownAngle != slopeDownAngleOld) {
                isSlope = true;
            }
            slopeDownAngleOld = slopeDownAngle;

            Debug.DrawRay(rayHit.point, slopeNormalPerp, Color.red);
            Debug.DrawRay(rayHit.point, rayHit.normal, Color.green);
        }
    }


    private void OnDrawGizmos() {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
