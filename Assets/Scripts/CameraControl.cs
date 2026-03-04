using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;

    private Vector3 shakeOffset;

    void LateUpdate()
    {
        if (!target) return;

        transform.position = target.position + offset + shakeOffset;
    }

    public void Shake(float duration = 0.1f, float strength = 0.15f)
    {
        StopAllCoroutines();
        StartCoroutine(DoShake(duration, strength));
    }

    public void HitPause(float duration)
    {
        StartCoroutine(HitStop(duration));
    }

    IEnumerator DoShake(float duration, float strength)
    {
        float t = 0f;

        while (t < duration)
        {
            shakeOffset = (Vector3)Random.insideUnitCircle * strength;
            t += Time.deltaTime;
            yield return null;
        }

        shakeOffset = Vector3.zero;
    }
    IEnumerator HitStop(float duration)
    {
        float originalTime = Time.timeScale;
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = originalTime;
    }

}
