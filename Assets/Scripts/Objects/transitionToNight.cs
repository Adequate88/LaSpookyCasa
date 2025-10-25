using TMPro;
using UnityEngine;

public class transitionToNight : MonoBehaviour
{

    private bool playerOnRange;

    void Start()
    {

    }

    void Update()
    {
        if (playerOnRange && Input.GetKeyDown(KeyCode.E))
        {
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
