using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class WinLoseManager : MonoBehaviour
{
    public SnapTray greenTray;
    public int totalBadPickups;

    [SerializeField] private UnityEvent OnWin;
    [SerializeField] private UnityEvent OnLose;
    
    private int removedBadPickups = 0;
    private bool gameEnded = false;

    // Called by garbage can trigger when a red pickup is removed

    private void Start()
    {
        // Small safety check
        if (greenTray == null)
            Debug.LogWarning("WinLoseManager: Green Tray not assigned!");
    }
    
    public void ReportBadPickupRemoved()
    {
        removedBadPickups++;
        CheckImmediateWinCondition();
    }

    public void ReportGoodPickupPlaced()
    {
        CheckImmediateWinCondition();
    }

    // Called by the minecart when reaching the goal
    public void OnMinecartReachedGoal()
    {
        if (gameEnded) return;

        GameTimer.Instance?.StopTimer();

        if (greenTray.IsComplete() && removedBadPickups >= totalBadPickups)
        {
            WinGame();
        }
        else
        {
            LoseGame();
        }
    }


    private void CheckImmediateWinCondition()
    {
        if (gameEnded) return;

        if (greenTray.IsComplete() && removedBadPickups >= totalBadPickups)
        {
            WinGame();
        }
    }

    private void WinGame()
    {
        if (gameEnded) return;
        gameEnded = true;

        GameTimer.Instance.StopTimer();

        UnlockCursor();
        OnWin?.Invoke();
    }

    public void LoseGame()
    {
        if (gameEnded) return;
        gameEnded = true;

        Debug.Log("You Lose!");
        UnlockCursor();
        OnLose?.Invoke();
    }

    private void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void RestartScene() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
