using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SceneButtonHandler : MonoBehaviour
{
    public AudioClip clickSound;
    public string sceneToLoad = "MainGameScene";
    public float delay = 1f;

    [SerializeField] private float delayBeforeFade = 5f;
    [SerializeField] private float fadeDuration = 1.5f;
    [SerializeField] private Image fadeImage;

    [SerializeField] private AudioClip quitSound;
    private AudioSource audioSource;

    [Header("Popup Settings")]
    [SerializeField] private GameObject popupPanel;

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        // Use or create an AudioSource on this GameObject
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        if (fadeImage != null)
        {
            Color c = fadeImage.color;
            c.a = 0f;
            fadeImage.color = c;
        }

        if (popupPanel != null)
            popupPanel.SetActive(false);
    }

    public void OnButtonPressed()
    {
        FadeOutMusic(1);
        StartCoroutine(PlaySoundAndLoadScene());
    }

    public void LoadTestScene()
    {
        sceneToLoad = "GameScene"; // Set the target scene name
        FadeOutMusic(1);
        StartCoroutine(PlaySoundAndLoadScene());
    }

    private IEnumerator PlaySoundAndLoadScene()
    {
        if (clickSound != null)
        {
            audioSource.PlayOneShot(clickSound);
        }

        yield return new WaitForSeconds(delayBeforeFade);

        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsed / fadeDuration);

            if (fadeImage != null)
            {
                Color c = fadeImage.color;
                c.a = alpha;
                fadeImage.color = c;
            }

            yield return null;
        }

        // Finalize black screen
        if (fadeImage != null)
        {
            Color c = fadeImage.color;
            c.a = 1f;
            fadeImage.color = c;
        }

    yield return new WaitForSeconds(delay);

        SceneManager.LoadScene(sceneToLoad);
    }
    public void QuitGame()
    {
        if (quitSound != null)
        {
            audioSource.PlayOneShot(quitSound);
            Invoke(nameof(ActuallyQuit), quitSound.length); // Wait for sound to finish
        }
        else
        {
            ActuallyQuit();
        }
    }

    private void ActuallyQuit()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }

    public IEnumerator FadeOutMusic(float fadeDuration)
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
    }

    public void ShowPopup()
    {
        if (popupPanel != null)
            popupPanel.SetActive(true);
    }

    public void HidePopup()
    {
        if (popupPanel != null)
            popupPanel.SetActive(false);
    }
}
