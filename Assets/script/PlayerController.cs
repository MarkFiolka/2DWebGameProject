using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject player;   // Assign via inspector
    [SerializeField] private Camera camDev;       // Assign via inspector
    public float moveSpeed;
    public float stopThreshold;
    public float decelerationRate;
    public float maxSpeed;

    private Transform playerTransform;
    private Transform userDevTransform;

    private Vector3 velocity = Vector3.zero;
    private bool isMoving = false;

    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        if (camDev == null)
        {
            camDev = GameObject.FindGameObjectWithTag("camDev").GetComponent<Camera>();
        }

        playerTransform = player.transform;
        userDevTransform = playerTransform.parent;
    }

    void Update()
    {
        RotatePlayerTowardsMouse();

        HandleMovement();
    }

    private void RotatePlayerTowardsMouse()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 worldMousePos = camDev.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, camDev.nearClipPlane));

        Vector3 direction = (worldMousePos - playerTransform.position);
        direction.z = 0;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        playerTransform.rotation = Quaternion.Euler(0, 0, angle - 90);
    }

    private void HandleMovement()
    {
        isMoving = false;

        Vector3 inputDirection = new Vector3(
            Input.GetKey(KeyCode.D) ? 1 : (Input.GetKey(KeyCode.A) ? -1 : 0),
            Input.GetKey(KeyCode.W) ? 1 : (Input.GetKey(KeyCode.S) ? -1 : 0),
            0
        ).normalized;

        if (inputDirection != Vector3.zero)
        {
            velocity += inputDirection * moveSpeed * Time.deltaTime;
            isMoving = true;
        }

        // Limit velocity to max speed
        if (velocity.magnitude > maxSpeed)
        {
            velocity = velocity.normalized * maxSpeed;
        }

        // Deceleration when not moving
        if (!isMoving)
        {
            velocity = Vector3.MoveTowards(velocity, Vector3.zero, decelerationRate * Time.deltaTime);

            if (velocity.magnitude < stopThreshold)
            {
                velocity = Vector3.zero;
            }
        }

        // Apply movement to userDev
        userDevTransform.position += velocity * Time.deltaTime;
    }
}
