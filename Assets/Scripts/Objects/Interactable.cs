using TMPro;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public TextMeshProUGUI interactText;
    public GameObject imageToShow;
    public GameObject grayOut;
    private GameObject canvasGO;

    private GameObject parentObject;

    private SpriteRenderer parentSpriteRenderer;

    [SerializeField] Color interactColor = new Color(0.796f, 0.784f, 0.141f, 1f);
    [SerializeField] AudioSource sfxEnter;
    [SerializeField] AudioSource sfxExit;


    private bool playerOnRange;

    void Awake()
    {
        
        // find the nearest Canvas above imageToShow
        if (imageToShow != null)
        {
            var canvas = imageToShow.GetComponentInParent<Canvas>(true);
            if (canvas != null) canvasGO = canvas.gameObject;
        }

        parentObject = transform.parent.gameObject;
    }

    void Start()
    {
        
        interactText.gameObject.SetActive(false); // Hide text at start
        imageToShow.SetActive(false); // Hide image at start
        grayOut.SetActive(false); // Hide gray screen
        canvasGO.SetActive(false); // Hide Canvas entirely

        parentSpriteRenderer = parentObject.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (playerOnRange && Input.GetKeyDown(KeyCode.E))
        {
            if (!imageToShow.activeSelf)
            {
                sfxEnter.Play();
            }
            else
            {
                sfxExit.Play();
            }
            canvasGO.SetActive(!canvasGO.activeSelf);
            grayOut.SetActive(!grayOut.activeSelf); 
            imageToShow.SetActive(!imageToShow.activeSelf);
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            interactText.gameObject.SetActive(true);
            playerOnRange = true;

            ChangeColor(interactColor);

        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (imageToShow.activeSelf) {
                sfxExit.Play();
            }

            interactText.gameObject.SetActive(false);
            playerOnRange = false;
            imageToShow.SetActive(false);
            canvasGO.SetActive(false);
            grayOut.SetActive(false); 

            ChangeColor(new Color(1f, 1f, 1f, 1f));
        }
    }

    private void ChangeColor(Color newColor)
    {
        if (parentSpriteRenderer != null)
            parentSpriteRenderer.color = newColor;
    }
}


