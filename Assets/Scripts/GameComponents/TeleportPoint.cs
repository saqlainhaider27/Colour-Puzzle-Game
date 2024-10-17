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

        playerTransform.position = toTeleportPoint.transform.position;
        modifiedMoveDirection = moveDirVector;

    }
    

}
