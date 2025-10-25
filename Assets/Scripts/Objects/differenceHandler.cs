using UnityEngine;

public class differenceHandler : MonoBehaviour
{

    public void Deactivate()
    {
        // Disable after a short delay, so OnClick can finish
        StartCoroutine(HideAfterFrame());
    }

    private System.Collections.IEnumerator HideAfterFrame()
    {
        yield return null; // wait one frame
        gameObject.SetActive(false);
    }
}
