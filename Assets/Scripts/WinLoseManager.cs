using UnityEngine;
using UnityEngine.SceneManagement;

public class WinLoseManager : MonoBehaviour
{
    public SnapTray greenTray;      
    public int totalBadPickups;      
    private int removedBadPickups = 0;

    private bool gameEnded = false;

    [Header("Scene Names")]
    public string winSceneName = "WinScreen";
    public string loseSceneName = "LoseScreen";

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

        GameTimer timer = Object.FindFirstObjectByType<GameTimer>();
        if (timer != null) timer.StopTimer();

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

        GameTimer timer = Object.FindFirstObjectByType<GameTimer>();
        if (timer != null) timer.StopTimer();


        Debug.Log("You Win!");
        UnlockCursor();
        SceneManager.LoadScene(winSceneName);
    }

    private void LoseGame()
    {
        if (gameEnded) return;
        gameEnded = true;

        Debug.Log("You Lose!");
        UnlockCursor();
        SceneManager.LoadScene(loseSceneName);
    }

    private void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
