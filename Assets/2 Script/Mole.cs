using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Mole : MonoBehaviour
{
    Animator anim;
    [SerializeField]
    GameObject Hole;
    [SerializeField, Tooltip("두더지가 뚫고 갈 타일맵")]
    Tilemap moleTile;
    [SerializeField, Tooltip("두더지가 뚫고 간 후 상태의 타일맵")]
    Tilemap moleHoleTile;

    private void Awake() {
        anim = GetComponent<Animator>();
    }
    public void GoDown() {
        anim.SetBool("isHit", true);
        StartCoroutine(Enum_GoDown());
    }
    IEnumerator Enum_GoDown() {
        yield return new WaitForSeconds(0.6f);
        anim.SetTrigger("Down");
        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f);

        yield return new WaitForSeconds(0.4f);
        float progress = 0;
        Hole.SetActive(true);
        foreach(SpriteRenderer item in Hole.GetComponentsInChildren<SpriteRenderer>()) {
            item.color = new Color(1, 1, 1, 0);
        }
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
        while(progress < 1) {
            progress += 0.2f;
            moleTile.color = Color.Lerp(Color.white, new Color(1, 1, 1, 0), progress);
            moleHoleTile.color = Color.Lerp(new Color(1, 1, 1, 0), Color.white, progress);
            foreach (SpriteRenderer item in Hole.GetComponentsInChildren<SpriteRenderer>()) {
                item.color = Color.Lerp(new Color(1, 1, 1, 0), Color.white, progress);
            }
            yield return new WaitForSeconds(0.05f);
        }
        moleTile.gameObject.SetActive(false);
    }
}
