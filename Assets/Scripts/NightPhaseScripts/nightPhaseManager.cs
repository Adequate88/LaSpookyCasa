using UnityEngine;

public class nightPhaseManager : MonoBehaviour
{
    [SerializeField] private JumpScareScript jumpScare;
    [SerializeField] private monsterScript monsterScript;
    [SerializeField] private sleepScreen sleepingScreen;
    [SerializeField] private torchScript torchScript;
    [SerializeField] private float sleepTimeInput;


    [SerializeField] AudioSource sfxAmbient;
    private InputActions inputActions;
    private float curSleepTimeInput;
    private timerScript timer;
    private bool sleeping;

    bool nightActive;
    

    private void Start()
    {
        sleepingScreen.setIntro();
        timer = new timerScript();
        inputActions = new InputActions();
        inputActions.Enable();
        nightActive = false;
        sleeping = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        bool sleepInput = inputActions.PlayerNight.Sleep.IsPressed();

        if (!sleeping) {
            if (!nightActive && inputActions.PlayerNight.Torch.IsPressed())
            {
                nightActive = true;
                monsterScript.setAwake();
            }

            if (sleepInput)
            {
                timer.count(ref curSleepTimeInput);
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
    }

}
