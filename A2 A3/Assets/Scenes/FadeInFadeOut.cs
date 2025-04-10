using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

public class FadeInFadeOut : MonoBehaviour
{
    public CanvasGroup fadeCanvasGroup;
    public float fadeDuration = 1f;
    public SwitchCamera switchCamScript;
    public GameObject blackScreen;

    public void Start()
    {
        fadeCanvasGroup.alpha = 0f;
        blackScreen.SetActive(false);
    }
    public void BlackScreenOut()
    {
        StartCoroutine(FadeOut());
    }
    public IEnumerator FadeOut()
    {
        //Debug.Log("StartFading");
        blackScreen.SetActive(true);
        fadeCanvasGroup.alpha = 0f;

        // Increase the alpha of the black screen img
        fadeCanvasGroup.blocksRaycasts = true;
        float time = 0f;
        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            fadeCanvasGroup.alpha = time / fadeDuration;
            yield return null;
        }
        fadeCanvasGroup.alpha = 1f;
        yield return new WaitForSeconds(0.2f);

        // When done turn off the black screen
        blackScreen.SetActive(false);
    }

}
