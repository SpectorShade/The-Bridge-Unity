using System;
using UnityEngine;
using TMPro;

public class GameTimer : MonoBehaviour
{

    public static GameTimer Instance { get; private set; }

    [SerializeField] private TMP_Text timerText;

    public static float finalElapsedTime = 0f;

    public float elapsedTime { get; private set; } = 0f;

    private void Awake() {
        Instance = this;
    }

    void Update()
    {

        elapsedTime += Time.unscaledDeltaTime;

        int minutes = Mathf.FloorToInt(elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime % 60f);

        timerText.text = $"{minutes:00}:{seconds:00}";
    }

    public void StartTimer()
    {
        enabled = true;
    }

    public void StopTimer()
    {
        enabled = false;
        finalElapsedTime = elapsedTime;
    }

    public void ResumeTimer()
    {
        enabled = true;
    }
    

    public void ResetTimer()
    {
        elapsedTime = 0f;
        timerText.text = "00:00";
    }
}
