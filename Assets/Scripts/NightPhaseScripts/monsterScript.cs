using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class monsterScript : MonoBehaviour
{

    private timerScript timer;
    [SerializeField] GameObject[] spawnPoints;
    [SerializeField] private float moveTime;
    [SerializeField] private float stunTime;
    [SerializeField] private float fadeSpeed;

    private float curMoveTime;
    private float curStunTime;
    private bool flashed;
    private bool anim;

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
    }

    // Update is called once per frame
    void Update()
    {
        if (!anim)
        {
            ResetAlpha(sprite);

            if (flashed)
            {
                timer.count(ref curStunTime);
            }

            if (curStunTime <= 0)
            {
                flashed = false;
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
                while (spawn == prevSpawn);

                prevSpawn = spawn;

                curSpawn = spawnPoints[spawn].transform;

                enemyTransform.position = curSpawn.position;

                curMoveTime = moveTime;
                curStunTime = stunTime;

            }


            timer.count(ref curMoveTime);
        }
        else
        {
            Color c = sprite.color;
            c.a -= fadeSpeed;
            if (c.a <= 0f)
            {
                c.a = 0f;
                anim = false; // stop fading when done
                curStunTime = stunTime;
                curMoveTime = 0;
            }
            
            sprite.color = c;
            
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
            
            
            Debug.Log("gatcha bitch");
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
