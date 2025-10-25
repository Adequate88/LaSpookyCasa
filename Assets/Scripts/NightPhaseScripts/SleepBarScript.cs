using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SleepBarScript : MonoBehaviour
{
    private Slider slider;

    [SerializeField] private Image fill;
    [SerializeField] private Image bar;
    [SerializeField] private TextMeshProUGUI text;

    private void Start()
    {
        slider = GetComponent<Slider>();
        SetUIVisible(false);
    }

    public void barUpdate(bool pressing, float time, float maxTime)
    {
        SetUIVisible(pressing);

        slider.maxValue = maxTime;
        slider.value = maxTime - time;
    }

    private void SetUIVisible(bool visible)
    {
        float alpha = visible ? 0.2f : 0f; // your desired fade level

        SetImageAlpha(fill, alpha);
        SetImageAlpha(bar, alpha);
        SetTextAlpha(text, visible ? 0.2f : 0f); // text fully visible or invisible
    }

    private void SetImageAlpha(Image image, float alpha)
    {
        if (image == null) return;
        Color c = image.color;
        c.a = alpha;
        image.color = c;
    }

    private void SetTextAlpha(TextMeshProUGUI tmp, float alpha)
    {
        if (tmp == null) return;
        Color c = tmp.color;
        c.a = alpha;
        tmp.color = c;
    }
}
