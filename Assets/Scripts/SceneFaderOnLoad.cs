using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SceneFaderOnLoad : MonoBehaviour
{
    [Tooltip("Image covering the screen. Should start as fully black.")]
    [SerializeField] private Image fadeImage;

    [Tooltip("How long it takes to fade in.")]
    [SerializeField] private float fadeDuration = 1.5f;

    private void Start()
    {
        if (fadeImage != null)
        {
            Color c = fadeImage.color;
            c.a = 1f; // Ensure it's fully opaque at start
            fadeImage.color = c;

            StartCoroutine(FadeIn());
        }
    }

    private IEnumerator FadeIn()
    {
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);

            Color c = fadeImage.color;
            c.a = alpha;
            fadeImage.color = c;

            yield return null;
        }

        // Ensure fully transparent and non-blocking
        Color finalColor = fadeImage.color;
        finalColor.a = 0f;
        fadeImage.color = finalColor;

        // Optional: Disable raycast blocking to allow interaction
        fadeImage.raycastTarget = false;
    }
}
