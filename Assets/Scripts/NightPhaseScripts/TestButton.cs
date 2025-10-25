using UnityEngine;
using UnityEngine.UI;

public class TestButton : MonoBehaviour
{
    Button button;
    [SerializeField] TransitionTestScript transitionTestScript;
    private string scene = "NightPhase";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonClicked);
    }

    private void OnButtonClicked()
    {
        transitionTestScript.loadScene(scene);
    }
}
