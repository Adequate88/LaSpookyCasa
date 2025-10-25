using UnityEngine;
using UnityEngine.SceneManagement;

public class NightSetupManager : MonoBehaviour
{
    public static NightSetupManager Instance;

    [Header("Monster Behaviour")]
    public float moveTime;
    public int startHealth;
    public int appearanceUnLikelyhood;
    public bool skips;

    [Header("Torch Settings")]
    public float torchHealth;

    private void Awake()
    {
        // Singleton pattern so only one survives between scenes
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
