using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PreGamePopup : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject popupPanel; // Assign popup panel
    public float minDelay = 2f;   // minimum seconds before player can dismiss
    public bool lockCursorWhileActive = true;

    private bool canDismiss = false;

    private GameTimer timer;

    private bool popupClosed = false;

    void Start()
    {
        if (popupPanel != null)
        {
            popupPanel.SetActive(true);
        }

        // Pause the game while the popup is active
        Time.timeScale = 0f;

        // Lock the cursor if desired
        if (lockCursorWhileActive)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }


        // Start delay before allowing dismissal
        StartCoroutine(EnableDismissAfterDelay());

        timer = Object.FindFirstObjectByType<GameTimer>();

        if (timer != null)
            timer.StopTimer();
    }

    void Update()
    {
        if (!canDismiss || popupClosed) return;

        if (Input.anyKeyDown || Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            ClosePopup();
        }
    }

    private IEnumerator EnableDismissAfterDelay()
    {
        yield return new WaitForSecondsRealtime(minDelay);
        canDismiss = true;
    }

    private void ClosePopup()
    {

        popupClosed = true;

        if (popupPanel != null)
            popupPanel.SetActive(false);

        if (timer != null)
            timer.ResumeTimer();

        Time.timeScale = 1f; // resume game

        // Unlock cursor so player can move freely
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
