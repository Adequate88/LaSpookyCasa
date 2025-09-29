using UnityEngine;

public class JumpScareScript : MonoBehaviour
{
    [SerializeField] private AudioClip clip;
    [SerializeField] private float maxScareTime;
    [SerializeField] private Transform camera;
    [SerializeField] private torchScript torchScript;

    private AudioSource audioSource;
    private bool active;
    private float jumpScareTime;
    private timerScript timerScript;
    private SpriteRenderer sprite;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        sprite = GetComponent<SpriteRenderer>();
        audioSource.playOnAwake = false;
        sprite.enabled = false;
        jumpScareTime = maxScareTime;
        timerScript = new timerScript();
    }

    private void Update()
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

        if (jumpScareTime <= 0)
        {
            torchScript.Disable();
            sprite.enabled = true;
            Play();
        }
    }

    public void Scare()
    {
        active = true;
    }

    private void Play()
    {
        audioSource.PlayOneShot(clip);
    }
}
