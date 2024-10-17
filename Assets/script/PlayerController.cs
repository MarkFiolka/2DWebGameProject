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
        Vector3 mousePos = Input.mousePosition;

        mousePos = cam.ScreenToWorldPoint(mousePos);

        Vector3 direction = mousePos - transform.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));
        
        isMoving = false;
        
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

        if (velocity.magnitude > maxSpeed)
        {
            velocity = velocity.normalized * maxSpeed;
        }

        if (!isMoving)
        {
            velocity = Vector3.MoveTowards(velocity, Vector3.zero, decelerationRate * Time.deltaTime);
            if (velocity.magnitude < stopThreshold)
            {
                velocity = Vector3.zero;
            }
        }
        
        player.transform.position += velocity * Time.deltaTime;
    }
}
