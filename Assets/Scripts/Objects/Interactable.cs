using TMPro;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public TextMeshProUGUI interactText;
    public GameObject imageToShow;

    private bool playerOnRange;

    void Start()
    {
        interactText.gameObject.SetActive(false); // Hide text at start
        imageToShow.SetActive(false); // Hide image at start
    }

    void Update()
    {
        if (playerOnRange && Input.GetKeyDown(KeyCode.E))
        {
            imageToShow.SetActive(!imageToShow.activeSelf);
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            interactText.gameObject.SetActive(true);
            playerOnRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            interactText.gameObject.SetActive(false);
            playerOnRange = false;
            imageToShow.SetActive(false);
        }
    }
}


