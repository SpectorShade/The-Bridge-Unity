using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StrikeManager : MonoBehaviour
{
    public WinLoseManager winLoseManager;
    [Header("Strike UI")]
    public Image[] strikeIcons;          // Assign 5 icons 
    public Color normalColor = Color.black;
    public Color strikeColor = Color.white;

    [Header("Strike Settings")]
    public int maxStrikes = 5;

    private int currentStrikes = 0;
    private bool gameEnded = false;

    public float strikeGracePeriod = 4f; // seconds
    private float timeSinceStart = 0f;

    private void Update()
    {
        timeSinceStart += Time.deltaTime;
    }

    public void RegisterStrike()
    {
        if (gameEnded) return;

        if (timeSinceStart < strikeGracePeriod) return;

        if (currentStrikes < strikeIcons.Length)
        {
            strikeIcons[currentStrikes].color = strikeColor;
            currentStrikes++;
            FindFirstObjectByType<VignetteFlash>().FlashBad();
        }

        if (currentStrikes >= maxStrikes)
        {
            gameEnded = true;
            winLoseManager.LoseGame();
        }
    }

    public void ResetStrikes()
    {
        currentStrikes = 0;
        gameEnded = false;

        foreach (var icon in strikeIcons)
        {
            icon.color = normalColor;
        }
    }
}
