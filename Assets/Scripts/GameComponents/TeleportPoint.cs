using System.Collections;
using UnityEngine;

public class TeleportPoint : MonoBehaviour , ICollectable {

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
    public void TeleportPlayer(Transform playerTransform, out Vector2 modifiedMoveDirection) {
        if (Teleported) {
            modifiedMoveDirection = Vector2.zero;
            return;
        }

        playerTransform.position = toTeleportPoint.transform.position;
        modifiedMoveDirection = moveDirVector;

        this.Teleported = true;
        toTeleportPoint.Teleported = true;

        StartCoroutine(ResetTeleportCoolDown());
    }

    private IEnumerator ResetTeleportCoolDown() {
        const float TELEPORT_COOLDOWN = 0.1f;
        yield return new WaitForSeconds(TELEPORT_COOLDOWN);

        this.Teleported = false;
        toTeleportPoint.Teleported = false;
    }

    public void Collect() {
        if (!Teleported) {
            //Player.Instance.transform.position = toTeleportPoint.transform.position;
            //this.Teleported = true;
            //toTeleportPoint.Teleported = true;
            //EventController.Invoke(EventController.OnTeleport, moveDirVector);
            //StartCoroutine(ResetTeleportCoolDown());
        }
    }
}
