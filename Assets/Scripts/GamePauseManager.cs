using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class GamePauseManager : MonoBehaviour
{
    [Header("Fade Settings")]
    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeDuration = 0.5f;
    [SerializeField] private Color fadeColor = new Color(0, 0, 0, 0.5f);

    [Header("UI Elements")]
    [SerializeField] private GameObject pauseMenuPanel; // <-- Panel with buttons + image

    private bool isPaused = false;
    private bool isFading = false;

    private GameTimer timer;

    private void Start()
    {
        timer = FindFirstObjectByType<GameTimer>();

        if (fadeImage != null)
        {
            Color c = fadeImage.color;
            c.a = 0f;
            fadeImage.color = c;
        }

        if (pauseMenuPanel != null)
            pauseMenuPanel.SetActive(false);

        // Hide cursor at start (assuming game uses mouse look)
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) && !isFading)
        {
            if (!isPaused)
                StartCoroutine(PauseGame());
            else
                StartCoroutine(ResumeGame());
        }
    }

    private IEnumerator PauseGame()
    {
        isFading = true;
        float elapsed = 0f;

        Color startColor = new Color(0, 0, 0, 0f);
        Color endColor = fadeColor;

        // Fade in background
        while (elapsed < fadeDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            fadeImage.color = Color.Lerp(startColor, endColor, elapsed / fadeDuration);
            yield return null;
        }

        fadeImage.color = endColor;

        if (timer != null)
            timer.StopTimer();

        // Show pause menu and unlock cursor
        if (pauseMenuPanel != null)
            pauseMenuPanel.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Time.timeScale = 0f;
        isPaused = true;
        isFading = false;
    }

    private IEnumerator ResumeGame()
    {
        isFading = true;
        Time.timeScale = 1f;

        if (timer != null)
            timer.ResumeTimer();


        float elapsed = 0f;
        Color startColor = fadeColor;
        Color endColor = new Color(0, 0, 0, 0f);

        // Hide pause menu and re-lock cursor
        if (pauseMenuPanel != null)
            pauseMenuPanel.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Fade out background
        while (elapsed < fadeDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            fadeImage.color = Color.Lerp(startColor, endColor, elapsed / fadeDuration);
            yield return null;
        }

        fadeImage.color = endColor;
        isPaused = false;
        isFading = false;
    }

    // Optional: for linking buttons
    public void ResumeButton()
    {
        if (isPaused && !isFading)
            StartCoroutine(ResumeGame());
    }

    public void QuitButton()
    {
        Debug.Log("Returning to main menu...");
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        UnityEngine.SceneManagement.SceneManager.LoadScene("Title");
    }
}
