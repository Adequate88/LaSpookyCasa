using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RandomDifferencesManager : MonoBehaviour
{
    [Header("Assign each 'Differences' parent here (one per Interactable)")]
    [Tooltip("Drag the 'Differences' Transform from each of your 6 interactables.")]
    public List<Transform> differencesParents = new List<Transform>();

    [Header("Selection Settings")]
    [Min(0)]
    public int totalToShow = 5;

    [Tooltip("Run the random selection automatically in Start()")]
    public bool autoPickOnStart = true;

    [Tooltip("Use a fixed seed to make the selection reproducible (helpful for testing).")]
    public bool useFixedSeed = false;
    public int fixedSeed = 12345;

    // Holds the last selection (read-only from Inspector if you like)
    [SerializeField, Tooltip("Last picked differences (debug)")]
    private List<GameObject> _lastPicked = new List<GameObject>();
    public IReadOnlyList<GameObject> LastPicked => _lastPicked;

    void Start()
    {
        if (autoPickOnStart)
        {
            PickRandomDifferences();
        }
    }

    [ContextMenu("Pick Random Differences Now")]
    public void PickRandomDifferences()
    {
        // 1) Collect all candidate difference items
        var allCandidates = CollectAllDifferenceItems();

        if (allCandidates.Count == 0)
        {
            Debug.LogWarning("[RandomDifferencesManager] No difference items found. " +
                             "Make sure you've assigned the 'Differences' parents.");
            return;
        }

        // Clamp totalToShow
        int k = Mathf.Clamp(totalToShow, 0, allCandidates.Count);

        // 2) Shuffle candidates
        var rng = useFixedSeed ? new System.Random(fixedSeed) : new System.Random();
        Shuffle(allCandidates, rng);

        // 3) Disable all, then enable the first k
        foreach (var go in allCandidates)
            SafeSetActive(go, false);

        _lastPicked = allCandidates.Take(k).ToList();
        foreach (var go in _lastPicked)
            SafeSetActive(go, true);

        Debug.Log($"[RandomDifferencesManager] Enabled {_lastPicked.Count} / {allCandidates.Count} differences.");
    }

    private List<GameObject> CollectAllDifferenceItems()
    {
        var all = new List<GameObject>();

        foreach (var parent in differencesParents.Where(p => p != null))
        {
            // We only take DIRECT children of the 'Differences' parent
            foreach (Transform child in parent)
            {
                if (child != null)
                    all.Add(child.gameObject);
            }
        }

        return all;
    }

    private static void Shuffle<T>(IList<T> list, System.Random rng)
    {
        // Fisher–Yates
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = rng.Next(i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }

    private static void SafeSetActive(GameObject go, bool active)
    {
        if (go != null && go.activeSelf != active)
            go.SetActive(active);
    }
}