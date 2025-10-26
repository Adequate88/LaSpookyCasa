using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class monsterScript : MonoBehaviour
{
    private timerScript timer;
    [SerializeField] SpawnPointScript[] spawnPoints;
    private float moveTime;
    [SerializeField] private float stunTime;
    [SerializeField] private float fadeTime;
    private int startHealth;
    private int appearanceUnLikelyhood = 2;
    private float maxWaitTime;
    private float curWaitTime;
    private AudioSource audioMoster;
    [SerializeField] AudioClip[] MoveNoises = new AudioClip[5];
 
    

    [Header("hider settings")]
    [SerializeField] private bool hider;
    private bool skips;

    public void setMoveTime(float mt) { moveTime = mt; }
    public void setStartHealth(int mt) { startHealth = mt; }
    public void setAppearanceUnLikelyhood(int mt) { appearanceUnLikelyhood = mt; }
    public void setSkips(bool mt) { skips = mt; }
    public void setMaxWait(float mt) { maxWaitTime = mt; }


    private float curMoveTime;
    private float curFadeTime;
    private float curStunTime;
    private bool flashed;
    private bool anim;
    private bool isActive;
    private bool killReady;
    private bool isAwake;

    private int curHealth;

    private Transform curSpawn;
    private Transform enemyTransform;
    private BoxCollider2D enemyCollider;
    private SpriteRenderer sprite;

    private int nextSpawnIndex;
    private int curSpawnIndex;
    private int prevSeenSpawnIndex;

    void Start()
    {
        enemyTransform = transform;
        audioMoster = GetComponent<AudioSource>();
        curMoveTime = 0;
        enemyCollider = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        timer = new timerScript();
        nextSpawnIndex = hider ? 0 : spawnPoints.Length - 1;
        prevSeenSpawnIndex = -1;
        curStunTime = stunTime;
        flashed = false;
        isActive = false;
        killReady = false;
        anim = false;
        curHealth = startHealth;
        isAwake = false;
    }

    void FixedUpdate()
    {
        //Debug.Log(curHealth);
        Debug.Log("currentSpawn" + curSpawnIndex);
        Debug.Log("lastSeenSpawn" + prevSeenSpawnIndex);

        if ((curHealth > 0) && !killReady && isAwake)
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
            prevSeenSpawnIndex = nextSpawnIndex;
            nextSpawnIndex = hider ? 0 : nextSpawnIndex;
        }
    }

    private void HandleSpawnBehaviour()
    {

        if (curMoveTime <= 0 && !flashed)
        {
            // choose if visible
            if (Random.Range(0, appearanceUnLikelyhood) == 1)
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
            while (spawn == nextSpawnIndex || spawnPoints[spawn].getVisible());

            nextSpawnIndex = spawn;
            curSpawn = spawnPoints[spawn].GetTransform();
            enemyTransform.position = curSpawn.position;

            curMoveTime = moveTime;
            curStunTime = stunTime;
        }

        timer.count(ref curMoveTime);
    }

    private void HidingBehaviourHandle()
    {
        if (((curMoveTime <= 0)) && !flashed && (nextSpawnIndex < spawnPoints.Length))
        {
            int rando = (Random.Range(0, appearanceUnLikelyhood));
            Debug.Log("Im Stuck 4s");
            // choose if visible
            if ((rando != 1 || isActive))
            {
             
                enemyCollider.enabled = false;
                sprite.enabled = false;
                isActive = false;
                Debug.Log("Im Stuck 3");

            }
            else
            {
                if ((skips && spawnPoints[nextSpawnIndex].getVisible()) && curSpawnIndex != spawnPoints.Length - 1 )
                {
                    nextSpawnIndex += 1;
                    audioMoster.clip = MoveNoises[Random.Range(0, (MoveNoises.Length - 1))];
                    audioMoster.PlayOneShot(audioMoster.clip);
                    Debug.Log("Im Stuck 1");
                }
                else
                {
                    enemyCollider.enabled = true;
                    sprite.enabled = true;
                    isActive = true;
                    audioMoster.clip = MoveNoises[Random.Range(0, (MoveNoises.Length - 1))];
                    audioMoster.PlayOneShot(audioMoster.clip);
                    Debug.Log("Im Stuck 2");

                }  
            }

            int spawn = nextSpawnIndex;

            nextSpawnIndex = spawn;
            curSpawn = spawnPoints[spawn].GetTransform();
            enemyTransform.position = curSpawn.position;
            curSpawnIndex = spawn;

            curMoveTime = moveTime;
            curStunTime = stunTime;

            if (isActive)
            {
                nextSpawnIndex += 1;
            }

        }
        else if (nextSpawnIndex == spawnPoints.Length && ((curMoveTime <= 0)) && isActive)
        {
            enemyCollider.enabled = false;
            sprite.enabled = false;
            isActive = false;
            killReady = true;
            Debug.Log("Im Stuck 5s");
        }
        

        timer.count(ref curMoveTime);

    }

    private void HandleFadeAnimation()
    {
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
                //prevSeenSpawnIndex = nextSpawnIndex;
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

    private void ResetAlpha(SpriteRenderer sprite)
    {
        Color c = sprite.color;
        c.a = 1f;
        sprite.color = c;
    }

    public bool getKill()
    {
        return killReady;
    }

    public bool getHider()
    {
        return hider;
    }

    public void setAwake()
    {
        isAwake = true;
    }

    public void setKill()
    {
        killReady = true;
    }

    public bool getAlive()
    {
        return curHealth > 0;
    }
}
