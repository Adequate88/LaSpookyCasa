using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class torchScript : MonoBehaviour
{

    private Transform torchTransform;
    private Light2D light;
    private InputActions inputActions;
    private CircleCollider2D circleCollider;

    private bool switchOn;
    private bool switchPressed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inputActions = new InputActions();
        torchTransform = GetComponent<Transform>();
        light = GetComponent<Light2D>();
        circleCollider = GetComponent<CircleCollider2D>();
        inputActions.Enable();
        switchOn = false;
        switchPressed = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Screen coordinates in pixels (bottom-left = (0,0))
        Vector2 screenPos = Mouse.current.position.ReadValue();

        // Convert to world position (e.g. for placing objects in 2D)
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, Camera.main.nearClipPlane));

        bool torchButtonInput = inputActions.PlayerNight.Torch.IsPressed();

        switchOn = switchCheck(switchOn, ref switchPressed, torchButtonInput);

        if (switchOn)
        {
            light.intensity = 2;
            circleCollider.enabled = true;
        }
        else
        {
            light.intensity = 0;
            circleCollider.enabled = false;
        }

        torchTransform.position = worldPos;

    }

    private bool switchCheck(bool swOn, ref bool swPr, bool pressing)
    {
        if (swPr)
        {
            if (!pressing)
            {
                swPr = false;
            } 
        }
        else 
        {
            if (!swOn && pressing)
            {
                swPr = true;
                return true;
            }
            else if (swOn && pressing)
            {
                swPr = true;
                return false;
            }
        }
        
        return swOn;
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }
}
