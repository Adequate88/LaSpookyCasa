using UnityEngine;

public class NarrativeController : MonoBehaviour
{   
    public typeWriterEffect typeWriter;
    private int interactionCount = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        interactionCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (interactionCount == 6){
            typeWriter.OnAllInteractionsComplete();
        }
    }

    public void updateCount(){
        interactionCount += 1;
    }

}
