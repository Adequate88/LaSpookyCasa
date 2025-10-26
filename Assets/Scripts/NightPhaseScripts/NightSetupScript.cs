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
    public float maxWaitTime;

    [Header("Torch Settings")]
    public float torchHealth;

    [Header("Extra Setup Variables")]
    public int[] MaxStartHealth = new int[3];
    public float[] moveTimesNight = new float[3];
    public int[] appearnceProbsNight = new int[3];
    public float[] maxWaitTimes = new float[3];
    public int day = 0;
    public bool completed = false;

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
