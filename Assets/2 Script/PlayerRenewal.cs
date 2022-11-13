using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerRenewal : MonoBehaviour
{
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
    [SerializeField]
    private AudioClip jumpStep;

    public string chapterStageNum;
    //public string chapterNum { 
    //    get { return chapterStageNum[0]; }
    //    set { value = chapterStageNum[0]; }
    //}
    public float MaxSpeed
    {
        get { return applySpeed; }
    }

    private float h;
    public float H
    {
        get { return h; }
    }
    private float slopeDownAngle;
    private float slopeDownAngleOld;
    private float applySpeed;
    public float ApplySpeed
    {
        get { return applySpeed; }
    }
    public float abilityCurGauge;
    public float abilityMaxGauge;
    float windPower;
    float horrorDurationCurTime;
    float horrorCoolCurTime;
    [SerializeField]
    float batPoopMaxTime;
    float batPoopCurTime;
    float batPoopJumpPower;
    float batPoopSpeed;
    [HideInInspector]
    public float giantDebuffSpeed;

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
    public bool playerWalk
    {
        get { return animator.GetBool("isWalk"); }
    }

    private bool sadD;
    private bool sading;
    private bool horrorD;
    private bool angryD;
    private bool angrying;
    private static bool horroring;
    public static bool Horroring
    {
        get { return horroring; }
    }
    public bool isSuperJump;
    private bool test;
    private bool windBlowing;
    public bool FlipX
    {
        get { return spriteRenderer.flipX; }
    }

    private Vector2 slopeNormalPerp;
    private Vector2 colliderSize;
    private Vector2 colliderOffset;
    private Vector2 plusVelocity;

    private GameObject stageNumObject;

    IEnumerator idleDelay;

    Image horrorTimeImg;
    Text horrorTimeTxt;

    //component
    Rigidbody2D rigid;
    CapsuleCollider2D capsule;
    SpriteRenderer spriteRenderer;
    public Animator animator;
    AudioSource audioSource;

    public Animator Anim
    {
        get { return animator; }
    }
    public Rigidbody2D Rigid
    {
        get { return rigid; }
    }

    int chapter, stage = 0;

    void Awake()
    {
        Application.targetFrameRate = 60;
        //SoundManager.instance.audioFoot = GetComponent<AudioSource>();

        rigid = GetComponent<Rigidbody2D>();
        capsule = GetComponent<CapsuleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        colliderSize = capsule.size;
        colliderOffset = capsule.offset;

        if (SceneManager.GetActiveScene().buildIndex == 1)
        {

            youCanJump = false;
            youCanCrawl = false;
        }
        else
        {
            youCanJump = true;
            youCanCrawl = true;
        }
        jumpEnd = true;

        applySpeed = walkSpeed;
        abilityMaxGauge = 5;

        horrorCoolCurTime = 0;
        if (SceneManager.GetActiveScene().name != "1 Prologue 1" && SceneManager.GetActiveScene().name != "2 House" && SceneManager.GetActiveScene().name != "3 Prologue 2")
        {
            horrorTimeImg = GameObject.Find("HorrorTimeImg").GetComponent<Image>();
            horrorTimeImg.fillAmount = 0;
            horrorTimeTxt = GameObject.Find("HorrorTimeTxt").GetComponent<Text>();
            horrorTimeTxt.text = "";
        }
    }
    void Start()
    {
        //씬 변경 후 스테이지 정보 가져오기 / 스테이지 정보는 "(chapter)-(stage)" 형식
        stageNumObject = GameObject.Find("StageNum");
        if (stageNumObject == null)
        {
            Debug.Log("chapter stage number not find");
        }
        else
        {
            chapterStageNum = stageNumObject.GetComponent<StageManager>().ChapterStageNum;
            //int stageNum = int.Parse(chapterStageNum[1]);

            //스테이지에 맞는 포지션에 플레이어 스폰
            GameObject pos = GameObject.Find(chapterStageNum);
            rigid.position = pos.transform.position;

            chapter = int.Parse(chapterStageNum.Split('-')[0]);
            stage = int.Parse(chapterStageNum.Split('-')[1]);
        }

        GameObject.Find("StageManager").GetComponent<StageManager>().ChapterStageNum = chapterStageNum;

        if (chapter == 0)
        {
            if (stage >= 1)
            {
                youCanJump = true;
            }
            if (stage >= 3)
            {
                youCanCrawl = true;
            }
        }
        if (chapter == 2)
        {
            if (stage == 8)
            {
                GetComponent<PlayerVsGiant>().Init();
            }
        }
        Destroy(stageNumObject);
    }
    void Update()
    {
        if (Input.GetButton("Jump"))
        {
            Jump();
        }
        else if (Input.GetButtonUp("Jump"))
        {
            animator.SetBool("isJumpPress", false);
        }

        Sit();
        SkillDownCheck();
        HorrorSkill();

        SkillAnimation();
        SitObjectPass();

        // 공포 지속시간
        if (horroring)
        {
            horrorDurationCurTime -= Time.deltaTime;
            if (horrorDurationCurTime < 0.0f)
            {
                horrorDurationCurTime = 0;
                horroring = false;
                animator.SetTrigger("HorrorEndTrigger");
            }
        }
        else
        {
            horrorCoolCurTime -= Time.deltaTime;
        }

        // 박쥐 똥
        if (batPoopCurTime < batPoopMaxTime)
        {
            batPoopCurTime += Time.deltaTime;
        }
        else
        {
            batPoopJumpPower = 0;
            batPoopSpeed = 0;
        }
    }
    void FixedUpdate()
    {
        Debug.DrawRay(transform.position + Vector3.up * 1.5f + Vector3.right * 0.2f, Vector3.up * 1.4f, Color.red);
        setHorizontal();
        EtcMove();
        PlusVelocity();

        Move();
        SlopeCheck();
        GroundCheck();
        ColliderChange();
        Idle();
    }
    void LateUpdate()
    {
        if (SceneManager.GetActiveScene().name != "1 Prologue 1" && SceneManager.GetActiveScene().name != "2 House" && SceneManager.GetActiveScene().name != "3 Prologue 2")
        {
            if (horroring)
            {
                horrorTimeImg.color = Color.white;
                horrorTimeImg.fillAmount = horrorDurationCurTime / 3;
                if (horrorDurationCurTime >= 1.0f)
                    horrorTimeTxt.text = $"{horrorDurationCurTime:N0}s";
                else
                    horrorTimeTxt.text = $"{horrorDurationCurTime:N1}s";
            }
            else if (!horroring && horrorCoolCurTime > 0)
            {
                horrorTimeImg.color = Color.black;
                horrorTimeImg.fillAmount = horrorCoolCurTime / 5;
                if (horrorCoolCurTime >= 1.0f)
                    horrorTimeTxt.text = $"{horrorCoolCurTime:N0}s";
                else
                    horrorTimeTxt.text = $"{horrorCoolCurTime:N1}s";
            }
            else if (!horroring && horrorCoolCurTime < 0)
            {
                horrorTimeTxt.text = "";
            }
        }
    }
    void setHorizontal()
    {
        if (isSitUp)
            return;
        h = Input.GetAxisRaw("Horizontal");
    }
    void SkillDownCheck()
    {
        //점프 제한
        if (!canJump || isJumping || !jumpEnd)
            return;
        //앉기 제한
        if (animator.GetBool("isSit"))
            return;
        if (dontInput)
        {
            angryD = false;
            sadD = false;
        }
        // 플레이어능력이 켜져있나? ( 아니면 요정능력이 켜져있는 상태 )
        // 챕터(scene)에 맞게 능력을 써지게(임시)
        if (GameManager.manager.playerAbilityOn)
        {
            if(SceneManager.GetActiveScene().buildIndex == 2)
            {
                if (!sading)
                {
                    sadD = Input.GetButton("Sad");
                }
            }

            if (SceneManager.GetActiveScene().buildIndex == 3)
            {
                if (!sading)
                {
                    sadD = Input.GetButton("Sad");
                }

                if (!horroring)
                {
                    //Debug.Log(horrorCoolCurTime);
                    if (horrorCoolCurTime <= 0.0f)
                    {
                        horrorD = Input.GetButton("Horror");
                    }
                }
            }

            if (SceneManager.GetActiveScene().buildIndex == 8)
            {
                if (!sading)
                {
                    sadD = Input.GetButton("Sad");
                }

                if (!horroring)
                {
                    //Debug.Log(horrorCoolCurTime);
                    if (horrorCoolCurTime <= 0.0f)
                    {
                        horrorD = Input.GetButton("Horror");
                    }
                }

                if (!angrying)
                {
                    angryD = Input.GetButton("Angry");
                }
            }
        }

        else
        {
            sadD = false;
            sading = false;
            horrorD = false;
            horroring = false;
            angryD = false;
            angrying = false;
        }
    }
    void SkillAnimation()
    {
        if (sadD && !sading)
        {
            sading = true;
            if (!GameManager.manager.haveSadMask)
                animator.SetTrigger("noMaskSadTrigger");
            else
                animator.SetTrigger("SadTrigger");
            Invoke("sadOut", 2f);
        }
        if (horrorD && !horroring)
        {
            horroring = true;
            animator.SetTrigger("HorrorTrigger");
        }
        if (angryD && !angrying)
        {
            angrying = true;
            if (!GameManager.manager.haveAngryMask)
                animator.SetTrigger("noMaskAngryTrigger");
            else
                animator.SetTrigger("AngryTrigger");
            Invoke("angryOut", 2f);
        }

    }
    void sadOut()
    {
        sading = false;
    }
    void angryOut()
    {
        angrying = false;
    }
    void HorrorSkill()
    {
        if (horrorD)
        {
            horrorCoolCurTime = 5.0f;
            horrorDurationCurTime = 3.0f;
            if (horroring)
                horrorD = false;
        }
    }
    // 기본적인 이동 이외의 다른 요소로 인한 속도 변화 확인
    void PlusVelocity()
    {
        // 플레이어의 바닥이 움직이는 바닥일때
        plusVelocity = Vector2.zero;
        RaycastHit2D[] hits = Physics2D.RaycastAll(new Vector2(transform.position.x + 0.1f, transform.position.y + 1f), Vector2.down, 1f);
        Debug.DrawRay(new Vector2(transform.position.x + 0.1f, transform.position.y + 1f), Vector2.down * 1, Color.magenta);
        if (hits.Length > 0)
        {
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider.GetComponent<Rigidbody2D>() == null)
                    plusVelocity = Vector2.zero;
                if (hit.collider.GetComponent<Rigidbody2D>() != null)
                {
                    Rigidbody2D hitRigid = hit.collider.GetComponent<Rigidbody2D>();
                    plusVelocity += new Vector2(hitRigid.velocity.x, 0);
                    break;
                }
            }
        }
        //오브젝트 "바람" 안에 있을때
        if (windBlowing)
        {
            plusVelocity += Vector2.right * windPower;
        }
    }
    // 플레이어 이동관련
    void Move()
    {
        //Move
        if (dontInput || isSitUp || isSuperJump)
            return;
        //능력 잠금
        if (sading || horroring || angrying)
            return;
        float speed = applySpeed + batPoopSpeed + giantDebuffSpeed > 0 ? applySpeed + batPoopSpeed + giantDebuffSpeed : 0.1f;
        if (!isSlope)
            rigid.velocity = plusVelocity + new Vector2(speed * h, rigid.velocity.y);
        else if (isSlope && !isJumping && isGround)
            rigid.velocity = plusVelocity + new Vector2(speed * slopeNormalPerp.x * -h, applySpeed * slopeNormalPerp.y * -h);
        //기어가고 있을 때 속도
        //if(animator.GetBool("isSit") && animator.GetBool("isWalk"))
        //    rigid.velocity = new Vector2(crawlSpeed * h, 0);
    }
    // 플레이어 점프
    void Jump()
    {
        if (!youCanJump)
            return;
        if (!canJump || dontInput || (!jumpEnd && !animator.GetBool("isJumpPress")))
            return;
        if (sading || horroring || angrying)
            return;
        if (animator.GetBool("isSit"))
            return;

        canJump = false;
        isJumping = true;
        jumpEnd = false;
        //rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
        rigid.velocity = plusVelocity + new Vector2(applySpeed * h, jumpPower + batPoopJumpPower);
        animator.SetBool("isJumpPress", true);
    }

    // 플레이어 바닥이 땅인지 확인
    void GroundCheck()
    {
        isGround = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround | LayerMask.GetMask("Object"));

        if (isGround && animator.GetBool("isWalk") && !audioSource.isPlaying)
        {
            audioSource.clip = footStep;
            audioSource.Play();
        }
        else if (!isGround || !animator.GetBool("isWalk"))
        {
            audioSource.clip = null;
            audioSource.Stop();
        }

        if (rigid.velocity.y <= 0)
            isJumping = false;

        if (isGround && !isJumping)
            canJump = true;

        if (!isGround)
        {
            canJump = false;
            audioSource.Stop();
        }
        if (isGround && isSuperJump && !test)
        {
            isSuperJump = false;
        }
    }
    // 플레이어 앉기
    void Sit()
    {
        if (!youCanCrawl)
            return;
        if (/*animator.GetBool("isWalk") ||*/dontInput)
            return;
        if (sading || horroring || angrying)
            return;
        if (Input.GetButton("Sit")/* && !animator.GetBool("isSit")*/)
        {
            animator.SetBool("isSit", true);
            applySpeed = crawlSpeed;
        }
        //else if (Input.GetButtonUp("Sit")) {
        //    if (!Physics2D.Raycast(transform.position + Vector3.up * 1.5f + Vector3.right * 0.2f, Vector3.up, 1.4f){
        //        applySpeed = walkSpeed;
        //    }
        //}
        // 앉아있는 플레이어 일어나기.
        if ((Input.GetButtonUp("Sit") || !Input.GetButton("Sit")) && !dontInput)
        {
            int layerMask = (1 << 7) + (1 << 9);
            RaycastHit2D hit = Physics2D.Raycast(transform.position + Vector3.up * 1.5f + Vector3.right * 0.2f, Vector3.up, 1.4f, layerMask);
            bool hitCheck = hit;
            if (hit)
            {
                if (hit.transform.GetComponent<Object>())
                {
                    Object obj = hit.transform.GetComponent<Object>();
                    if (obj.IsPassObject)
                        hitCheck = false;
                }
            }
            if (!hitCheck)
            {
                applySpeed = walkSpeed;
                animator.SetTrigger("sitTrigger");
            }
        }
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("SitUp")
          && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.8f)
        {
            animator.SetBool("isSit", false);
            isSitUp = false;
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("SitUp"))
        {
            isSitUp = true;
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("SitDown")
           && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f)
            animator.SetTrigger("sitTrigger");
    }
    // 움직이지 않고, 가만히 있는경우
    void Idle()
    {
        //idle 빌드업
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("stand") && !idleCoroutinePlay)
        {
            idleDelay = IdleDelay();
            StartCoroutine(idleDelay);
        }
        //중간에 움직였을 때
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("stand") && idleCoroutinePlay)
        {
            idleCoroutinePlay = false;
            StopCoroutine(idleDelay);
        }
        //애니메이션 종료
        if (animator.GetBool("isIdle") && !idleCoroutinePlay)
        {
            StartCoroutine(IdleEnd());
        }
    }
    void EtcMove()
    {
        //flipX and friction and walk animation
        if (dontInput)
            return;
        if (sading || horroring || angrying)
            return;

        if (Input.GetButton("Horizontal") && !isSitUp)
        {
            rigid.sharedMaterial = noFriction;
            spriteRenderer.flipX = h == -1;
            animator.SetBool("isWalk", true);
        }
        else if (!Input.GetButton("Horizontal") && isGround)
        {
            rigid.sharedMaterial = fullFriction;
            animator.SetBool("isWalk", false);
        }
        if (windBlowing)
        {
            rigid.sharedMaterial = noFriction;
        }

        //점프 값 변경
        animator.SetBool("isJump", isJumping);
        animator.SetFloat("jumpValue", rigid.velocity.y);

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("jumpStart")
                 && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f)
        {
            animator.SetTrigger("jumpTrigger");
        }

        animator.SetBool("isGround", isGround);

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("jumpEnd")
             && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f)
        {
            animator.SetTrigger("jumpTrigger");
            jumpEnd = true;
        }
    }
    void ColliderChange()
    {
        // 앉을때
        if (animator.GetBool("isSit"))
        {
            capsule.size = new Vector2(1, 1.5f);
            capsule.offset = new Vector2(colliderOffset.x + 0.1f, 0.85f);
        }
        else
        {
            capsule.size = colliderSize;
            capsule.offset = colliderOffset;
        }
    }

    IEnumerator IdleDelay()
    {
        idleCoroutinePlay = true;
        yield return new WaitForSeconds(3f);

        animator.SetBool("isIdle", true);
        idleCoroutinePlay = false;
    }
    IEnumerator IdleEnd()
    {
        idleCoroutinePlay = true;

        yield return new WaitForSeconds(0.1f);

        animator.SetBool("isIdle", false);
        idleCoroutinePlay = false;
    }

    void SlopeCheck()
    {
        // 경사인지 아닌지 체크하는 함수
        Vector2 checkPos = transform.position - new Vector3(0.0f, colliderSize.y / 2) + new Vector3(0.0f, capsule.offset.y);

        SlopeCheckHorizontal(checkPos);
        SlopeCheckVertical(checkPos);
    }
    void SlopeCheckHorizontal(Vector2 checkPos)
    {
        RaycastHit2D slopeHitFront = Physics2D.Raycast(checkPos, transform.right, slopeCheckDistance, whatIsGround);
        RaycastHit2D slopeHitBack = Physics2D.Raycast(checkPos, -transform.right, slopeCheckDistance, whatIsGround);
        // Debug.DrawRay(slopeHitFront.point, slopeHitFront.normal, Color.blue);
        // Debug.DrawRay(slopeHitBack.point, slopeHitBack.normal, Color.magenta);
        if (slopeHitFront)
        {
            isSlope = true;
        }
        else if (slopeHitBack)
        {
            isSlope = true;
        }
        else
        {
            isSlope = false;
        }

        //if (!slopeHitBack && !slopeHitFront)
        //    isSlope = false;
    }
    void SlopeCheckVertical(Vector2 checkPos)
    {
        RaycastHit2D rayHit = Physics2D.Raycast(checkPos, Vector2.down, slopeCheckDistance, whatIsGround);

        if (rayHit)
        {
            slopeNormalPerp = Vector2.Perpendicular(rayHit.normal).normalized;

            slopeDownAngle = Vector2.Angle(rayHit.normal, Vector2.up);

            if (slopeDownAngle != slopeDownAngleOld)
            {
                isSlope = true;
            }
            slopeDownAngleOld = slopeDownAngle;

            //Debug.DrawRay(rayHit.point, slopeNormalPerp, Color.red);
            //Debug.DrawRay(rayHit.point, rayHit.normal, Color.green);
        }
    }
    void SitObjectPass()
    {
        if (animator.GetBool("isSit") && h != 0)
        {
            Debug.DrawRay(transform.position + Vector3.up, Vector2.right * 1f, Color.blue);
            RaycastHit2D hit = Physics2D.Raycast(transform.position + Vector3.up, Vector2.right, FlipX ? -1f : 1f, LayerMask.GetMask("SitPassObject"));

            if (hit)
            {
                SitPassObj sitObj = hit.collider.gameObject.GetComponent<SitPassObj>();
                sitObj.StartCoroutine(sitObj.playerAutoCrawl());
                applySpeed = walkSpeed;
            }
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("CanJump"))
        {
            youCanJump = true;
        }
        if (collision.CompareTag("CanCrawl"))
        {
            youCanCrawl = true;
        }
        if (collision.gameObject.layer == LayerMask.NameToLayer("Obstacle") && !coroutineRun)
        {
            //장애물과 부딪혔을때
            if (!horroring)
            {
                StartCoroutine(Die());
            }
        }
        if (collision.CompareTag("Debuff"))
        {
            if (collision.GetComponent<Debuff>().Type == DebuffType.batPoop)
            {
                // 박쥐 똥 디버프
                batPoopCurTime = 0;
                Debuff debuff = collision.GetComponent<Debuff>();
                batPoopSpeed = debuff.floatValue[0];
                batPoopJumpPower = debuff.floatValue[1];
            }
        }
        if (collision.CompareTag("stageSave"))
        {
            //구간 저장
            sm.ChapterStageNum = collision.gameObject.name;
        }
        //if (collision.gameObject.tag == "nextChapter")
        //{
        //    // 다음 챕터로 이동
        //    GameObject chapterStage = GameObject.Find("StageManager");
        //    chapterStage.name = "StageNum";
        //    SceneManager.LoadScene(int.Parse(sm.ChapterStageNum.Split('-')[0]) + 2);
        //    DontDestroyOnLoad(chapterStage);
        //}
        if (collision.CompareTag("Object"))
        {
            test = true;
            if (!collision.gameObject.GetComponent<Object>())
                return;
            Object obj = collision.gameObject.GetComponent<Object>();
            if (obj.Name == Object.objectName.Wind)
            {
                windBlowing = true;
                windPower += obj.Speed;
                return;
            }
            obj.ObjectAbility(transform, this);
        }
        if (collision.CompareTag("Mask"))
        {
            //마스크 획득
            if (collision.gameObject.GetComponent<Mask>().mask == Mask.MaskType.Sad)
            {
                collision.gameObject.GetComponent<Mask>().MaskEvent(this);
                abilityCurGauge = abilityMaxGauge;
            }
            else if (collision.gameObject.GetComponent<Mask>().mask == Mask.MaskType.Horror)
            {
                collision.gameObject.GetComponent<Mask>().MaskEvent(this);
                abilityCurGauge = abilityMaxGauge;
            }
            else if (collision.gameObject.GetComponent<Mask>().mask == Mask.MaskType.Angry)
            {
                collision.gameObject.GetComponent<Mask>().MaskEvent(this);
                abilityCurGauge = abilityMaxGauge;
            }
            else if (collision.gameObject.GetComponent<Mask>().mask == Mask.MaskType.Happy)
            {
                collision.gameObject.GetComponent<Mask>().MaskEvent(this);
                abilityCurGauge = abilityMaxGauge;
            }
        }
        if (collision.CompareTag("Warp") && collision.GetComponent<Warp_CircleEffect>())
        {
            StartCoroutine(collision.GetComponent<Warp_CircleEffect>().Warp());
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Object"))
        {
            if (!collision.GetComponent<Object>())
                return;
            Object obj = collision.gameObject.GetComponent<Object>();
            // 바람에서 벗어날 때, 이동속도는 다시 정상으로
            if (obj.Name == Object.objectName.Wind)
            {
                windBlowing = false;
                windPower = 0;
                return;
            }
        }
    }
    public void Testtest()
    {
        Invoke("testFalse", 1);
    }
    void testFalse()
    {
        test = false;
    }
    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Skill"))
        {
            if (sadD)
            {
                if (collision.gameObject.GetComponent<Object>() != null)
                {
                    collision.gameObject.GetComponent<Object>().Skill();
                    sadD = false;
                }
                else if (collision.gameObject.GetComponent<Chipmunk>() != null)
                {
                    collision.gameObject.GetComponent<Chipmunk>().CutScene(this);
                }
            }

            if(angryD)
            {
                if (collision.gameObject.GetComponent<Object>() != null)
                {
                    collision.gameObject.GetComponent<Object>().Skill();
                    angryD = false;
                }
            }
        }
    }
    public IEnumerator Die()
    {
        coroutineRun = true;
        animator.SetTrigger("dieTrigger");
        dontInput = true;
        yield return new WaitForSeconds(2f);

        //GameObject chapterStage = GameObject.Find("StageManager");
        //chapterStage.name = "StageNum";
        GameObject chapterStage = GameObject.Find(sm.ChapterStageNum);
        gameObject.transform.position = chapterStage.transform.position;
        //SceneManager.LoadScene(int.Parse(sm.ChapterStageNum.Split('-')[0]) + 1);
        //DontDestroyOnLoad(chapterStage);
        Anim.SetTrigger("resTrigger");
        coroutineRun = false;
        jumpEnd = true;
        dontInput = false;

    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
