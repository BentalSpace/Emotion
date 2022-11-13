using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderWeb : MonoBehaviour
{
    public float rotPower;
    float flightSpeed = 2;

    Transform target;
    AnimationCurve curve;
    Animator anim;

    IEnumerator coroutine;
    void Awake() {
        anim = GetComponent<Animator>();
    }
    private void OnEnable() {
        GetComponent<SpriteRenderer>().color = Color.white;
        anim.Play("Spider_Bullet");
    }
    private void OnDisable() {
        StopAllCoroutines();
        coroutine = null;
    }
    void Update() {
        //transform.Translate(new Vector2(Time.deltaTime * -10f, 0));
        // -10 < 20

        //transform.localEulerAngles += Vector3.forward * Time.deltaTime * 17;
        if (transform.localEulerAngles.z < 20) {
        }
    }
    public void Fire(Transform _target, AnimationCurve _curve) {
        target = _target;
        curve = _curve;

        StartCoroutine(IEFlight());
    }
    private IEnumerator IEFlight() {
        float duration = flightSpeed;
        float time = 0.0f;
        Vector3 start = transform.position;
        Vector3 end = target.position + (Vector3.up * 1.5f);

        while (time < duration) {
            time += Time.deltaTime;
            float linearT = time / duration;
            float heightT = curve.Evaluate(linearT);

            float rotZ = -20 + (linearT * 40);
            transform.localEulerAngles = Vector3.forward * rotZ;

            float height = Mathf.Lerp(0.0f, 5, heightT);

            transform.position = Vector2.Lerp(start, end, linearT) + new Vector2(0.0f, height);
            // 2초동안 이동
            if(time >= 1.5f && coroutine == null) {
                coroutine = FireEndHide();
                StartCoroutine(coroutine);
            }
            yield return null;
        }   
        Debug.Log("coroutineEnd");
    }

    IEnumerator FireEndHide() {
        // 0.5초동안 사라짐
        float progress = 0;
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        while (progress < 1) {
            progress += 0.1f;
            renderer.color = Color.Lerp(Color.white, new Color(1, 1, 1, 0), progress);
            yield return new WaitForSeconds(0.05f);
         }
        coroutine = null;
        gameObject.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D collision) {
        Debug.Log("TES");
        if (collision.CompareTag("Object")) {
            if(collision.GetComponent<Object>().Name == Object.objectName.Stool) {
                collision.GetComponent<Stool>().Hide();
                gameObject.SetActive(false);
            }
        }
        else if (collision.CompareTag("Player")) {
            gameObject.SetActive(false);
        }
    }
}
