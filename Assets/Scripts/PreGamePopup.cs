using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class PreGamePopup : MonoBehaviour
{
    [Header("UI Elements")]
    public float minDelay = 2f;   // minimum seconds before player can dismiss
    public bool lockCursorWhileActive = true;

    [SerializeField] private UnityEvent OnGameStart;
    
    private bool canDismiss = false;

    private bool popupClosed = false;

    void Awake()
    {

        // Pause the game while the popup is active
        Time.timeScale = 0f;


        // Start delay before allowing dismissal
        StartCoroutine(EnableDismissAfterDelay());

            GameTimer.Instance?.StopTimer();
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

        GameTimer.Instance?.ResumeTimer();

        Time.timeScale = 1f; // resume game

        // Unlock cursor so player can move freely
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        OnGameStart?.Invoke();
    }
}
