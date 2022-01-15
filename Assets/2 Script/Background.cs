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

    void Awake() {
        //spriteSize = 57.6f;
    }

    void FixedUpdate() {

    }
    void Update() {
        BackgroundScrolling();
        Vector2 curPos = transform.position;
        Vector2 nextPos = new Vector2(player.H, 0) * speed * Time.deltaTime;
        transform.position = curPos + nextPos;
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
}
