using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Giant : MonoBehaviour {
    enum GiantState { Stop, Move, Search }
    [SerializeField]
    GiantState state;
    /* 
     * 1순위. 공간Space 변할 시 그곳으로 이동 
     * (공간으로 할지? 플레이어 시야 내에서 할지?)
     * 2순위. 왼쪽2~3s 이동 - 0.5s후 돌아보기 - 2초 유지 - 오른쪽으로 2~3s 이동 - 0.5s후 돌아보기
     */

    int moveSpace;

    [SerializeField]
    float maxSpeed;
    float curSpeed;

    float patternTime;

    // 움직임
    float[] maxPosX = new float[2];
    float walkTime;
    bool isLeft = true;
    Transform eyeLight;
    int moveCount;
    // 이동 중 플레이어 발견
    bool isDiscovery;
    float lastDiscoveryPos;
    public bool isAtkHand;

    // 벽 뒤 확인
    public Transform playerHidePillar = null;
    float searchingTime;
    bool isScan;
    bool scanStart;
    [SerializeField, Tooltip("기둥 잡는 손")]
    GameObject HoldingPillarHand;
    public bool holdinPillarHandOut;
    [SerializeField, Tooltip("플레이어 확인공격 하는 손")]
    GameObject scanAtkFist;
    public bool scanMove;
    bool scanMoveStart;
    bool scanAtk;
    public bool scanAtkPunch;
    bool scanAtkPunchFirst;
    int StopCnt;

    // 마지막 추격전
    bool isLast;
    float lastSpeed;

    [SerializeField]
    Text tempTxt;

    // 컴포넌트
    Rigidbody2D rigid;
    SpriteRenderer renderer;
    Animator anim;
    AudioSource audio;

    // 플레이어 관련 변수
    PlayerVsGiant player;
    Transform playerTr;

    void Awake() {
        state = GiantState.Move;
        rigid = GetComponent<Rigidbody2D>();
        renderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();

        maxPosX[0] = 568;
        maxPosX[1] = 660;
        patternTime = 0;
        walkTime = 4;
        moveCount = 0;
        searchingTime = 99f;

        //eyeLight = transform.GetChild(0);

        player = GameObject.Find("player").GetComponent<PlayerVsGiant>();
        playerTr = GameObject.Find("player").transform;
    }
    void Update() {
        renderer.flipX = !isLeft;
        if (isLast)
        {
            // 마지막 추격전
            Move();
            Debug.Log("마지막 추격전");
            float dis = player.transform.position.x - transform.position.x > 0 ? player.transform.position.x - transform.position.x : (player.transform.position.x - transform.position.x) * -1;
            
            if(dis < 15 && isDiscovery)
            {
                Attack(dis);
            }
            AttackGiantHandEnable();
        }
        if (!player.IsStart)
            return;
        patternTime += Time.deltaTime;
        AttackGiantHandEnable();

        StateUpdate();
        PosXLimit();
        if (scanMove) {
            if (scanMoveStart) {
                scanMoveStart = false;
                if (isLeft) {
                    transform.position += Vector3.left * 4f;
                }
                else {
                    transform.position += Vector3.right * 4f;
                }
            }
        }
        if (scanAtkPunch) {
            // 기둥 뒤 확인 후 공격
            if (scanAtkPunchFirst) {
                scanAtkPunchFirst = false;
                if (isLeft) {
                    scanAtkFist.transform.localPosition = new Vector2(-3.3f, 3.6f);
                }
                else {
                    scanAtkFist.transform.localPosition = new Vector2(3.3f, 3.6f);
                }
                scanAtkFist.GetComponent<SpriteRenderer>().flipX = renderer.flipX;
                scanAtkFist.SetActive(true);
                StartCoroutine(PlayerDie());
            }
        }
        else {
            scanAtkFist.SetActive(false);

        }
        if(!(anim.GetCurrentAnimatorStateInfo(0).IsName("Scan") || anim.GetCurrentAnimatorStateInfo(0).IsName("ScanCancel") || anim.GetCurrentAnimatorStateInfo(0).IsName("ScanAttack"))) { 
            HoldingPillarHand.SetActive(false);
        }
        else {

            HoldingPillarHand.SetActive(true);
        }
        if ((anim.GetCurrentAnimatorStateInfo(0).IsName("ScanCancel") || anim.GetCurrentAnimatorStateInfo(0).IsName("ScanAttack")) && anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.95f) {
            state = GiantState.Stop;
            scanStart = false;
            Debug.Log("스캔 공격 후 스탑");
        }
        //eyeLight.localEulerAngles = isLeft ? Vector3.forward * 90 : Vector3.forward * -90;

        // 가속
        if (1 > curSpeed && state == GiantState.Move) {
            curSpeed += Time.deltaTime * 3;
        }
        if (audio.time > 3.2f) {
            audio.Stop();
        }
    }
    void FixedUpdate() {
        if (!player.IsStart)
            return;
        FixedStateUpdate();
    }
    void PosXLimit() {
        if (transform.position.x <= maxPosX[0]) {
            isLeft = false;
        }
        else if (transform.position.x >= maxPosX[1]) {
            isLeft = true;
        }
    }
    void StateUpdate() {
        switch (state) {
            case GiantState.Stop:
                anim.SetBool("isMove", false);
                if (1 < patternTime) {
                    if (transform.position.x > playerTr.position.x)
                        isLeft = true;
                    else
                        isLeft = false;
                    patternTime = 0;
                    StopCnt++;
                    //int rand = Random.Range(1, 4);
                    state = GiantState.Move;
                }
                break;
            case GiantState.Move:
                anim.SetBool("isMove", true);
                Move();
                if (isDiscovery) {
                    // 발견했다면? 마지막 발견 위치까지 걸어가지
                    float dis = lastDiscoveryPos - transform.position.x > 0 ? lastDiscoveryPos - transform.position.x : (lastDiscoveryPos - transform.position.x) * -1;
                    //Debug.Log("거리 ? " + dis);
                    if (dis < 15) {
                        Attack(dis);
                    }
                }
                //
                if (isLeft) {
                    if (transform.position.x > playerTr.position.x && !player.IsHide) {
                        Debug.Log("플레이어 걸림");
                        isDiscovery = true;
                        lastDiscoveryPos = player.transform.position.x;
                        player.GiantApproaching();
                    }
                }
                else {
                    if (transform.position.x < playerTr.position.x && !player.IsHide) {
                        Debug.Log("플레이어 걸림");
                        isDiscovery = true;
                        lastDiscoveryPos = player.transform.position.x;
                        player.GiantApproaching();
                    }
                }
                // State.Move 끄는거 MoveLerp()에 있음
                break;
            case GiantState.Search:
                if(patternTime > 1) {
                    if (scanAtk)
                        return;
                    // 플레이어가 있는지 지금부터 검사
                    Debug.Log("검사 중");
                    float dis = player.transform.position.x - transform.position.x > 0 ? player.transform.position.x - transform.position.x : (player.transform.position.x - transform.position.x) * -1;
                    if (dis < 10) {
                        // 공포 능력을 쓰지 않았다면
                        if (!PlayerRenewal.Horroring) {
                            scanAtk = true;
                            anim.SetBool("isScan", true);
                            scanAtkPunchFirst = true;
                        }
                    }
                }
                if (999 < patternTime) {
                    patternTime = 0;
                    StopCnt = 0;
                    state = GiantState.Move;
                }
                break;
        }
    }
    void Attack(float dis)
    {
        // 플레이어가 기둥 뒤에 없다면 공격
        if (!player.IsHide)
        {
            anim.SetTrigger("AtkTrigger");
            patternTime = -4;
            state = GiantState.Stop;
            // 0.9166666666초 후에 플레이어 사망 처리
            //StartCoroutine(PlayerDie());
            isDiscovery = false;
        }
        // 플레이어가 기둥 뒤에 있어서 숨었다면 기둥 뒤 확인
        else if (playerHidePillar)
        {
            dis = playerHidePillar.position.x - transform.position.x > 0 ? playerHidePillar.position.x - transform.position.x : (playerHidePillar.position.x - transform.position.x) * -1;
            Debug.Log("기둥 뒤 확인 : " + dis);
            if (dis < 6.1f)
            {
                // 기둥 뒤 확인
                if (!scanStart)
                {
                    state = GiantState.Search;
                    patternTime = 0;
                    scanAtk = false;
                    anim.SetBool("isScan", false);
                    anim.SetTrigger("ScanAtkTrigger");
                    if (isLeft)
                    {
                        HoldingPillarHand.transform.position = new Vector3(playerHidePillar.position.x - 2.6f, -1.5f);
                    }
                    else
                    {
                        HoldingPillarHand.transform.position = new Vector3(playerHidePillar.position.x + 2.5f, -1.5f);
                    }
                    HoldingPillarHand.GetComponent<SpriteRenderer>().flipX = renderer.flipX;
                    HoldingPillarHand.SetActive(true);
                    StopAllCoroutines();
                    scanStart = true;
                    scanMoveStart = true;
                }
                isDiscovery = false;
            }
        }
    }
    void AttackGiantHandEnable() {
        if (isAtkHand) {
            transform.GetChild(0).GetComponent<SpriteRenderer>().flipX = renderer.flipX;
            StartCoroutine(PlayerDie());
            transform.GetChild(0).localPosition = renderer.flipX ? new Vector2(3.16f, 2.46f) : new Vector2(-3.16f, 2.46f);
            transform.GetChild(0).gameObject.SetActive(true);
        }
        else {
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }
    IEnumerator PlayerDie() {
        // @@ 테스트용 빼기
        //yield return new WaitForSeconds(0.9166f);
        player.PlayerDie();
        //tempTxt.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        // 플레이어가 죽으면, 거인의 위치는 초기 위치로 초기화
        if (isLast)
        {
            gameObject.SetActive(false);
        }
        else
        {
            transform.position = new Vector2(620, -9);
        }
    }
    public void EnvironmentPlayerDie()
    {
        StartCoroutine(Enum_EnvironmentPlayerDie());
    }
    IEnumerator Enum_EnvironmentPlayerDie()
    {
        yield return new WaitForSeconds(2f);
        isDiscovery = false;
        patternTime = 0;
        state = GiantState.Stop;
        transform.position = new Vector2(620, -9);
        StopAllCoroutines();
        if (isLast)
            gameObject.SetActive(false);
    }
    void FixedStateUpdate() {
        switch (state) {
            case GiantState.Stop:
                Debug.Log("정지");
                break;
            case GiantState.Move:
                break;
            case GiantState.Search:
                Debug.Log("플레이어 서칭");
                break;
        }
    }
    void Move() {
        if (isLeft) {
            Movement();
            //rigid.velocity = Vector3.left * curSpeed * maxSpeed;
        }
        else {
            Movement();
            //rigid.velocity = Vector3.right * curSpeed * maxSpeed;
        }
    }
    void Movement() {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Move") && (anim.GetCurrentAnimatorStateInfo(0).normalizedTime % 1 >= 0.25f && anim.GetCurrentAnimatorStateInfo(0).normalizedTime % 1 <= 0.49f)) {
            if (moveCount == 0) {
                moveCount = 1;
                StartCoroutine(MoveLerp(true));
                //transform.position = transform.position + (Vector3.left * 1);
            }
        }
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Move") && (anim.GetCurrentAnimatorStateInfo(0).normalizedTime % 1 >= 0.5f && anim.GetCurrentAnimatorStateInfo(0).normalizedTime % 1 <= 0.74f)) {
            if (moveCount == 1) {
                moveCount = 0;
                StartCoroutine(MoveLerp(false));
                //transform.position = transform.position + (Vector3.left * 1);
            }
        }
    }
    IEnumerator MoveLerp(bool up) {
        Vector2 originPos = transform.position;
        float progress = 0;
        bool shake = false;
        while (progress < 1) {
            progress += 0.2f;
            if (!isDiscovery)
            {
                if (isLeft)
                {
                    if (up)
                        transform.position = Vector2.Lerp(originPos, originPos + (Vector2.left * 1) /*+ (Vector2.up * 0.5f)*/, progress);
                    else
                    {
                        transform.position = Vector2.Lerp(originPos, originPos + (Vector2.left * 1)/* + (Vector2.down * 0.5f)*/, progress);
                        if (progress >= 0.7f && !shake)
                        {
                            shake = true;
                            CameraShake.instance.ShakeCoroutine();
                            audio.Play();
                            audio.time = 2.5f;
                        }
                    }
                }
                else
                {
                    if (up)
                        transform.position = Vector2.Lerp(originPos, originPos + (Vector2.right * 1)/* + (Vector2.up * 0.5f)*/, progress);
                    else
                    {
                        transform.position = Vector2.Lerp(originPos, originPos + (Vector2.right * 1)/* + (Vector2.down * 0.5f)*/, progress);
                        if (progress >= 0.7f && !shake)
                        {
                            shake = true;
                            CameraShake.instance.ShakeCoroutine();
                            audio.Play();
                            audio.time = 2.5f;
                        }
                    }
                }
            }
            else
            {
                Debug.Log("움직임");
                if (isLeft)
                {
                    if (up)
                    {
                        if(isLast)
                            transform.position = Vector2.Lerp(originPos, originPos + (Vector2.left * 2.5f) /*+ (Vector2.up * 0.5f)*/, progress);
                        else
                            transform.position = Vector2.Lerp(originPos, originPos + (Vector2.left * 2) /*+ (Vector2.up * 0.5f)*/, progress);
                    }
                    else
                    {
                        if(isLast)
                            transform.position = Vector2.Lerp(originPos, originPos + (Vector2.left * 2.5f) /*+ (Vector2.up * 0.5f)*/, progress);
                        else
                            transform.position = Vector2.Lerp(originPos, originPos + (Vector2.left * 2)/* + (Vector2.down * 0.5f)*/, progress);
                        if (progress >= 0.7f && !shake)
                        {
                            shake = true;
                            CameraShake.instance.ShakeCoroutine();
                            audio.Play();
                            audio.time = 2.5f;
                        }
                    }
                }
                else
                {
                    if (up)
                    {
                        if(isLast)
                            transform.position = Vector2.Lerp(originPos, originPos + (Vector2.right * 2.5f)/* + (Vector2.up * 0.5f)*/, progress);
                        else
                            transform.position = Vector2.Lerp(originPos, originPos + (Vector2.right * 2)/* + (Vector2.up * 0.5f)*/, progress);
                    }
                    else
                    {
                        if(isLast)
                            transform.position = Vector2.Lerp(originPos, originPos + (Vector2.right * 2.5f)/* + (Vector2.up * 0.5f)*/, progress);
                        else
                            transform.position = Vector2.Lerp(originPos, originPos + (Vector2.right * 2)/* + (Vector2.down * 0.5f)*/, progress);
                        if (progress >= 0.7f && !shake)
                        {
                            shake = true;
                            CameraShake.instance.ShakeCoroutine();
                            audio.Play();
                            audio.time = 2.5f;
                        }
                    }
                }
            }
            yield return new WaitForSeconds(0.05f);
        }
        if (!up) {
            if (walkTime < patternTime && !isDiscovery) {
                patternTime = 0;
                rigid.velocity = Vector2.zero;
                state = GiantState.Stop;
                curSpeed = 0;
            }
        }
    }
    public void EventEndHide()
    {
        StartCoroutine(enum_EventEndHide());
    }
    IEnumerator enum_EventEndHide()
    {
        float progress = 0;
        while(progress < 1)
        {
            progress += 0.1f;
            renderer.color = Color.Lerp(Color.white, new Color(1, 1, 1, 0), progress);
            yield return new WaitForSeconds(0.1f);
        }
        gameObject.SetActive(false);
    }

    public void LastChaseStart()
    {
        StartCoroutine(Enum_LastChase());
    }
    IEnumerator Enum_LastChase()
    {
        float progress = 0;
        Vector2 vec = transform.position;
        vec.x = 879;
        transform.position = vec;
        gameObject.SetActive(true);
        isLast = true;
        isLeft = false;
        isDiscovery = true;
        anim.SetBool("isMove", true);

        while (progress < 1)
        {
            progress += 0.2f;
            renderer.color = Color.Lerp(new Color(1,1,1,0), Color.white, progress);
            yield return new WaitForSeconds(0.1f);
        }
    }
}
