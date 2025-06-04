using UnityEngine;
using System.Collections;

public class ScreenShake : MonoBehaviour
{
    public IEnumerator Shake(float duration, float magnitude) {
        Vector3 originalPos = transform.position;
        float elapsed = 0.0f;
        while (elapsed < duration) {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;
            transform.localPosition = new Vector3(originalPos.x + x, originalPos.y + y, +originalPos.z);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = originalPos;
    }
    public void TriggerShake(float duration, float magnitude) {
        StartCoroutine(Shake(duration, magnitude));

    }
  
}
