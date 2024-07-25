using UnityEngine;

public class Player : MonoBehaviour {

    [Header("Player Settings")]
    [SerializeField] private float moveSpeed;
    private Vector2 moveDirection;
    [SerializeField] private float raycastLength = 0.5f;


    [Header("References")]
    [SerializeField] private SwipeDetection swipeDetection;
    [SerializeField] private LayerMask collisionLayer;


    private bool canMove;

    private void Update() {
        moveDirection = swipeDetection.GetSwipeDirection();
        canMove = IsPathClear() && moveDirection != Vector2.zero;
        if (canMove) {
            transform.position += (Vector3)moveDirection * Time.deltaTime * moveSpeed;
            RotateInMoveDirection(); // Rotate object in move direction
        }
        else {
            // Stop player movement if WinPoint reached or collision detected
            moveDirection = Vector2.zero;

        }

    }

    private bool IsPathClear() {
        // Check for collision with anything on the collision layer
        RaycastHit2D raycastHit = Physics2D.Raycast(transform.position, moveDirection, raycastLength, collisionLayer);
        if (raycastHit) {
            if (raycastHit.collider.GetComponent<Wall>() != null) {
                Debug.Log("Collided Wall");
                RotateAwayFromCollision(raycastHit.point);
                return false; // Collision detected, cannot move

            }
        }


        // Check if close enough to WinPoint (using distance)
        float distanceToWinPoint = Vector2.Distance(transform.position, WinPoint.Instance.transform.position);
        if (distanceToWinPoint <= 0.1f) {
            UIController.Instance.ShowWinMenu();
            HideSelf();
            return false;
        }

        return true; // No collision and not close enough to WinPoint
    }


    private void HideSelf() {
        this.gameObject.SetActive(false);
    }
    private void RotateInMoveDirection() {
        float _angle = Mathf.Atan2(moveDirection.x, moveDirection.y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, -_angle));
    }
    private void RotateAwayFromCollision(Vector2 collisionPoint) {
        Vector2 directionAwayFromCollision = (Vector2)transform.position - collisionPoint;
        float angle = Mathf.Atan2(directionAwayFromCollision.x, directionAwayFromCollision.y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, -angle));
    }


    public bool CanPlayerMove() {
        return canMove;
    }

}
