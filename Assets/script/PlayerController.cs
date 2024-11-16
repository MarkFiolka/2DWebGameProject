using Unity.Netcode;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    [SerializeField] private float speed = 1f;
    [SerializeField] private float rotationSpeed = 360f;

    private Rigidbody2D rb;
    private Vector2 inputDirection;

    private NetworkVariable<float> networkedRotation = new NetworkVariable<float>(
        default,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server);

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (IsOwner)
        {
            ProcessInput();
            RequestMoveServerRpc(inputDirection);
            UpdateRotation();
        }
        else
        {
            InterpolateRotation();
        }
    }

    private void ProcessInput()
    {
        inputDirection = Vector2.zero;

        if (Input.GetKey(KeyCode.W)) inputDirection += Vector2.up;
        if (Input.GetKey(KeyCode.S)) inputDirection += Vector2.down;
        if (Input.GetKey(KeyCode.A)) inputDirection += Vector2.left;
        if (Input.GetKey(KeyCode.D)) inputDirection += Vector2.right;

        inputDirection.Normalize();
    }

    [ServerRpc]
    private void RequestMoveServerRpc(Vector2 direction)
    {
        if (!IsServer || direction.magnitude == 0)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        Vector2 newPosition = rb.position + direction * speed * Time.fixedDeltaTime;
        rb.MovePosition(newPosition);
    }

    private void UpdateRotation()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 directionToMouse = (mousePosition - transform.position).normalized;
        float targetAngle = Mathf.Atan2(directionToMouse.y, directionToMouse.x) * Mathf.Rad2Deg + -90f;

        float smoothAngle = Mathf.LerpAngle(transform.eulerAngles.z, targetAngle, rotationSpeed);
        transform.rotation = Quaternion.Euler(0, 0, smoothAngle);

        UpdateRotationServerRpc(smoothAngle);
    }

    [ServerRpc]
    private void UpdateRotationServerRpc(float rotation)
    {
        networkedRotation.Value = rotation;
    }

    private void InterpolateRotation()
    {
        float smoothRotation = Mathf.LerpAngle(transform.eulerAngles.z, networkedRotation.Value, rotationSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(0, 0, smoothRotation);
    }
}
