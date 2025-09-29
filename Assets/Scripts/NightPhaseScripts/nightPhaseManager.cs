using UnityEngine;

public class nightPhaseManager : MonoBehaviour
{
    [SerializeField] private JumpScareScript jumpScare;
    [SerializeField] private monsterScript monsterScript;

    

    // Update is called once per frame
    void Update()
    {
        if (monsterScript.getKill())
        {
            jumpScare.Scare();
        }
    }
}
