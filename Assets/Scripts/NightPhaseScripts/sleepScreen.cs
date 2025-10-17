using UnityEngine;

public class sleepScreen : MonoBehaviour
{

    [SerializeField] private float fadeTime;
    private SpriteRenderer sprite;
    private float curFadeTime;
    private timerScript timer;

    bool intro;
    bool goToSleep;


    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        timer = new timerScript();
        Color c = sprite.color;
        c.a = 1;
        sprite.color = c;
    }

    // Update is called once per frame
    void Update()
    {
        FadeIn();
        FadeOut();
    }

    private void FadeIn()
    {
        if (intro && curFadeTime > 0)
        {
            timer.count(ref curFadeTime);
            Color c = sprite.color;
            c.a = curFadeTime / fadeTime;
            sprite.color = c;
        }
        else
        {
            intro = false;
        }
    }

    private void FadeOut()
    {
        if (goToSleep && curFadeTime > 0)
        {
            timer.count(ref curFadeTime);
            Color c = sprite.color;
            c.a = 1 - curFadeTime / fadeTime;
            sprite.color = c;
        }
    }

    public void setIntro()
    {
        curFadeTime = fadeTime;
        intro = true;
    }

    public void setSleep()
    {
        curFadeTime = fadeTime;
        goToSleep = true;
    }

}
