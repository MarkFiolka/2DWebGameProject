/*using UnityEngine;

public class BulletScript : MonoBehaviour
{
    private Vector3 direction;
    private float speed;
    private float damage;
    private float maxDistance;
    private float slowdownDistance = 2f;
    private int maxRayPositions = 3;

    private float traveledDistance = 0f;
    private Vector3[] rayPositions;
    private int rayPositionIndex = 0;

    private bool isSlowingDown = false;

    private string shooterId;
    private string bulletId;
    private Collider2D shooterCollider;

    private void Start()
    {
        rayPositions = new Vector3[maxRayPositions];
    }

    public void Initialize(Vector3 bulletDirection, float bulletSpeed, float bulletDamage, float bulletMaxDistance, string shooterIdentifier, string bulletIdentifier, Collider2D shooterColliderRef)
    {
        Debug.Log("Bullet initialized successfully");
        direction = bulletDirection.normalized;
        speed = bulletSpeed;
        damage = bulletDamage;
        maxDistance = bulletMaxDistance;
        shooterId = shooterIdentifier;
        bulletId = bulletIdentifier;
        shooterCollider = shooterColliderRef;

        // Ignore collision with the shooter
        Collider2D bulletCollider = GetComponent<Collider2D>();
        if (shooterCollider != null && bulletCollider != null)
        {
            Physics2D.IgnoreCollision(shooterCollider, bulletCollider);
            Debug.Log("Ignoring collision with shooter.");
        }
    }

    private void Update()
    {
        float deltaDistance = speed * Time.deltaTime;

        if (!isSlowingDown && traveledDistance >= maxDistance - slowdownDistance)
        {
            isSlowingDown = true;
        }

        if (isSlowingDown)
        {
            deltaDistance *= Mathf.Clamp01((maxDistance - traveledDistance) / slowdownDistance);
        }

        Vector3 newPosition = transform.position + direction * deltaDistance;
        traveledDistance += deltaDistance;

        SaveRayPosition(transform.position);

        transform.position = newPosition;

        if (traveledDistance >= maxDistance)
        {
            Destroy(gameObject);
        }
    }

    private void SaveRayPosition(Vector3 position)
    {
        rayPositions[rayPositionIndex] = position;
        rayPositionIndex = (rayPositionIndex + 1) % maxRayPositions;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == null) return;

        IdentificationScript identification = collision.GetComponent<IdentificationScript>();
        if (identification != null)
        {
            Debug.Log($"Collision detected with: {collision.name}, PlayerId: {identification.GetPlayerId()}, ShooterId: {shooterId}");

            // Ignore collisions with the shooter or the same bullet
            if (identification.GetPlayerId() == shooterId || identification.GetBulletId() == bulletId)
            {
                Debug.Log("Ignored collision with shooter or same bullet.");
                return;
            }

            // Handle collision with another player
            if (identification.IsPlayer() && identification.GetPlayerId() != shooterId)
            {
                Debug.Log($"Bullet hit enemy player {collision.name}, dealing {damage} damage");
                Destroy(gameObject);
            }
        }

        // Destroy the bullet if it hits anything else
        Destroy(gameObject);
    }

    public Vector3[] GetRayPositions()
    {
        Vector3[] recordedPositions = new Vector3[maxRayPositions];
        for (int i = 0; i < maxRayPositions; i++)
        {
            recordedPositions[i] = rayPositions[(rayPositionIndex + i) % maxRayPositions];
        }
        return recordedPositions;
    }
}*/
