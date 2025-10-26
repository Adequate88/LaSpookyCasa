using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class typeWriterEffect : MonoBehaviour
{
    [Header("UI")]
    public GameObject dialogueBox;          // panel or container that holds the text
    public TMP_Text dialogueText;           // TextMeshProUGUI for the lines

    [Header("Typing")]
    [Tooltip("Seconds per character")]
    public float typingSpeed = 0.04f;
    [Tooltip("Extra delay after punctuation to feel more natural")]
    public float punctuationHold = 0.25f;
    [Tooltip("Pause after each line before the next line auto starts")]
    public float linePause = 0.9f;
    [Tooltip("Pause between blocks of dialogue")]
    public float blockPause = 1.0f;
    public AudioSource typingTick;          // optional tiny tick per char

    [Header("Atmosphere - optional")]
    public AudioSource ambience;            // idle ambience to fade down
    public AudioSource eerieLaugh;          // the laugh sfx
    public AudioSource music;                // the music
    public Light roomLight;                 // dim on shift
    public float dimToIntensity = 0.2f;
    public float dimDuration = 1.5f;
    public GameObject globalVolume;
    public GameObject bloodyText;
    [Header("Screen Fade")]
    public CanvasGroup fadePanel;
    public float fadeDuration = 1.5f;

    [Header("Game Flow Hooks")]
    public UnityEvent onPlayerControlGained;    // fire when player should move around
    public UnityEvent onPlayerControlLost;      // fire when we resume auto dialogue
    public UnityEvent onIntroFinished;          // fire after the last line finishes

    // Opening block
    private readonly string[] openingLines = new string[]
    {
        "Mom and Dad said they'll only be gone for a week.",
        "They left this morning... before the sun came up.",
        "I'm not scared or anything. Just... it feels weird when it's this quiet.",
        "Anyway, I should probably clean my room before they get back.",
        "Or at least pretend I tried."
    };

    // Lines that play right after all interactions are done
    private readonly string[] shiftLines = new string[]
    {
        "Okay... I think that was everything.",
        "The room looks kind of better... I guess.",
        "Maybe I'll rest for a minute.",
    };

    // Final realization lines
    private readonly string[] realizationLines = new string[]
    {
        "...What was that?",
        "Hello?",
        "Mom? Dad? That was not funny.",
        "...is someone there?",
    };

    // state
    private bool interactionsComplete = false;
    private Coroutine runner;

    void Start()
    {
        if (dialogueBox != null) dialogueBox.SetActive(true);
        if (dialogueText != null) dialogueText.text = "";
        globalVolume.SetActive(false);
        bloodyText.SetActive(false);
        runner = StartCoroutine(PlayWholeIntro());
    }

    // call this from your interaction tracker when the player finished everything
    public void OnAllInteractionsComplete()
    {
        interactionsComplete = true;
    }

    private IEnumerator PlayWholeIntro()
    {
        // block 1 - opening
        yield return StartCoroutine(PlayBlock(openingLines));

        // pause briefly after fade

        // fade into room scene
        yield return StartCoroutine(HideDialogueBox());
        yield return StartCoroutine(FadeOut()); // <- fade from black to scene

        // pause briefly after fade
        yield return new WaitForSeconds(0.5f);

        // hand control to player
        onPlayerControlGained?.Invoke();

        // wait until player finishes interactions
        yield return new WaitUntil(() => interactionsComplete);

        // fade back to dialogue (optional, can remove if you don't want a fade before shift)
        yield return StartCoroutine(FadeIn());
        yield return new WaitForSeconds(0.25f);

        // take control back
        onPlayerControlLost?.Invoke();
        yield return StartCoroutine(ShowDialogueBox());


        // block 2 - pre shift lines
        yield return StartCoroutine(PlayBlock(shiftLines));
        yield return StartCoroutine(HideDialogueBox());
        yield return new WaitForSeconds(1.0f);

        // PLAY LAUGH SOUND HERE
        yield return StartCoroutine(stopMusic());
        yield return StartCoroutine(playLaugh());
        yield return new WaitForSeconds(2.5f);
        yield return StartCoroutine(ShowDialogueBox());

        // block 3 - realization
        yield return StartCoroutine(PlayBlock(realizationLines));

        // Turn on global volume HERE
        yield return StartCoroutine(activateVolume());
        yield return StartCoroutine(showBloodyText());
        // fade to black for transition to next part (optional)
        yield return StartCoroutine(HideDialogueBox());
        yield return StartCoroutine(FadeOut());

        // done
        onIntroFinished?.Invoke();
    }

    private IEnumerator FadeOut()
    {
        if (fadePanel == null) yield break;
        fadePanel.alpha = 1f;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / fadeDuration;
            fadePanel.alpha = 1f - t;
            yield return null;
        }

        fadePanel.alpha = 0f;
    }

    private IEnumerator FadeIn()
    {
        if (fadePanel == null) yield break;
        fadePanel.alpha = 0f;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / fadeDuration;
            fadePanel.alpha = t;
            yield return null;
        }

        fadePanel.alpha = 1f;
    }

    private IEnumerator HideDialogueBox(float duration = 0.5f)
    {
        CanvasGroup cg = dialogueBox.GetComponent<CanvasGroup>();
        if (cg == null) yield break;

        float startAlpha = cg.alpha;
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            cg.alpha = Mathf.Lerp(startAlpha, 0f, t);
            yield return null;
        }
        cg.alpha = 0f;
        cg.interactable = false;
        cg.blocksRaycasts = false;
    }

    private IEnumerator ShowDialogueBox(float duration = 0.5f)
    {
        CanvasGroup cg = dialogueBox.GetComponent<CanvasGroup>();
        if (cg == null) yield break;

        float startAlpha = cg.alpha;
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            cg.alpha = Mathf.Lerp(startAlpha, 1f, t);
            yield return null;
        }
        cg.alpha = 1f;
        cg.interactable = true;
        cg.blocksRaycasts = true;
    }

    private IEnumerator PlayBlock(string[] lines)
    {
        for (int i = 0; i < lines.Length; i++)
        {
            yield return StartCoroutine(TypeLine(lines[i]));
            yield return new WaitForSeconds(linePause);
        }
    }

    private IEnumerator TypeLine(string line)
    {
        if (dialogueText == null) yield break;

        dialogueText.text = "";
        for (int i = 0; i < line.Length; i++)
        {
            char c = line[i];
            dialogueText.text += c;

            if (typingTick != null) typingTick.Play();

            // light punctuation rhythm
            if (c == '.' || c == ',' || c == ';' || c == ':' || c == '!' || c == '?')
            {
                yield return new WaitForSeconds(punctuationHold);
            }
            else
            {
                yield return new WaitForSeconds(typingSpeed);
            }
        }
    }

    private IEnumerator playLaugh()
    {
        // the laugh
        if (eerieLaugh != null)
        {
            eerieLaugh.Play();
            yield return new WaitForSeconds(1.0f);
        }
    }

    private IEnumerator stopMusic()
    {
        // the laugh
        if (music != null)
        {
            music.Stop();
            yield return new WaitForSeconds(0.5f);
        }
    }
    private IEnumerator playMusic()
    {
        // the laugh
        if (music != null)
        {
            music.Play();
            yield return new WaitForSeconds(0.5f);
        }
    }
    private IEnumerator activateVolume()
    {
        // the laugh
        if (globalVolume != null)
        {
            globalVolume.SetActive(true);
            yield return new WaitForSeconds(0.5f);
        }
    }

    private IEnumerator showBloodyText()
    {
        // the laugh
        if (bloodyText != null)
        {
            bloodyText.SetActive(true);
            yield return new WaitForSeconds(0.5f);
        }
    }

    // optional helper if you want to cancel this sequence
    public void StopIntro()
    {
        if (runner != null) StopCoroutine(runner);
        runner = null;
        if (dialogueBox != null) dialogueBox.SetActive(false);
    }
}
