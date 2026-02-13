using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.Collections;

public class ScreenVignette : MonoBehaviour
{
    public Volume volume;
    private Vignette vignette;

    void Start()
    {
        volume.profile.TryGet(out vignette);
        vignette.intensity.value = 0f;
    }

    public void StartBlackout(float duration = 2f)
    {
        StartCoroutine(BlackoutRoutine(duration));
    }

    IEnumerator BlackoutRoutine(float time)
    {
        float t = 0f;
        while (t < time)
        {
            t += Time.deltaTime;
            vignette.intensity.value = Mathf.Lerp(0f, 1f, t / time);
            yield return null;
        }
        vignette.intensity.value = 1f;
    }

    public void ForceBlack()
    {
        if (vignette != null)
            vignette.intensity.value = 1f;
    }
}
