using UnityEngine;

public class PlayerRenewal : MonoBehaviour {
    [SerializeField]
    private float maxSpeed;
    [SerializeField]
    private float jumpPower;
    [SerializeField]
    private float slopeCheckDistance;
    [SerializeField]
    private float groundCheckRadius;
    [SerializeField]
    private float crawlSpeed;
    [SerializeField]
    private PhysicsMaterial2D noFriction;
    [SerializeField]
    private PhysicsMaterial2D fullFriction;
    [SerializeField]
    private Transform groundCheck;
    [SerializeField]
    private LayerMask whatIsGround;

    private string chapterStageNum;

    private float h;
    private float slopeDownAngle;
    private float slopeDownAngleOld;

    private bool isSlope;
    private bool canJump;
    private bool isGround;
    private bool isJumping;

    private Vector2 slopeNormalPerp;
    private Vector2 colliderSize;

    private GameObject stageNumObject;

    //component
    Rigidbody2D rigid;
    CapsuleCollider2D capsule;
    SpriteRenderer spriteRenderer;
    Animator animator;

    void Awake() {
        rigid = GetComponent<Rigidbody2D>();
        capsule = GetComponent<CapsuleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        colliderSize = capsule.size;
    }
    void Start() {
        //씬 변경 후 스테이지 정보 가져오기 / 스테이지 정보는 "(chapter)-(stage)" 형식
        stageNumObject = GameObject.Find("StageNum");
        chapterStageNum = stageNumObject.GetComponent<StageManager>().ChapterStageNum;
        string[] temp = chapterStageNum.Split('-');
        int stageNum = int.Parse(temp[1]);

        //스테이지에 맞는 포지션에 플레이어 스폰
        GameObject pos = GameObject.Find("stage" + stageNum);
        rigid.position = pos.transform.position;

        Destroy(stageNumObject);
    }
    void Update() {
        if (Input.GetButtonDown("Jump")) {
            Jump();
        }
        
        if (Input.GetButtonUp("Sit") || !Input.GetButton("Sit")) {
            animator.SetTrigger("sitTrigger");
        }
    }
    void FixedUpdate() {
        h = Input.GetAxisRaw("Horizontal");

        EtcMove();

        Move();
        SlopeCheck();
        GroundCheck();
        ColliderChange();
        Sit();
    }

    void Move() {
        //Move
        if (!isSlope)
            rigid.velocity = new Vector2(maxSpeed * h, rigid.velocity.y);
        else if (isSlope && !isJumping)
            rigid.velocity = new Vector2(maxSpeed * slopeNormalPerp.x * -h, maxSpeed * slopeNormalPerp.y * -h);
        //기어가고 있을 때 속도
        if(animator.GetBool("isSit") && animator.GetBool("isWalk"))
            rigid.velocity = new Vector2(crawlSpeed * h, 0);
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
    void Sit() {
        if (Input.GetButton("Sit")) {
            animator.SetBool("isSit", true);
        }
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("SitUp")
          && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f)
            animator.SetBool("isSit", false);

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("SitDown")
           && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f)
            animator.SetTrigger("sitTrigger");
    }
    void EtcMove() {
        //flipX and friction and walk animation
        if (Input.GetButton("Horizontal")) {
            rigid.sharedMaterial = noFriction;
            spriteRenderer.flipX = h == -1;
            animator.SetBool("isWalk", true);
        }
        else {
            rigid.sharedMaterial = fullFriction;
            animator.SetBool("isWalk", false);
        }

        //점프 값 변경
        animator.SetBool("isJump", isJumping);
        animator.SetFloat("jumpValue", rigid.velocity.y);

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("jumpStart")
                 && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f)
            animator.SetTrigger("jumpTrigger");

        animator.SetBool("isGround", isGround);

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("jumpEnd")
             && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f)
            animator.SetTrigger("jumpTrigger");
    }
    void ColliderChange() {
        // 점프 착지 할때
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("jumpEnd")
             && animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.9f)
            capsule.size = new Vector2(1, 2.6f);
        // 앉기 풀고 일어나는 도중
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("SitUp")
            && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.5f)
            capsule.size = new Vector2(1, 2.85f);
        // 앉을때
        else if (animator.GetBool("isSit"))
            capsule.size = new Vector2(1, 2.2f);
        else
            capsule.size = new Vector2(1, 2.85f);
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
