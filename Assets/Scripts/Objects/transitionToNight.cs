using TMPro;
using UnityEngine;

public class transitionToNight : MonoBehaviour
{

    [SerializeField] GameObject fadeHandler;
    DifferenceTracker diffTracker;
    

    private bool playerOnRange;

    void Start()
    {
        fadeHandler.SetActive(false);
        diffTracker = FindAnyObjectByType<DifferenceTracker>();
    }

    void Update()
    {
        if (playerOnRange && Input.GetKeyDown(KeyCode.E))
        {   
            fadeHandler.SetActive(true);
            prepareNight( diffTracker.getRemainingActiveDifferences(), diffTracker.getInitialDif(), NightSetupManager.Instance);
            FindObjectOfType<fadeTransition>().FadeToScene("NightPhase");
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerOnRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerOnRange = false;
        }
    }

    private void prepareNight(int remaining, int total, NightSetupManager setup)
    {
        setup.startHealth = setup.MinStartHealth[setup.day] + remaining;
        setup.moveTime = setup.moveTimesNight[setup.day - 1];
        setup.appearanceUnLikelyhood = setup.appearnceProbsNight[setup.day - 1];
        setup.skips = true;
        setup.maxWaitResets = setup.nightMaxWeightResets[setup.day - 1];
    }
}
