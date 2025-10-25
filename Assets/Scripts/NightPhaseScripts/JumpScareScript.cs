using UnityEngine;

public class JumpScareScript : MonoBehaviour
{
    [SerializeField] private AudioClip clip;
    [SerializeField] private float maxScareTime;
    [SerializeField] private float scareDuration;
    [SerializeField] private Transform camera;
    [SerializeField] private torchScript torchScript;

    [SerializeField] AudioSource sfxBackground;
    private AudioSource audioSource;
    private bool active;
    private float jumpScareTime;
    private float curScareTime;
    private timerScript timerScript;
    private SpriteRenderer sprite;
    private Animator animator;
    private string triggerName = "PlayOnce";
    private bool isScaring = false; // Track if jumpscare is active
    private bool finished;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        sprite = GetComponent<SpriteRenderer>();
        audioSource.playOnAwake = false;
        sprite.enabled = false;
        jumpScareTime = maxScareTime;
        timerScript = new timerScript();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        transform.position = camera.position;

        if (active)
        {
            timerScript.count(ref jumpScareTime);
        }
        else
        {
            jumpScareTime = Random.Range(1, maxScareTime);
        }

        // Start jumpscare
        if (jumpScareTime <= 0 && !isScaring)
        {
            isScaring = true;
            curScareTime = scareDuration; // Set once when jumpscare starts
            Play();
            torchScript.Disable();
            sprite.enabled = true;
        }

        // Count down jumpscare duration
        if (isScaring)
        {
            timerScript.count(ref curScareTime);

            if (curScareTime <= 0)
            {
                sprite.enabled = false;
                audioSource.Stop();
                sfxBackground.Stop();
                finished = true;

                // Reset jump scare timer
                jumpScareTime = Random.Range(1, maxScareTime);
            }
        }
    }

    public void Scare()
    {
        active = true;
    }

    private void Play()
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
            sfxBackground.Play();
        }


        if (animator != null)
        {
            animator.SetTrigger(triggerName);
        }
    }

    public bool IsDone() {
        return finished;
    }
}

