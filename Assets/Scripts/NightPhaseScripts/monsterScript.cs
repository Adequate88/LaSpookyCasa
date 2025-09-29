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

    [Header("hider settings")]
    [SerializeField] private bool hider;
    [SerializeField] private bool skips;


    private float curMoveTime;
    private float curFadeTime;
    private float curStunTime;
    private bool flashed;
    private bool anim;
    private bool isActive;
    private bool killReady;

    private int curHealth;

    private Transform curSpawn;
    private Transform enemyTransform;
    private BoxCollider2D enemyCollider;
    private SpriteRenderer sprite;

    private int prevSpawn;

    void Start()
    {
        enemyTransform = transform;
        curMoveTime = 0;
        enemyCollider = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        timer = new timerScript();
        prevSpawn = hider ? 0 : spawnPoints.Length - 1;
        curStunTime = stunTime;
        flashed = false;
        isActive = false;
        killReady = false;
        anim = false;
        curHealth = startHealth;
    }

    void Update()
    {
        if (curHealth > 0 || !killReady)
        {
            if (!anim)
            {
                TorchCheck();

                if (hider)
                {
                    HidingBehaviourHandle();
                }
                else
                {
                    HandleSpawnBehaviour();
                }
                
            }
            else
            {
                HandleFadeAnimation();
            }
        }
        else
        {
            sprite.enabled = false;
            enemyCollider.enabled = false;
            // dead
        }
    }

    private void TorchCheck()
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
            prevSpawn = hider ? 0 : prevSpawn;
        }
    }

    private void HandleSpawnBehaviour()
    {

        if (curMoveTime <= 0 && !flashed)
        {
            // choose if visible
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

    private void HidingBehaviourHandle()
    {
        if (curMoveTime <= 0 && !flashed)
        {
            // choose if visible
            if ((Random.Range(0, 2) == 1) || isActive || spawnPoints[prevSpawn].getVisible())
            {
                if (skips && spawnPoints[prevSpawn].getVisible() && prevSpawn < spawnPoints.Length - 1)
                {
                    prevSpawn += 1;
                }

                enemyCollider.enabled = false;
                sprite.enabled = false;
                isActive = false;
            }
            else
            {
                enemyCollider.enabled = true;
                sprite.enabled = true;
                isActive = true;
            }

            int spawn = prevSpawn;

            prevSpawn = spawn;
            curSpawn = spawnPoints[spawn].GetTransform();
            enemyTransform.position = curSpawn.position;

            curMoveTime = moveTime;
            curStunTime = stunTime;

            if (isActive)
            {
                if (prevSpawn == spawnPoints.Length - 1)
                {
                    killReady = true;
                }
                else
                {
                    prevSpawn += 1;
                }
            }
        }

        timer.count(ref curMoveTime);
       
    }

    private void HandleFadeAnimation()
    {
        Debug.Log("eyp");
        timer.count(ref curFadeTime);
        Color c = sprite.color;
        c.a = curFadeTime / fadeTime;

        if (curFadeTime <= 0f)
        {
            c.a = 0f;
            anim = false; 
            curStunTime = stunTime;
            curMoveTime = 0;
            flashed = false;
            curHealth -= 1;
        }

        sprite.color = c;
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

    public void ResetAlpha(SpriteRenderer sprite)
    {
        Color c = sprite.color;
        c.a = 1f;
        sprite.color = c;
    }
}
