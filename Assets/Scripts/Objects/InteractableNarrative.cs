using TMPro;
using UnityEngine;

public class InteractableNarrative : MonoBehaviour
{
    public GameObject imageToShow;
    public GameObject grayOut;
    public GameObject Ebutton;
    public NarrativeController narrativeHandler;

    private GameObject canvasGO;
    private GameObject parentObject;
    private SpriteRenderer parentSpriteRenderer;
    private bool playerOnRange;
    private bool alreadyChecked = false;



    [SerializeField] Color interactColor = new Color(0.796f, 0.784f, 0.141f, 1f);
    [SerializeField] AudioSource sfxEnter;
    [SerializeField] AudioSource sfxExit;



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
        
        imageToShow.SetActive(false); // Hide image at start
        grayOut.SetActive(false); // Hide gray screen
        canvasGO.SetActive(false); // Hide Canvas entirely
        Ebutton.SetActive(false);

        parentSpriteRenderer = parentObject.GetComponent<SpriteRenderer>();
        alreadyChecked = false;
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

            if(!alreadyChecked) {
                
                alreadyChecked = true;
                narrativeHandler.updateCount();

            }

        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Ebutton.SetActive(true);
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

            playerOnRange = false;
            imageToShow.SetActive(false);
            canvasGO.SetActive(false);
            grayOut.SetActive(false); 
            Ebutton.SetActive(false);

            ChangeColor(new Color(1f, 1f, 1f, 1f));
        }
    }

    private void ChangeColor(Color newColor)
    {
        if (parentSpriteRenderer != null)
            parentSpriteRenderer.color = newColor;
    }
}


