using UnityEngine;
using TMPro;

public class WinScreenUI : MonoBehaviour
{
    [SerializeField] private TMP_Text finalTimeText;

    void Start()
    {
        if (finalTimeText != null)
        {
            float time = GameTimer.finalElapsedTime;
            int minutes = Mathf.FloorToInt(time / 60f);
            int seconds = Mathf.FloorToInt(time % 60f);
            finalTimeText.text = $"Time: {minutes:00}:{seconds:00}";
        }
    }
}
