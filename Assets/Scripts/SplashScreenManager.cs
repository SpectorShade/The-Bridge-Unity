using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SplashScreenManager : MonoBehaviour
{
    [Header("Durations")]
    public float splashDuration = 3f;
    public float fadeDuration = 1f;

    [Header("Fade")]
    public Image fadeImage; // Assign a black Image in Canvas covering the screen

    [Header("Scene Settings")]
    public string nextSceneName = "Title"; // Set this in the Inspector

    private void Start()
    {
        if (fadeImage != null)
            fadeImage.color = new Color(0, 0, 0, 1f); // Start fully black

        StartCoroutine(PlaySplash());
    }

    private IEnumerator PlaySplash()
    {
        // Fade in (black -> transparent)
        if (fadeImage != null)
        {
            float t = 0f;
            while (t < fadeDuration)
            {
                t += Time.deltaTime;
                float alpha = Mathf.Lerp(1f, 0f, t / fadeDuration);
                fadeImage.color = new Color(0, 0, 0, alpha);
                yield return null;
            }
        }

        yield return new WaitForSeconds(splashDuration);

        // Fade out (transparent -> black)
        if (fadeImage != null)
        {
            float t = 0f;
            while (t < fadeDuration)
            {
                t += Time.deltaTime;
                float alpha = Mathf.Lerp(0f, 1f, t / fadeDuration);
                fadeImage.color = new Color(0, 0, 0, alpha);
                yield return null;
            }
        }

        SceneManager.LoadScene(nextSceneName);
    }
}
