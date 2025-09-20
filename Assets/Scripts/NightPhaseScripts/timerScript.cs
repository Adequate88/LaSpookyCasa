using UnityEngine;

public class timerScript
{
    //timer function
    public void count(ref float currTime)
    {
        if (currTime > 0)
            currTime -= Time.fixedDeltaTime;
    }
}
