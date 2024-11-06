using System.Collections;
using UnityEngine;

public class TeleportPoint : NonCollideable {

    [System.Serializable]
    public enum MoveDirections {
        Up,
        Down,
        Left,
        Right
    }

    [SerializeField] private MoveDirections newMoveDirection;  // Direction player moves after teleport
    private Vector2 moveDirVector;  // Stores vector direction of movement
    [SerializeField] private TeleportPoint toTeleportPoint;  // The target teleport point to teleport to

    private bool teleported = false;
    public bool Teleported {
        get {
            return teleported;
        }
        private set {
            teleported = value;
        }
    }

    private void Awake() {
        // Convert enum direction to vector direction
        switch (newMoveDirection) {
            case MoveDirections.Up:
            moveDirVector = Vector2.up;
            break;
            case MoveDirections.Down:
            moveDirVector = Vector2.down;
            break;
            case MoveDirections.Left:
            moveDirVector = Vector2.left;
            break;
            case MoveDirections.Right:
            moveDirVector = Vector2.right;
            break;
        }
    }
    // Teleport the player and set the new movement direction
    public void TeleportPlayer(Transform playerTransform, out Vector2 modifiedMoveDirection) {
        if (Teleported) {
            // Prevent multiple teleportations
            modifiedMoveDirection = Vector2.zero;
            return;
        }

        // Move player to the target teleport point's position
        playerTransform.position = toTeleportPoint.transform.position;
        // Set new movement direction after teleportation
        modifiedMoveDirection = moveDirVector;

        // Mark both teleport points as "used"
        this.Teleported = true;
        toTeleportPoint.Teleported = true;

        // Start cooldown to prevent immediate teleporting again
        StartCoroutine(ResetTeleportCoolDown());
    }

    private IEnumerator ResetTeleportCoolDown() {
        const float TELEPORT_COOLDOWN = 0.1f;
        yield return new WaitForSeconds(TELEPORT_COOLDOWN);

        // Reset the teleported status after the cooldown
        this.Teleported = false;
        toTeleportPoint.Teleported = false;
    }
}
