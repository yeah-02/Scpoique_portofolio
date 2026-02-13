using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScreenFader : MonoBehaviour
{
    public Image fadeImage;
    public float fadeTime = 2f;

    void Start()
    {
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        float t = 0f;
        Color c = fadeImage.color;

        while (t < fadeTime)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(1f, 0f, t / fadeTime);
            fadeImage.color = c;
            yield return null;
        }

        fadeImage.gameObject.SetActive(false);
    }
}

