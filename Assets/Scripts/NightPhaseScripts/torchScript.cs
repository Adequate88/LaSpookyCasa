using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class torchScript : MonoBehaviour
{

    private Transform torchTransform;
    private Light2D light;
    private InputActions inputActions;
    private CircleCollider2D circleCollider;
    private timerScript timer;

    private float TorchHealth;
    [SerializeField] AudioSource sfxOn;
    [SerializeField] AudioSource sfxOff;
    [SerializeField] AudioSource sfxHum;

    public void setTorchHealth(float ht) { TorchHealth = ht;}

    private bool switchOn;
    private bool switchPressed;
    private float curTorchHealth;

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
        curTorchHealth = TorchHealth;
        timer = new timerScript();

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

        if (switchOn && !(curTorchHealth <= 0))
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

        Debug.Log(curTorchHealth);

    }

    private void FixedUpdate()
    {
        if (switchOn)
        {
            timer.count(ref curTorchHealth);
        }
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
                sfxOn.Play();
                sfxHum.Play();
                return true;
            }
            else if (swOn && pressing)
            {
                swPr = true;
                sfxOff.Play();
                sfxHum.Stop();
                return false;
            }
        }
        
        return swOn;
    }

    public void Disable()
    {
        if (sfxHum.isPlaying) sfxHum.Stop();
        gameObject.SetActive(false);
    }

    public float getTorchHealth()
    {
        return curTorchHealth;
    } 

    public float getTorchMax()
    {
        return TorchHealth;
    }

    public void killTorch()
    {
        curTorchHealth = 0;
    }
}
