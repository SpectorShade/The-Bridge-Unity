using UnityEngine;
using TMPro;
using System.Collections;

public class ObjectListUIManager : MonoBehaviour
{
    [Header("UI References")]
    public RectTransform panelTransform; // The sliding panel
    public TMP_Text goodCountText;
    public TMP_Text badCountText;
    public TMP_Text neutralCountText;

    [Header("Animation Settings")]
    public float slideDuration = 0.5f;
    public Vector2 hiddenPosition = new Vector2(0, -180); // Start position (offscreen)
    public Vector2 shownPosition = new Vector2(0, 20);    // Shown position

    private bool isShown = false;
    private Coroutine slideRoutine;

    [Header("Counts")]
    private int currentGood, currentBad, currentNeutral;
    private int totalGood, totalBad, totalNeutral;

    void Start()
    {
        panelTransform.anchoredPosition = hiddenPosition;
        UpdateText();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
            TogglePanel();
    }

    public void TogglePanel()
    {
        if (slideRoutine != null)
            StopCoroutine(slideRoutine);

        Vector2 target = isShown ? hiddenPosition : shownPosition;
        slideRoutine = StartCoroutine(SlidePanel(target));
        isShown = !isShown;
    }

    private IEnumerator SlidePanel(Vector2 targetPos)
    {
        Vector2 startPos = panelTransform.anchoredPosition;
        float elapsed = 0f;

        while (elapsed < slideDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            panelTransform.anchoredPosition = Vector2.Lerp(startPos, targetPos, elapsed / slideDuration);
            yield return null;
        }

        panelTransform.anchoredPosition = targetPos;
    }

    public void UpdateUI(int good, int bad, int neutral, int totalGood, int totalBad, int totalNeutral)
    {
        currentGood = good;
        currentBad = bad;
        currentNeutral = neutral;

        this.totalGood = totalGood;
        this.totalBad = totalBad;
        this.totalNeutral = totalNeutral;

        UpdateText();
    }
    
    private void UpdateText()
    {
        if (goodCountText != null)
            goodCountText.text = $"{currentGood}/{totalGood}";

        if (badCountText != null)
            badCountText.text = $"{currentBad}/{totalBad}";

        if (neutralCountText != null)
            neutralCountText.text = $"{currentNeutral}/{totalNeutral}";
    }

}
