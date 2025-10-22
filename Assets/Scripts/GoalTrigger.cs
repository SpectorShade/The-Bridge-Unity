using UnityEngine;

public class GoalTrigger : MonoBehaviour
{
    public WinLoseManager manager;

    private void OnTriggerEnter(Collider other)
    {
       manager.OnMinecartReachedGoal();
    }
}
