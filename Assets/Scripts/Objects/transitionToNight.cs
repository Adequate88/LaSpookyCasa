using TMPro;
using UnityEngine;

public class transitionToNight : MonoBehaviour
{

    [SerializeField] GameObject fadeHandler;
    private bool playerOnRange;

    void Start()
    {
        fadeHandler.SetActive(false);
    }

    void Update()
    {
        if (playerOnRange && Input.GetKeyDown(KeyCode.E))
        {   
            fadeHandler.SetActive(true);
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
}
