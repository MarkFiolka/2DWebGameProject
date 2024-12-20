using UnityEngine;
using Unity.Netcode;

public class PlayerMovement : NetworkBehaviour
{
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private Animator animator;

    private string currentAnimation = "PlayerIdle"; // Default animation

    private User _user;

    private void Update()
    {
        HandleMovement();
        HandleAnimation();
        UpdatePlayerPosition();
    }

    public void SetUser(User user)
    {
        _user = user;
    }

    private void HandleMovement()
    {
        Vector3 moveDirection = Vector3.zero;

        if (Input.GetKey(KeyCode.W)) moveDirection.y = +1f;
        if (Input.GetKey(KeyCode.D)) moveDirection.x = +1f;
        if (Input.GetKey(KeyCode.S)) moveDirection.y = -1f;
        if (Input.GetKey(KeyCode.A)) moveDirection.x = -1f;

        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }

    private void HandleAnimation()
    {
        bool isMoving = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) ||
                        Input.GetKey(KeyCode.D);

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 playerPosition = transform.position;
        Vector3 directionToMouse = (mousePosition - playerPosition).normalized;

        string newAnimation;

        if (isMoving)
        {
            newAnimation = DetermineAnimation(directionToMouse, "Run");
        }
        else
        {
            newAnimation = DetermineAnimation(directionToMouse, "Idle");
        }

        if (newAnimation != currentAnimation)
        {
            currentAnimation = newAnimation;
            animator.Play(currentAnimation);
        }
    }

    private string DetermineAnimation(Vector3 direction, string animationType)
    {
        if (direction.x > 0 && Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            return $"Player{animationType}Right";
        }
        else if (direction.x < 0 && Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            return $"Player{animationType}Left";
        }
        else if (direction.y > 0)
        {
            return $"Player{animationType}Up";
        }
        else
        {
            return $"Player{animationType}Down";
        }
    }

    private void UpdatePlayerPosition()
    {
        if (_user == null)
        {
            Debug.LogError("User object is not set for PlayerMovement!");
            return;
        }
        _user.PosY = (int)transform.position.y;
        _user.PosX = (int)transform.position.x;
    }
}
