using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerRenewal : MonoBehaviour {
    [SerializeField]
    private StageManager sm;
    [SerializeField]
    private float walkSpeed;
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
    [SerializeField]
    private AudioClip footStep;

    public string chapterStageNum;
    //public string chapterNum { 
    //    get { return chapterStageNum[0]; }
    //    set { value = chapterStageNum[0]; }
    //}
    public float MaxSpeed {
        get { return applySpeed; }
    }

    private float h;
    public float H {
        get { return h; }
    }
    private float slopeDownAngle;
    private float slopeDownAngleOld;
    private float applySpeed;

    private bool isSlope;
    private bool canJump;
    private bool isGround;
    private bool isJumping;
    private bool youCanJump;
    private bool youCanCrawl;
    private bool idleCoroutinePlay;
    public bool dontInput;
    private bool jumpEnd;
    private bool isSitUp;
    private bool coroutineRun;


    private Vector2 slopeNormalPerp;
    private Vector2 colliderSize;

    private GameObject stageNumObject;

    IEnumerator idleDelay;
    IEnumerator dieCoroutine;

    //component
    Rigidbody2D rigid;
    CapsuleCollider2D capsule;
    SpriteRenderer spriteRenderer;
    Animator animator;
    AudioSource audioSource;

    int chapter, stage = 0;

    void Awake() {
        rigid = GetComponent<Rigidbody2D>();
        capsule = GetComponent<CapsuleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        colliderSize = capsule.size;

        if (SceneManager.GetActiveScene().buildIndex == 1) {

            youCanJump = true;
            youCanCrawl = true;
        }
        else {
            youCanJump = true;
            youCanCrawl = true;
        }
        jumpEnd = true;

        dieCoroutine = Die();
        applySpeed = walkSpeed;
    }
    void Start() {
        //씬 변경 후 스테이지 정보 가져오기 / 스테이지 정보는 "(chapter)-(stage)" 형식
        stageNumObject = GameObject.Find("StageNum");
        //Debug.Log(stageNumObject.name);
        if (stageNumObject == null) {
            Debug.Log("chapter stage number not find");
        }
        else {
            chapterStageNum = stageNumObject.GetComponent<StageManager>().ChapterStageNum;
            //int stageNum = int.Parse(chapterStageNum[1]);

            //스테이지에 맞는 포지션에 플레이어 스폰
            GameObject pos = GameObject.Find(chapterStageNum);
            rigid.position = pos.transform.position;

            chapter = int.Parse(chapterStageNum.Split('-')[0]);
            stage = int.Parse(chapterStageNum.Split('-')[1]);
        }

        GameObject.Find("StageManager").GetComponent<StageManager>().ChapterStageNum = chapterStageNum;

        if (chapter == 0) {
            if (stage >= 1) {
                youCanJump = true;
            }
            if (stage >= 3) {
                youCanCrawl = true;
            }
        }
        Destroy(stageNumObject);
    }
    void Update() {
        if (Input.GetButton("Jump")) {
            Jump();
        }
        else if (Input.GetButtonUp("Jump")) {
            animator.SetBool("isJumpPress", false);
        }
        
        if ((Input.GetButtonUp("Sit") || !Input.GetButton("Sit")) && !dontInput) {
            animator.SetTrigger("sitTrigger");
        }
        Sit();
    }
    void FixedUpdate() {
        setHorizontal();
        EtcMove();

        Move();
        SlopeCheck();
        GroundCheck();
        ColliderChange();
        Idle();
    }
    void setHorizontal() {
        if (isSitUp)
            return;
        h = Input.GetAxisRaw("Horizontal");
    }
    void Move() {
        //Move
        if (dontInput || isSitUp)
            return;

        if (!isSlope)
            rigid.velocity = new Vector2(applySpeed * h, rigid.velocity.y);
        else if (isSlope && !isJumping)
            rigid.velocity = new Vector2(applySpeed * slopeNormalPerp.x * -h, applySpeed * slopeNormalPerp.y * -h);
        //기어가고 있을 때 속도
        //if(animator.GetBool("isSit") && animator.GetBool("isWalk"))
        //    rigid.velocity = new Vector2(crawlSpeed * h, 0);
    }
    void Jump() {
        if (!canJump || !youCanJump || dontInput || (!jumpEnd && !animator.GetBool("isJumpPress")))
            return;

        canJump = false;
        isJumping = true;
        jumpEnd = false;
        //rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
        rigid.velocity = new Vector2(applySpeed * h, jumpPower);
        animator.SetBool("isJumpPress", true);
    }
    void GroundCheck() {
        isGround = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);

        if (isGround && animator.GetBool("isWalk") && !audioSource.isPlaying) {
            audioSource.clip = footStep;
            audioSource.Play();
        }
        else if (!isGround || !animator.GetBool("isWalk")) {
            audioSource.clip = null;
            audioSource.Stop();
        }

        if (rigid.velocity.y <= 0)
            isJumping = false;

        if (isGround && !isJumping)
            canJump = true;

        if (!isGround) {
            canJump = false;
            audioSource.Stop();
        }
    }
    void Sit() {
        if (/*animator.GetBool("isWalk") ||*/ !youCanCrawl || dontInput)
            return;
        if (Input.GetButton("Sit")/* && !animator.GetBool("isSit")*/) {
            animator.SetBool("isSit", true);
            applySpeed = crawlSpeed;
        }
        else if (Input.GetButtonUp("Sit")) {
            applySpeed = walkSpeed;
        }
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("SitUp")
          && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.8f) {
            animator.SetBool("isSit", false);
            isSitUp = false;
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("SitUp")){
            isSitUp = true;
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("SitDown")
           && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f)
            animator.SetTrigger("sitTrigger");
    }
    void Idle() {
        //idle 빌드업
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("stand") && !idleCoroutinePlay) {
            idleDelay = IdleDelay();
            StartCoroutine(idleDelay);
        }
        //중간에 움직였을 때
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("stand") && idleCoroutinePlay) {
            idleCoroutinePlay = false;
            StopCoroutine(idleDelay);
        }
        //애니메이션 종료
        if (animator.GetBool("isIdle") && !idleCoroutinePlay) {
            StartCoroutine(IdleEnd());
        }
    }
    void EtcMove() {
        //flipX and friction and walk animation
        if (dontInput)
            return;

        if (Input.GetButton("Horizontal") && !isSitUp) {
            rigid.sharedMaterial = noFriction;
            spriteRenderer.flipX = h == -1;
            animator.SetBool("isWalk", true);
        }
        else if(!Input.GetButton("Horizontal")) {
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
             && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f) { 
            animator.SetTrigger("jumpTrigger");
            jumpEnd = true;
        }
    }
    void ColliderChange() {
        // 앉을때
        if (animator.GetBool("isSit")) {
            capsule.size = new Vector2(1, 1.5f);
            capsule.offset = new Vector2(capsule.offset.x, 0.85f);
        }
        else {
            capsule.size = new Vector2(1, 2.85f);
            capsule.offset = new Vector2(0, 1.5f);
        }
    }

    IEnumerator IdleDelay() {
        idleCoroutinePlay = true;
        yield return new WaitForSeconds(3f);

        animator.SetBool("isIdle", true);
        idleCoroutinePlay = false;
    }
    IEnumerator IdleEnd() {
        idleCoroutinePlay = true;

        yield return new WaitForSeconds(0.1f);

        animator.SetBool("isIdle", false);
        idleCoroutinePlay = false;
    }

    void SlopeCheck() {
        // 경사인지 아닌지 체크하는 함수
        Vector2 checkPos = transform.position - new Vector3(0.0f, colliderSize.y / 2) + new Vector3(0.0f, capsule.offset.y);

        SlopeCheckHorizontal(checkPos);
        SlopeCheckVertical(checkPos);
    }
    void SlopeCheckHorizontal(Vector2 checkPos) {
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

            //Debug.DrawRay(rayHit.point, slopeNormalPerp, Color.red);
            //Debug.DrawRay(rayHit.point, rayHit.normal, Color.green);
        }
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if(collision.tag == "CanJump") {
            youCanJump = true;
        }
        if(collision.tag == "CanCrawl") {
            youCanCrawl = true;
        }
        if (collision.gameObject.layer == LayerMask.NameToLayer("Obstacle") && !coroutineRun) {
            //장애물과 부딪혔을때
            animator.SetTrigger("dieTrigger");
            StartCoroutine(dieCoroutine);
        }
        if (collision.gameObject.tag == "stageSave") {
            //구간 저장
            sm.ChapterStageNum = collision.gameObject.name;
        }
        if (collision.gameObject.tag == "nextChapter") {
            //다음 챕터로 이동
            GameObject chapterStage = GameObject.Find("StageManager");
            chapterStage.name = "StageNum";
            SceneManager.LoadScene(int.Parse(sm.ChapterStageNum.Split('-')[0]) + 2);
            DontDestroyOnLoad(chapterStage);
        }

    }
    IEnumerator Die() {
        coroutineRun = true;
        dontInput = true;
        yield return new WaitForSeconds(2f);
        coroutineRun = false;

        GameObject chapterStage = GameObject.Find("StageManager");
        chapterStage.name = "StageNum";
        SceneManager.LoadScene(int.Parse(sm.ChapterStageNum.Split('-')[0]) + 1);
        DontDestroyOnLoad(chapterStage);
    }

    private void OnDrawGizmos() {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
