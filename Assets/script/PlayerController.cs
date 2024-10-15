using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject player;
    public float moveSpeed;
    public float stopThreshold;
    public float decelerationRate;
    public float maxSpeed;
    public Camera cam;

    private Vector3 velocity = Vector3.zero;
    private bool isMoving = false;

    void Update()
    {
        // Get the mouse position in screen coordinates
        Vector3 mousePos = Input.mousePosition;

        // Convert the mouse position to world position
        mousePos = cam.ScreenToWorldPoint(mousePos);

        // Calculate the direction from the player to the mouse position
        Vector3 direction = mousePos - transform.position;

        // Calculate the angle in radians, then convert to degrees
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Apply rotation to the player
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));
        
        isMoving = false;
        
        // Handle input and apply movement force
        if (Input.GetKey(KeyCode.W))
        {
            velocity += new Vector3(0, moveSpeed * Time.deltaTime, 0);
            isMoving = true;
        }
        if (Input.GetKey(KeyCode.D))
        {
            velocity += new Vector3(moveSpeed * Time.deltaTime, 0, 0);
            isMoving = true;
        }
        if (Input.GetKey(KeyCode.S))
        {
            velocity += new Vector3(0, -moveSpeed * Time.deltaTime, 0);
            isMoving = true;
        }
        if (Input.GetKey(KeyCode.A))
        {
            velocity += new Vector3(-moveSpeed * Time.deltaTime, 0, 0);
            isMoving = true;
        }

        // Cap the velocity to maxSpeed
        if (velocity.magnitude > maxSpeed)
        {
            velocity = velocity.normalized * maxSpeed;
        }

        // Apply deceleration if no input
        if (!isMoving)
        {
            velocity = Vector3.MoveTowards(velocity, Vector3.zero, decelerationRate * Time.deltaTime);
            if (velocity.magnitude < stopThreshold)
            {
                velocity = Vector3.zero;
            }
        }

        // Apply movement to the player
        player.transform.position += velocity * Time.deltaTime;
    }
}