using System.Collections;
using System.Linq;
using UnityEngine;
using TMPro;

public class DifferenceTracker : MonoBehaviour
{
    [Header("References")]
    [SerializeField] RandomDifferencesManager picker;   // your existing script

    [Header("Timing")]
    [SerializeField, Min(0.02f)] float checkInterval = 0.25f; // how often to refresh

    // state
    private int totalSelected = 0;
    private int activeRemaining = 0;

    void OnEnable()
    {
        StartCoroutine(TrackRoutine());
    }

    IEnumerator TrackRoutine()
    {
        // wait until picker has made a selection
        while (picker == null || picker.LastPicked == null || picker.LastPicked.Count == 0)
        {
            yield return null;
        }

        totalSelected = picker.LastPicked.Count;
        Debug.Log(totalSelected + "dhjcbvsd");

        // periodic refresh
        var wait = new WaitForSeconds(checkInterval);
        while (true)
        {
            activeRemaining = picker.LastPicked.Count(go => go != null && go.activeSelf);
            Debug.Log(activeRemaining);
            // win condition hook
            if (activeRemaining == 0)
            {
                OnAllCleared(); // call your win logic here
                // break if you do not need to keep tracking after done
                // yield break;
            }

            yield return wait;
        }
    }

    // put your game progression here
    void OnAllCleared()
    {
        // Debug.Log("All differences cleared!");
        // Example: open door, load next level, show banner, etc.
    }

    public int getRemainingActiveDifferences()
    {
        return activeRemaining;
    }

    public int getInitialDif()
    {
        return totalSelected;
    }
}
