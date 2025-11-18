using UnityEngine;
using UnityEngine.UI;

public class CarProgressBar : MonoBehaviour
{
    [Header("UI References")]
    public RectTransform carIcon;          // the little car sprite
    public RectTransform goalFlag;         // the flag at the end of the bar

    [Header("Minecart References")]
    public Transform minecart;             // the actual moving minecart in the level
    public Transform goal;                 // the goal position in the level

    private float startX;
    private float endX;
    private float totalDistance;

    void Start()
    {
        // Cache UI positions
        startX = 0f;  // starting edge of bar (car starts here)
        endX = goalFlag.localPosition.x;

        // Calculate total distance from minecart start to goal in world space
        if (minecart != null && goal != null)
            totalDistance = Vector3.Distance(minecart.position, goal.position);
    }

    void Update()
    {
        if (!minecart || !goal) return;

        // Calculate how far the minecart is from the goal
        float distanceRemaining = Vector3.Distance(minecart.position, goal.position);

        // Normalize progress (1 = start, 0 = at goal)
        float progress = Mathf.InverseLerp(totalDistance, 0f, distanceRemaining);

        // Move car sprite accordingly
        float newX = Mathf.Lerp(startX, endX, progress);
        carIcon.localPosition = new Vector3(newX, carIcon.localPosition.y, 0f);
    }
}
