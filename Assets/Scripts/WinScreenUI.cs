using UnityEngine;
using UnityEngine.Events;

public class WinScreenUI : MonoBehaviour
{
    [SerializeField] private UnityEvent<string> OnTimerText;

    void Start()
    {
            float time = GameTimer.finalElapsedTime;
            int minutes = Mathf.FloorToInt(time / 60f);
            int seconds = Mathf.FloorToInt(time % 60f);
            OnTimerText.Invoke($"Tiempo: {minutes:00}:{seconds:00}");
    }
}
