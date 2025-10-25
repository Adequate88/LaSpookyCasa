using TMPro;
using UnityEngine;

public class nightPhaseManager : MonoBehaviour
{
    [SerializeField] private JumpScareScript jumpScare;
    [SerializeField] private monsterScript monsterScript;
    [SerializeField] private sleepScreen sleepingScreen;
    [SerializeField] private torchScript torchScript;
    [SerializeField] private float sleepTimeInput;
    [SerializeField] private SleepBarScript sleepBarScript;
    [SerializeField] private TextMeshProUGUI introText;
    [SerializeField] private float outroTime;


    [SerializeField] AudioSource sfxAmbient;
    private InputActions inputActions;
    private float curSleepTimeInput;
    private float curOutroTime;
    private timerScript timer;
    private bool sleeping;
    private bool ended;
    bool nightActive;

    private void Awake()
    {
        SetValues(NightSetupManager.Instance);
    }


    private void Start()
    {
        sleepingScreen.setIntro();
        timer = new timerScript();
        inputActions = new InputActions();
        inputActions.Enable();
        nightActive = false;
        sleeping = false;
        curOutroTime = outroTime;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        bool sleepInput = inputActions.PlayerNight.Sleep.IsPressed();

        sleepBarScript.barUpdate(sleepInput, curSleepTimeInput, sleepTimeInput);

        if (!sleeping) {



            if (!nightActive && inputActions.PlayerNight.Torch.IsPressed())
            {
                nightActive = true;
                introText.enabled = false;
                monsterScript.setAwake();
            }

            if (sleepInput)
            {

                timer.count(ref curSleepTimeInput);
                introText.enabled = false;
                if (curSleepTimeInput <= 0)
                {
                    sfxAmbient.Stop();
                    sleeping = true;
                    sleepingScreen.setSleep();
                    torchScript.killTorch();

                    if (monsterScript.getAlive())
                    {
                        monsterScript.setKill();
                    }
                    else
                    {
                        ended = true;
                    }
                }
            }
            else
            {
                curSleepTimeInput = sleepTimeInput;
            }
        }

        if (monsterScript.getKill() && sleeping)
        {
            jumpScare.Scare();
        }
        if (jumpScare.IsDone() || ended)
        {
            timer.count(ref outroTime);

            if (outroTime <= 0)
            {
                //Do scene transfer here
                Debug.Log("Switch Scene bitch");
            }
        }

    }

    private void SetValues(NightSetupManager setup) {
        monsterScript.setStartHealth(setup.startHealth);
        Debug.Log(setup.startHealth);
        monsterScript.setMoveTime(setup.moveTime);
        Debug.Log(setup.moveTime);
        monsterScript.setSkips(setup.skips);
        Debug.Log(setup.skips);
        monsterScript.setAppearanceUnLikelyhood(setup.appearanceUnLikelyhood);
        Debug.Log(setup.appearanceUnLikelyhood);
        torchScript.setTorchHealth(setup.torchHealth);
        Debug.Log(setup.torchHealth);
    }

}
