using UnityEngine;

public class torchHandScript : MonoBehaviour
{
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private torchScript torchScript;
    [SerializeField] private float followSpeed = 8f;
    [SerializeField] private Vector2 offset = new Vector2(0f, -1.5f); // base position below center
    [SerializeField] private float movementScale = 0.2f; // how much the hand reacts to mouse movement
    [SerializeField] private Camera mainCamera;

    private SpriteRenderer spriteRenderer;
    private InputActions inputActions;
    private int currentIndex = 0;
    private Vector3 basePosition;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        inputActions = new InputActions();
        inputActions.Enable();

        if (mainCamera == null)
            mainCamera = Camera.main;

        basePosition = transform.position; // starting position to scale movement around
    }

    void Update()
    {
        // --- Update torch sprite based on battery ---
        checkBatteryLevel(ref currentIndex, torchScript.getTorchHealth(), torchScript.getTorchMax());

        if (inputActions.PlayerNight.Torch.IsPressed())
            spriteRenderer.sprite = sprites[currentIndex + 1];
        else
            spriteRenderer.sprite = sprites[currentIndex];

        // --- Mouse world position ---
        Vector3 mouseWorld = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = 0f;

        // --- Calculate relative position offset from camera center ---
        Vector3 cameraCenter = mainCamera.transform.position;
        Vector3 directionFromCenter = mouseWorld - cameraCenter;

        // Scale down mouse influence
        Vector3 scaledOffset = directionFromCenter * movementScale;

        // Target position = base position (below camera) + scaled offset
        Vector3 targetPos = cameraCenter + (Vector3)offset + scaledOffset;
        targetPos.z = 0f;

        // Smooth follow
        transform.position = Vector3.Lerp(transform.position, targetPos, followSpeed * Time.fixedDeltaTime);

        // --- Optional: rotate slightly toward mouse ---
        Vector2 direction = (mouseWorld - transform.position).normalized;
        //float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, angle - 90f), 10f * Time.fixedDeltaTime);
    }

    private void checkBatteryLevel(ref int index, float health, float max)
    {
        if (health > max * 3 / 4) index = 0;
        else if (health > max / 2) index = 2;
        else if (health > max / 4) index = 4;
        else index = 6;
    }
}



