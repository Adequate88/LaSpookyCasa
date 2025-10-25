using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionTestScript : MonoBehaviour
{

    public void loadScene(string scene)
    {
        NightSetupManager.Instance.startHealth = 3;
        NightSetupManager.Instance.torchHealth = 20;
        NightSetupManager.Instance.appearanceUnLikelyhood = 2;
        NightSetupManager.Instance.skips = true;
        NightSetupManager.Instance.moveTime = 3;
        SceneManager.LoadScene(scene);
    }
}
