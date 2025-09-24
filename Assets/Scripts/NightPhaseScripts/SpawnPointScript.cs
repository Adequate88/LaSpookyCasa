using Unity.VisualScripting;
using UnityEngine;

public class SpawnPointScript : MonoBehaviour
{

    private bool visible;

    private void Awake()
    {
        visible = false;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "Torch")
        {
            visible = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Torch")
        {
            visible = false;
        }
    }

    public bool getVisible()
    {
        return visible;
    }

    public Transform GetTransform()
    {
        return transform;
    }
}
