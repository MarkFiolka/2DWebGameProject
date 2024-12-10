using UnityEngine;

public class IdentificationScript : MonoBehaviour
{
    [SerializeField] private string playerId; // Unique ID for the player
    [SerializeField] private string bulletId; // Unique ID for the bullet

    public void SetPlayerId(string id)
    {
        playerId = id;
    }

    public void SetBulletId(string id)
    {
        bulletId = id;
    }

    public string GetPlayerId()
    {
        return playerId;
    }

    public string GetBulletId()
    {
        return bulletId;
    }

    public bool IsPlayer()
    {
        return !string.IsNullOrEmpty(playerId) && string.IsNullOrEmpty(bulletId);
    }

    public bool IsBullet()
    {
        return !string.IsNullOrEmpty(bulletId);
    }
}