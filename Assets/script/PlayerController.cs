using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private GameObject player;
    public float moveSpeed;
    public float stopThreshold;
    public float decelerationRate;
    public float maxSpeed;
    private Camera camDev;

    private Vector3 velocity = Vector3.zero;
    private bool isMoving = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        camDev = GameObject.FindGameObjectWithTag("camDev").GetComponent<Camera>();
    }

    void Update()
    {
<<<<<<< HEAD
        
        Vector3 mousePos = Input.mousePosition;
        
        Vector3 worldMousePos = camDev.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, camDev.transform.position.z * -1));
        
        Vector3 direction = worldMousePos - player.transform.position;
        
        direction.z = 0;
        
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        player.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));

=======
        Vector3 mousePos = Input.mousePosition;

        mousePos = cam.ScreenToWorldPoint(mousePos);

        Vector3 direction = mousePos - transform.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));
        
>>>>>>> f11f41b3f002c13f8ed68ef55c8b4816e577c988
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
<<<<<<< HEAD
        
=======

>>>>>>> f11f41b3f002c13f8ed68ef55c8b4816e577c988
        if (velocity.magnitude > maxSpeed)
        {
            velocity = velocity.normalized * maxSpeed;
        }
<<<<<<< HEAD
        
=======

>>>>>>> f11f41b3f002c13f8ed68ef55c8b4816e577c988
        if (!isMoving)
        {
            velocity = Vector3.MoveTowards(velocity, Vector3.zero, decelerationRate * Time.deltaTime);
            if (velocity.magnitude < stopThreshold)
            {
                velocity = Vector3.zero;
            }
        }
        
<<<<<<< HEAD
        GameObject userDev = player.transform.parent.gameObject;
        userDev.transform.position += velocity * Time.deltaTime;
=======
        player.transform.position += velocity * Time.deltaTime;
>>>>>>> f11f41b3f002c13f8ed68ef55c8b4816e577c988
    }
}
