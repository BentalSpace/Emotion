using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake instance;

    Transform camTr;
    bool shakeRotate;

    Vector3 originPos;
    Quaternion originRot;

    void Awake() {
        instance = this;
        camTr = Camera.main.transform;
    }

    public IEnumerator ShakeCamera(float duration = 0.1f, float magnitudePos = 0.3f, float magnitudeRot = 0.1f) {
        // duration = �ð� | magnitudePos = ȭ���� ��鸮�� ������� �����ϸ� ��. | magnitudeRot = ȸ�� ��鸲 ���� (shakeRotate�� true�� ������ �۵�)
        originPos = camTr.localPosition;
        originRot = camTr.localRotation;
        float passTime = 0.0f;

        while (passTime < duration) {
            Vector3 shakePos = Random.insideUnitSphere;
            camTr.localPosition = originPos + shakePos * magnitudePos;

            if (shakeRotate) {
                Vector3 shakeRot = new Vector3(0, 0, Mathf.PerlinNoise(Time.time * magnitudeRot, 0.0f));

                camTr.localRotation = Quaternion.Euler(shakeRot);
            }
            passTime += Time.deltaTime;
            yield return null;
        }
        camTr.localPosition = originPos;
        camTr.localRotation = originRot;
    }
    public void ShakeCoroutine(float duration = 0.1f, float magnitudePos = 0.3f, float magnitudeRot = 0.1f) {
        StopAllCoroutines();
        StartCoroutine(ShakeCamera(duration, magnitudePos, magnitudeRot));
    }
}
