using UnityEngine;

public class UIBounce : MonoBehaviour
{
    public float amplitude = 10f; // how far up/down it moves
    public float frequency = 2f;  // how fast it bounces

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.localPosition;
    }

    void Update()
    {
        float newY = startPos.y + Mathf.Sin(Time.time * frequency) * amplitude;
        transform.localPosition = new Vector3(startPos.x, newY, startPos.z);
    }
}