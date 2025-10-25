using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class fadeTransition : MonoBehaviour
{
    [SerializeField] Image fadeImage;       // assign black image here
    [SerializeField] float fadeDuration = 1f;
    [SerializeField] float blackScreenHold = 1f;

    [SerializeField] AnimationCurve ease = AnimationCurve.EaseInOut(0, 0, 1, 1);

    void Awake()
    {
        // start fully black and fade in on scene load
        if (fadeImage)
        {
            Color c = fadeImage.color;
            c.a = 1;
            fadeImage.color = c;
            StartCoroutine(FadeIn());
        }
    }

    public void FadeToScene(string sceneName)
    {
        StartCoroutine(FadeOutAndLoad(sceneName));
    }

    IEnumerator FadeIn()
    {
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float u = Mathf.Clamp01(t / fadeDuration);   // progress 0..1
            float e = ease.Evaluate(u);                  // ease-in curve
            SetAlpha(1f - e);                            // alpha goes 1 → 0, slow then quick
            yield return null;
        }
        SetAlpha(0f);
    }

    IEnumerator FadeOutAndLoad(string sceneName)
    {
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            SetAlpha(t / fadeDuration);
            yield return null;
        }
        SetAlpha(1);

        yield return new WaitForSeconds(blackScreenHold);

        SceneManager.LoadScene(sceneName);
    }

    void SetAlpha(float a)
    {
        if (fadeImage)
        {
            Color c = fadeImage.color;
            c.a = a;
            fadeImage.color = c;
        }
    }
}