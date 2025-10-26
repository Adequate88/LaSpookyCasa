using TMPro;
using UnityEngine;

public class bedInteractable : MonoBehaviour
{
    public GameObject Ebutton;

    private GameObject parentObject;

    private SpriteRenderer parentSpriteRenderer;

    [SerializeField] Color interactColor = new Color(0.796f, 0.784f, 0.141f, 1f);
    [SerializeField] AudioSource sfxEnter;
    [SerializeField] AudioSource sfxExit;

    private bool playerOnRange;

    void Awake()
    {

        parentObject = transform.parent.gameObject;
    }

    void Start()
    {

        Ebutton.SetActive(false);
        parentSpriteRenderer = parentObject.GetComponent<SpriteRenderer>();
    }

    void Update()
    {

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

            playerOnRange = false;
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


