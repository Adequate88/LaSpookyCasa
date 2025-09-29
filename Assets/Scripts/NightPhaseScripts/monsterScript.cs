using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class monsterScript : MonoBehaviour
{

    private timerScript timer;
    [SerializeField] SpawnPointScript[] spawnPoints;
    [SerializeField] private float moveTime;
    [SerializeField] private float stunTime;
    [SerializeField] private float fadeTime;
    [SerializeField] private int startHealth;


    private float curMoveTime;
    private float curFadeTime;
    private float curStunTime;
    private bool flashed;
    private bool anim;

    private int curHealth;

    private Transform curSpawn;


    private Transform enemyTransform;

    private BoxCollider2D enemyCollider;
    private SpriteRenderer sprite;

    private int prevSpawn;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemyTransform = transform;
        curMoveTime = 0;
        enemyCollider = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        timer = new timerScript();
        prevSpawn = spawnPoints.Length - 1;
        curStunTime = stunTime;
        flashed = false;
        anim = false;
        curHealth = startHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (curHealth > 0)
        {
            if (!anim)
            {
                ResetAlpha(sprite);
                curFadeTime = fadeTime;

                if (flashed)
                {
                    timer.count(ref curStunTime);
                }

                if (curStunTime <= 0)
                {
                    anim = true;
                }

                if (curMoveTime <= 0 && !flashed)
                {
                    //choose if he is visible
                    if (Random.Range(0, 2) == 1)
                    {
                        enemyCollider.enabled = false;
                        sprite.enabled = false;
                    }
                    else
                    {
                        enemyCollider.enabled = true;
                        sprite.enabled = true;
                    }

                    int spawn;
                    do
                    {
                        spawn = Random.Range(0, spawnPoints.Length);

                    }
                    while (spawn == prevSpawn || spawnPoints[spawn].getVisible());

                    prevSpawn = spawn;

                    curSpawn = spawnPoints[spawn].GetTransform();

                    enemyTransform.position = curSpawn.position;

                    curMoveTime = moveTime;
                    curStunTime = stunTime;

                }


                timer.count(ref curMoveTime);
            }
            else
            {
                timer.count(ref curFadeTime);
                Color c = sprite.color;
                c.a = curFadeTime / fadeTime;
                //add running anim
                if (curFadeTime <= 0f)
                {
                    c.a = 0f;
                    anim = false; // stop fading when done
                    curStunTime = stunTime;
                    curMoveTime = 0;
                    flashed = false;
                    curHealth -= 1;
                }

                sprite.color = c;
            }
        }
        else
        {
            sprite.enabled = false;
            //dead
        }
        
        
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Torch")
        {
            if (curStunTime > 0)
            {
                curMoveTime = 0;
                flashed = true;
            }
            
            
            
        }
        
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Torch")
        {
            flashed = false;
            curMoveTime = 0; 
        }
    }

    // Instantly reset to fully visible
    public void ResetAlpha(SpriteRenderer sprite)
    {
        Color c = sprite.color;
        c.a = 1f;
        sprite.color = c;
    }

}
