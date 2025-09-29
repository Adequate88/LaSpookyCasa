//using UnityEngine;
//using UnityEngine.Experimental.AI;

//public class nightCamScript : MonoBehaviour
//{

//    [SerializeField] GameObject Background;
//    [SerializeField] float sensitivity;
//    InputActions inputActions;

//    Camera camera;
//    Vector2 currentCameraPosition;

//    // Start is called once before the first execution of Update after the MonoBehaviour is created
//    void Start()
//    {
//        camera = GetComponent<Camera>();
//        inputActions = new InputActions();
//        inputActions.Enable();

//        currentCameraPosition = Background.transform.position;

//    }

//    // Update is called once per frame
//    void Update()
//    {
//        Vector2 mouseDelta = inputActions.PlayerNight.Look.ReadValue<Vector2>();
//        currentCameraPosition += mouseDelta * sensitivity;
//        camera.transform.position = new Vector3(currentCameraPosition.x, currentCameraPosition.y, camera.transform.position.z);
//    }

//}

using UnityEngine;

public class nightCamScript : MonoBehaviour
{
    [SerializeField] GameObject Background;
    [SerializeField] float sensitivity = 0.01f;

    private InputActions inputActions;
    private Camera cam;
    private Vector2 currentCameraPosition;
    private Vector2 bgSize;

    void Start()
    {
        cam = GetComponent<Camera>();
        inputActions = new InputActions();
        inputActions.Enable();

        // start centered on background
        currentCameraPosition = Background.transform.position;

        // get background size in world units
        SpriteRenderer sr = Background.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            bgSize = sr.bounds.size;
        }
        else
        {
            Debug.LogWarning("Background has no SpriteRenderer. You’ll need another way to get size.");
        }
    }

    void Update()
    {
        // read mouse movement
        Vector2 mouseDelta = inputActions.PlayerNight.Look.ReadValue<Vector2>();
        currentCameraPosition += mouseDelta * sensitivity;

        // get camera half-extents
        float camHeight = cam.orthographicSize * 2f;
        float camWidth = camHeight * cam.aspect;

        // clamp camera to stay within background
        float minX = Background.transform.position.x - bgSize.x / 2f + camWidth / 2f;
        float maxX = Background.transform.position.x + bgSize.x / 2f - camWidth / 2f;
        float minY = Background.transform.position.y - bgSize.y / 2f + camHeight / 2f;
        float maxY = Background.transform.position.y + bgSize.y / 2f - camHeight / 2f;

        currentCameraPosition.x = Mathf.Clamp(currentCameraPosition.x, minX, maxX);
        currentCameraPosition.y = Mathf.Clamp(currentCameraPosition.y, minY, maxY);

        // apply position
        cam.transform.position = new Vector3(currentCameraPosition.x, currentCameraPosition.y, cam.transform.position.z);
    }
}

