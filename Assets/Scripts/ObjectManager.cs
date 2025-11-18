using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    [Header("Spawners")]
    public PickupSpawner goodSpawner;
    public PickupSpawner badSpawner;
    public PickupSpawner neutralSpawner;

    [Header("UI Reference")]
    public ObjectListUIManager objectListUI;
    
    private int totalGood;
    private int totalBad;
    private int totalNeutral;

    private int currentGood = 0;
    private int currentBad = 0;
    private int currentNeutral = 0;

    void Start()
    {
        // Get totals from spawner counts
        if (goodSpawner != null) totalGood = goodSpawner.spawnCount;
        if (badSpawner != null) totalBad = badSpawner.spawnCount;
        if (neutralSpawner != null) totalNeutral = neutralSpawner.spawnCount;

        // Initialize UI with current values
        objectListUI.UpdateUI(currentGood, currentBad, currentNeutral, totalGood, totalBad, totalNeutral);
    }

    public void AddGood()
    {
        currentGood++;
        objectListUI.UpdateUI(currentGood, currentBad, currentNeutral, totalGood, totalBad, totalNeutral);
    }

    public void AddBad()
    {
        currentBad++;
        objectListUI.UpdateUI(currentGood, currentBad, currentNeutral, totalGood, totalBad, totalNeutral);
    }

    public void AddNeutral()
    {
        currentNeutral++;
        objectListUI.UpdateUI(currentGood, currentBad, currentNeutral, totalGood, totalBad, totalNeutral);
    }
}
