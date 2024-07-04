using UnityEngine;

public class Player : MonoBehaviour {

    private GameInput gameInput;

    private Vector2 startPosition;
    private Vector2 endPosition;
    private Vector2 moveDir;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float dragSensitivity = 500f;

    private void Start() {
        gameInput = GameInput.Instance;
    }

    private void Update() {
        if (gameInput.IsDragging()) {
            Vector2 dragPosition = gameInput.GetDragPosition(out startPosition, out endPosition);

            // Calculate differences in x and y coordinates
            float xDiff = startPosition.x - endPosition.x;
            float yDiff = startPosition.y - endPosition.y;

            // Check which difference is larger to determine drag direction
            if (Mathf.Abs(xDiff) > Mathf.Abs(yDiff)) {
                // Horizontal movement
                if (xDiff > dragSensitivity) {
                    // Left
                    moveDir = new Vector2(-1f, 0f);
                }
                else if (xDiff < -dragSensitivity) {
                    // Right
                    moveDir = new Vector2(1f, 0f);
                }
            }
            else {
                // Vertical movement
                if (yDiff > dragSensitivity) {
                    // Down
                    moveDir = new Vector2(0f, -1f);
                }
                else if (yDiff < -dragSensitivity) {
                    // Up
                    moveDir = new Vector2(0f, 1f);
                }
            }
        }

        // Move the player
        transform.position += (Vector3)moveDir * Time.deltaTime * moveSpeed;
    }
}
