using UnityEngine;

public class TeleportPoint : MonoBehaviour {
    [System.Serializable]
    public enum MoveDirecrtions {
        Up,
        Down,
        Left,
        Right
    }
    [SerializeField] private MoveDirecrtions newMoveDirection;

    private Vector2 moveDirVector;
    [SerializeField] private TeleportPoint toTeleportPoint;
    private void Awake() {

        switch (newMoveDirection) {
            case MoveDirecrtions.Up:
                moveDirVector = Vector2.up;
            break;
            case MoveDirecrtions.Down:
                moveDirVector = Vector2.down;
            break;
            case MoveDirecrtions.Left:
                moveDirVector = Vector2.left;
            break;
            case MoveDirecrtions.Right:
                moveDirVector = Vector2.right;
            break;
        }


    }
    public void TeleportPlayer(Transform transform , out Vector2 modifiedMoveDirection) {

        transform.position = toTeleportPoint.transform.position;
        modifiedMoveDirection = moveDirVector;
    }
}
