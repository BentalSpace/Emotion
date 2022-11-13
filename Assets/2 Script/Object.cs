using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Object : MonoBehaviour
{
    public enum objectName { TreeGrow, Branch3, Branch1_8, HotStone, Ceramic, Mushroom, CloudX, CloudY, SadPail, SadHotStoneSadEnd, Wind, Water, Moss, Stool, 
        mRock, dRock, Stalactite
    }
    [SerializeField]
    bool isPassObject;
    public bool IsPassObject { get { return isPassObject; } }
    [SerializeField]
    PlayerRenewal player;
    [SerializeField]
    objectName WhatIsObjectName;
    [SerializeField]
    Animator anim;
    [SerializeField]
    float mushroomJumpXPower;
    [SerializeField]
    float mushroomJumpYPower;
    [SerializeField]
    float speed;
    [SerializeField]
    float cloudMoveStartPos;
    [SerializeField]
    float cloudMoveEndPos;
    [SerializeField]
    GameObject setActiveObject;
    [SerializeField]
    BoxCollider2D boxCollider;
    [SerializeField]
    Sprite changeImage;
    //마지막 위치에 둘것
    [SerializeField]
    GameObject[] needGameObject;

    [SerializeField]
    GameObject dialouge;

    bool isPowerOn;
    bool puzzleReady;
    public float Speed
    {
        get { return speed; }
    }

    public objectName Name
    {
        get { return WhatIsObjectName; }
    }


    void Update()
    {
        PassObjectUpdate();
    }
    void PassObjectUpdate()
    {
        if (!isPassObject)
            return;
        float dis = Vector2.Distance(gameObject.transform.position, player.gameObject.transform.position);

        if (dis <= 20)
        { // 인지범위
            RaycastHit2D[] playerHit = Physics2D.RaycastAll(new Vector2(player.transform.position.x + 0.1f, player.transform.position.y), Vector3.down, 1f);
            Debug.DrawRay(new Vector2(player.transform.position.x + 0.1f, player.transform.position.y), Vector3.down * 1, Color.red);
            if (playerHit.Length > 0)
            {
                foreach (RaycastHit2D hit in playerHit)
                {
                    if (hit.collider.gameObject.layer == 10)
                    {
                        transform.GetChild(0).gameObject.layer = 9;
                    }
                    else if (hit.collider.gameObject.layer == 7)
                    {
                        transform.GetChild(0).gameObject.layer = 10;
                    }
                }
            }
        }
        else
        {
            transform.GetChild(0).gameObject.layer = 10;
        }
    }
    public void Skill()
    {
        if (isPowerOn)
            return;
        switch (WhatIsObjectName)
        {
            case objectName.TreeGrow:
                StartCoroutine(TreeGrowAnimation());
                isPowerOn = true;
                //StartCoroutine(AnimationReturn(3f));
                break;
            case objectName.Branch3:
                anim.SetTrigger("skill 3");
                isPowerOn = true;
                PlayerAbilityGaugeUp();
                StartCoroutine(AnimationReturn(5f));
                break;
            case objectName.Branch1_8:
                anim.SetTrigger("skill 1.8");
                isPowerOn = true;
                PlayerAbilityGaugeUp();
                StartCoroutine(AnimationReturn(5f));
                break;
            case objectName.HotStone:
                setActiveObject.SetActive(true);
                isPowerOn = true;
                PlayerAbilityGaugeUp();
                break;
            case objectName.Ceramic:
                StartCoroutine(DownObjects());
                gameObject.GetComponent<SpriteRenderer>().sprite = changeImage;
                break;
            //슬픔 마지막 컷신 능력
            case objectName.SadPail:
                puzzleReady = true;
                gameObject.GetComponent<SpriteRenderer>().sprite = changeImage;
                StartCoroutine("DialougeOn");
                break;
            case objectName.SadHotStoneSadEnd:
                if (!needGameObject[3].GetComponent<Object>().puzzleReady)
                    break;
                Debug.Log("Test");
                StopCoroutine(SadEndEvent());
                StartCoroutine(SadEndEvent());
                break;
            case objectName.Water:
                gameObject.GetComponentInChildren<Water>().WaterAbility();
                break;
            case objectName.mRock:
                isPowerOn = true;
                // StartCoroutine("RockMove");
                gameObject.GetComponent<AngryObject>().Rock_Move();
                PlayerAbilityGaugeUp();
                Debug.Log("돌");
                break;
            case objectName.dRock:
                isPowerOn = true;
                // StartCoroutine("RockMove");
                gameObject.GetComponent<AngryObject>().Rock_Destroy();
                PlayerAbilityGaugeUp();
                Debug.Log("돌");
                break;
            case objectName.Stalactite:
                isPowerOn = true;
                gameObject.GetComponent<AngryObject>().Stalactite_Destroy();
                PlayerAbilityGaugeUp();
                Debug.Log("종유석");
                break;
        }
    }

    public void ObjectAbility(Transform target, PlayerRenewal player)
    {
        //플레이어가 오브젝트 닿는게 트리거인 경우
        switch (WhatIsObjectName)
        {
            case objectName.Mushroom:
                StartCoroutine(SuperJump(target, player));
                break;
            case objectName.CloudX:
                //켜져있으면 리턴
                if (isPowerOn)
                    return;
                StartCoroutine(CloudMove("X"));
                break;
            case objectName.CloudY:
                //켜져있으면 리턴
                if (isPowerOn)
                    return;
                StartCoroutine(CloudMove("Y"));
                break;
            case objectName.Moss:
                StartCoroutine(gameObject.GetComponent<Moss>().MossAbility(player));
                break;
        }
    }

    IEnumerator DialougeOn()
    {
        yield return new WaitForSeconds(1.5f);
        dialouge.SetActive(true);
        yield return new WaitForSeconds(1f);
        isPowerOn = false;
    }

    IEnumerator AnimationReturn(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        anim.SetTrigger("skillEnd");
        yield return new WaitForSeconds(1f);
        isPowerOn = false;
    }
    IEnumerator DownObjects()
    {
        yield return new WaitForSeconds(0.5f);
        while (true)
        {
            needGameObject[0].transform.position = Vector2.MoveTowards(needGameObject[0].transform.position, new Vector2(needGameObject[0].transform.position.x + 1, needGameObject[0].transform.position.y - 10), 1 * Time.deltaTime);
            needGameObject[1].transform.position = Vector2.MoveTowards(needGameObject[1].transform.position, new Vector2(needGameObject[1].transform.position.x + 5, needGameObject[0].transform.position.y - 10), 1 * Time.deltaTime);

            if (needGameObject[0].transform.position.y < -10.5f && needGameObject[1].transform.position.y < -10.5f)
            {
                yield break;
            }
            yield return null;
        }
    }
    IEnumerator TreeGrowAnimation()
    {
        // 나무가 커진다.
        anim.SetTrigger("Start");
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(player.transform.position.x + 0.1f, player.transform.position.y + 0.3f), Vector3.down, 1f);
        bool playerUp = hit.collider.tag == "Tree";
        yield return new WaitForSeconds(1f);
        boxCollider.offset = new Vector2(boxCollider.offset.x, 2.03f);
        if (playerUp)
            player.transform.position = new Vector2(player.transform.position.x, player.transform.position.y + 1.3f);
        yield return new WaitForSeconds(1f);
        boxCollider.offset = new Vector2(boxCollider.offset.x, 3.28f);
        if (playerUp)
            player.transform.position = new Vector2(player.transform.position.x, player.transform.position.y + 1.95f);
        if (playerUp)
            PlayerAbilityGaugeUp();

        //나무가 작아진다.
        yield return new WaitForSeconds(3f);
        anim.SetTrigger("End");
        yield return new WaitForSeconds(1.2f);
        boxCollider.offset = new Vector2(boxCollider.offset.x, 1.16f);
        isPowerOn = false;
    }
    IEnumerator SuperJump(Transform target, PlayerRenewal player)
    {
        if (target.transform.position.y > transform.position.y)
        {
            if (player.Rigid.velocity.y < -0.1f)
            {
                anim.SetTrigger("trigger");
                yield return new WaitForSeconds(0.3f);

                //현재 플레이어가 점프중이면 중단
                if (Mathf.Abs(player.Rigid.velocity.y) >= 0.1)
                {
                    yield break;
                }

                player.Rigid.velocity = Vector2.zero;
                if (player.FlipX)
                    player.Rigid.AddForce(new Vector2(mushroomJumpXPower * -1, mushroomJumpYPower), ForceMode2D.Impulse);
                else
                    player.Rigid.AddForce(new Vector2(mushroomJumpXPower, mushroomJumpYPower), ForceMode2D.Impulse);
                player.Testtest();
                player.isSuperJump = true;
            }
        }
    }
    IEnumerator CloudMove(string XY)
    {
        Rigidbody2D rigid = GetComponentInChildren<Rigidbody2D>();

        isPowerOn = true;
        anim.SetTrigger("Start");

        //목표지점으로 이동
        yield return new WaitForSeconds(1f);
        isPassObject = false;
        rigid.bodyType = RigidbodyType2D.Kinematic;
        float dir = cloudMoveStartPos - cloudMoveEndPos > 0 ? -1 : 1;
        switch (XY)
        {
            case "X":
                rigid.velocity = new Vector2(dir * speed, 0);
                while (true)
                {
                    if (dir > 0)
                    {
                        if (rigid.transform.position.x >= cloudMoveEndPos)
                        {
                            rigid.velocity = Vector2.zero;
                            break;
                        }
                    }
                    else
                    {
                        if (rigid.transform.position.x <= cloudMoveEndPos)
                        {
                            rigid.velocity = Vector2.zero;
                            break;
                        }
                    }
                    yield return null;
                }
                break;

            case "Y":
                rigid.velocity = new Vector2(0, dir * speed);
                while (true)
                {
                    if (dir > 0)
                    {
                        if (rigid.transform.position.y >= cloudMoveEndPos)
                        {
                            rigid.velocity = Vector2.zero;
                            break;
                        }
                    }
                    else
                    {
                        if (rigid.transform.position.y <= cloudMoveEndPos)
                        {
                            rigid.velocity = Vector2.zero;
                            break;
                        }
                    }
                    yield return null;
                }
                break;
        }

        //아래 시간 기다리고, 초기 위치로 돌아가기
        yield return new WaitForSeconds(3f);
        switch (XY)
        {
            case "X":
                rigid.velocity = new Vector2(dir * -speed, 0);
                while (true)
                {
                    if (dir > 0)
                    {
                        if (rigid.transform.position.x <= cloudMoveStartPos)
                        {
                            rigid.velocity = Vector2.zero;
                            rigid.bodyType = RigidbodyType2D.Static;
                            break;
                        }
                    }
                    else
                    {
                        if (rigid.transform.position.x >= cloudMoveStartPos)
                        {
                            rigid.velocity = Vector2.zero;
                            rigid.bodyType = RigidbodyType2D.Static;
                            break;
                        }
                    }
                    yield return null;
                }
                break;

            case "Y":
                rigid.velocity = new Vector2(0, dir * -speed);
                while (true)
                {
                    if (dir > 0)
                    {
                        if (rigid.transform.position.y <= cloudMoveStartPos)
                        {
                            rigid.velocity = Vector2.zero;
                            rigid.bodyType = RigidbodyType2D.Static;
                            break;
                        }
                    }
                    else
                    {
                        if (rigid.transform.position.y >= cloudMoveStartPos)
                        {
                            rigid.velocity = Vector2.zero;
                            rigid.bodyType = RigidbodyType2D.Static;
                            break;
                        }
                    }
                    yield return null;
                }
                break;
        }
        isPassObject = true;

        //다시 재작동할 준비    
        yield return new WaitForSeconds(2f);
        anim.SetTrigger("End");
        isPowerOn = false;
    }
    IEnumerator SadEndEvent()
    {
        //준비과정
        player.dontInput = true;
        Rigidbody2D rigid = setActiveObject.GetComponent<Rigidbody2D>();
        Rigidbody2D vineRigid = needGameObject[1].GetComponent<Rigidbody2D>();
        ParticleSystem particle = setActiveObject.GetComponentInChildren<ParticleSystem>();
        Animator branchAnim = needGameObject[0].GetComponent<Animator>();
        Animator threadAnim = needGameObject[2].GetComponent<Animator>();
        particle.gameObject.SetActive(false);
        setActiveObject.SetActive(true);
        Camera.main.gameObject.GetComponent<CameraMove>().AnimationProgress = true;
        Camera mainCamera = Camera.main;
        SpriteRenderer chipmunkSprite = needGameObject[5].GetComponent<SpriteRenderer>();

        //카메라 x 702(목표)까지 이동
        while (true)
        {
            mainCamera.transform.position = Vector3.MoveTowards(mainCamera.transform.position, new Vector3(702.4f, mainCamera.transform.position.y, -1), 6 * Time.deltaTime);
            if (mainCamera.transform.position.x >= 701.9f)
            {
                break;
            }
            yield return null;
        }
        //먹구름 이동
        yield return new WaitForSeconds(0.2f);
        rigid.velocity = Vector2.right * 5;
        while (true)
        {
            if (setActiveObject.transform.localPosition.x > setActiveObject.GetComponent<Object>().cloudMoveEndPos)
            {
                rigid.velocity = Vector2.zero;
                break;
            }
            yield return null;
        }
        //카메라는 x 702까지 이동

        //비 내리기
        particle.gameObject.SetActive(true);
        particle.Play();

        //나무가 자라면서 덩쿨 이동
        yield return new WaitForSeconds(1.5f);
        vineRigid.velocity = Vector2.right * 10;
        branchAnim.SetTrigger("sadTrigger");
        while (true)
        {
            if (needGameObject[1].transform.localPosition.x > needGameObject[1].GetComponent<Object>().cloudMoveEndPos)
            {
                vineRigid.velocity = Vector2.zero;
                break;
            }
            yield return null;
        }
        yield return new WaitForSeconds(0.1f);
        threadAnim.SetTrigger("trigger");
        yield return new WaitForSeconds(0.4f);

        //저울 기울기
        needGameObject[4].transform.GetChild(1).localScale = new Vector2(2.7f, 2.5f);
        needGameObject[4].transform.GetChild(1).eulerAngles = new Vector3(0, 0, 20);
        needGameObject[4].transform.GetChild(2).localPosition = new Vector2(-5, -3.8f);
        needGameObject[4].transform.GetChild(3).localPosition = new Vector2(5, 2);

        //다람쥐 옮기고 도토리 파먹기
        needGameObject[5].transform.position = new Vector2(754.27f, -5.7f);
        chipmunkSprite.flipX = true;
        needGameObject[5].GetComponentInChildren<Chipmunk>().TargetObjectSpriteChange(true, new Vector2(1.2f, 1.2f), new Vector2(-0.02f, 2.23f), new Vector2(2.35f, 3.2f));
        needGameObject[5].GetComponentInChildren<Chipmunk>().AcornSitPassObj();

        //나무에 구멍
        needGameObject[6].SetActive(true);

        //이벤트 종료
        Camera.main.gameObject.GetComponent<CameraMove>().AnimationProgress = false;
        player.dontInput = false;
    }

    void PlayerAbilityGaugeUp()
    {
        //슬픔챕터이고, 게이지가 맥스만큼 안차있다면 실행
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            if (player.abilityMaxGauge > player.abilityCurGauge)
            {
                player.abilityCurGauge += 1;
            }
        }

        else if (SceneManager.GetActiveScene().buildIndex == 8)
        {
            if (player.abilityMaxGauge > player.abilityCurGauge)
            {
                player.abilityCurGauge += 1;
            }
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "ThrowObject")
        {
            if (gameObject.tag == "Skill")
            {
                Skill();
            }
            ObjectManager.Instance.ReturnObject(collision.gameObject, "waterBall");
        }
    }
}