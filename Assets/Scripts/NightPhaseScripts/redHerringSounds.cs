using UnityEngine;

public class redHerringSounds : MonoBehaviour
{
    private AudioSource hehe;
    private timerScript timer;
    private float curwaitTime;
    [SerializeField] private float maxHerringWaitTime = 1;
    [SerializeField] private int likelyhoodOutof = 1;
    [SerializeField] private AudioClip[] sounds;


    private void Start()
    {
        hehe = GetComponent<AudioSource>();
        timer = new timerScript();
        curwaitTime = maxHerringWaitTime;
    }

    private void FixedUpdate()
    {
        timer.count(ref curwaitTime);

        if (curwaitTime <= 0) {
            curwaitTime = maxHerringWaitTime;
            if (Random.Range(1,likelyhoodOutof) == 1)
            {
                hehe.clip = sounds[Random.Range(1, sounds.Length)];
                hehe.PlayOneShot(hehe.clip);
            }
        }
    }
}
