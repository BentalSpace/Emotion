using UnityEngine;

public class Background : MonoBehaviour {
    [SerializeField]
    private int frontSprite;
    [SerializeField]
    private int backSprite;
    [SerializeField]
    float speed;
    [SerializeField]
    private Transform[] sprites;
    [SerializeField]
    PlayerRenewal player;
    [SerializeField]
    float spriteSize;

    Rigidbody2D rigid;

    void Awake() {
        rigid = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate() {

    }
    void Update() {
        BackgroundScrolling();
        BackgroundMove();
    }

    void BackgroundScrolling() {
        //무한 배경
        if (Camera.main.transform.position.x >= sprites[frontSprite].position.x + 3) {
            //앞으로 이동중일때
            Vector3 backSpritePos = sprites[backSprite].localPosition;
            sprites[backSprite].localPosition = backSpritePos + Vector3.right * spriteSize;

            int frontSpriteSave = frontSprite;
            frontSprite = backSprite;
            backSprite = frontSpriteSave - 1 == -1 ? sprites.Length - 1 : frontSpriteSave - 1;
        }
        else if (Camera.main.transform.position.x <= sprites[backSprite].position.x - 3) {
            //뒤로 이동중일때
            Vector3 backSpritePos = sprites[backSprite].localPosition;
            sprites[frontSprite].localPosition = backSpritePos + Vector3.left * spriteSize;

            int backSpriteSave = backSprite;
            backSprite = frontSprite;
            frontSprite = backSpriteSave + 1 == 3 ? 0 : backSpriteSave + 1;
        }
    }
    void BackgroundMove() {
        if (Mathf.Abs(player.GetComponent<Rigidbody2D>().velocity.x) > 1) {
            //Vector2 curPos = transform.position;
            Vector2 nextPos = new Vector2(player.H * player.ApplySpeed, 0) * (speed * 0.2f);
            //transform.position = curPos + nextPos;
            rigid.velocity = nextPos;
        }
        else {
            rigid.velocity = new Vector2(0, 0);
        }
    }
}
