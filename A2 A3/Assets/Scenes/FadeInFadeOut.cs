using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInFadeOut : MonoBehaviour
{
    public CanvasGroup fadeCanvasGroup;
    public float fadeDuration = 1f;

    public void BlackScreenOut()
    {
        fadeCanvasGroup.blocksRaycasts = true;
        float time = 0f;
        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            fadeCanvasGroup.alpha = time / fadeDuration;
            //yield return null;
        }
        fadeCanvasGroup.alpha = 1f;
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            StartCoroutine(FadeIn());
        }

        if (Input.GetMouseButton(1))
        {
            StartCoroutine(FadeOut());
        }

    }

    public IEnumerator FadeIn()
    {
        float time = fadeDuration;
        while (time > 0f)
        {
            time -= Time.deltaTime;
            fadeCanvasGroup.alpha = time / fadeDuration;
            yield return null;
        }
        fadeCanvasGroup.alpha = 0f;
        fadeCanvasGroup.blocksRaycasts = false;
    }

    public IEnumerator FadeOut()
    {
        fadeCanvasGroup.blocksRaycasts = true;
        float time = 0f;
        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            fadeCanvasGroup.alpha = time / fadeDuration;
            yield return null;
        }
        fadeCanvasGroup.alpha = 1f;
    }


}
