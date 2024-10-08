using System.Collections;
using UnityEngine;

public class TeleportPoint : MonoBehaviour {

    [System.Serializable]
    public enum MoveDirections {
        Up,
        Down,
        Left,
        Right
    }

    [SerializeField] private MoveDirections newMoveDirection;
    private Vector2 moveDirVector;
    [SerializeField] private TeleportPoint toTeleportPoint;
    [SerializeField] private float teleportCooldown = 0.5f; // Cooldown time for teleporting

    private bool isTeleportReady = true; // Cooldown flag

    private void Awake() {
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

    public void TeleportPlayer(Transform playerTransform, out Vector2 modifiedMoveDirection) {
        if (!isTeleportReady) {
            modifiedMoveDirection = Vector2.zero; // If not ready, prevent teleport
            return;
        }

        playerTransform.position = toTeleportPoint.transform.position;
        modifiedMoveDirection = moveDirVector;

        // Disable teleporting until cooldown is complete
        isTeleportReady = false;
        StartCoroutine(TeleportCooldown());
    }

    private IEnumerator TeleportCooldown() {
        yield return new WaitForSeconds(teleportCooldown);
        isTeleportReady = true; // Re-enable teleportation after cooldown
        Debug.Log("Player can teleport to: " + this.gameObject.name);
    }
}
