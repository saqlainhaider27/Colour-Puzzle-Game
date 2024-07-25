using System;
using UnityEngine;

public class SwipeDetection : MonoBehaviour {

    [Header("Player Settings")]
    [SerializeField] private float moveSpeed;
    private Vector2 moveDirection;
    [SerializeField] private float raycastLength = 0.5f;

    [Header("Swipe Settings")]
    [SerializeField] private float minimumDistance = .2f;
    [SerializeField] private float maximumTime = 1f;
    [SerializeField, Range(0f, 1f)] private float directionThreshold = .9f;

    [Header("References")]
    [SerializeField] private LayerMask collisionLayer;

    private InputManager inputManager;

    private Vector2 startPosition;
    private float startTime;

    private Vector2 endPosition;
    private float endTime;

    private Vector2 direction2D;

    private bool canMove;

    private void Awake() {
        inputManager = InputManager.Instance;
    }

    private void Update() {
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


    private void OnEnable() {
        inputManager.OnStartTouch += SwipeStart;
        inputManager.OnEndTouch += SwipeEnd;
    }

    private void OnDisable() {
        inputManager.OnStartTouch -= SwipeStart;
        inputManager.OnEndTouch -= SwipeEnd;
    }

    private bool IsPathClear() {
        // Check for collision with anything on the collision layer
        RaycastHit2D raycastHit = Physics2D.Raycast(transform.position, moveDirection, raycastLength, collisionLayer);
        if (raycastHit) {
            RotateAwayFromCollision(raycastHit.point);
            return false; // Collision detected, cannot move

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

    private void RotateAwayFromCollision(Vector2 collisionPoint) {
        Vector2 directionAwayFromCollision = (Vector2)transform.position - collisionPoint;
        float angle = Mathf.Atan2(directionAwayFromCollision.x, directionAwayFromCollision.y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, -angle));
    }

    private void HideSelf() {
        this.gameObject.SetActive(false);
    }

    private void SwipeStart(Vector2 position, float time) {
        startPosition = position;
        startTime = time;
    }

    private void SwipeEnd(Vector2 position, float time) {
        endPosition = position;
        endTime = time;
        DetectSwipe();
    }

    private void DetectSwipe() {
        if (Vector3.Distance(startPosition, endPosition) >= minimumDistance && (endTime - startTime) <= maximumTime) {
            Debug.DrawLine(startPosition, endPosition, Color.red, 5f);
            Vector3 direction = endPosition - startPosition;
            direction2D = new Vector2(direction.x, direction.y).normalized;
            SwipeDirection(direction2D);
        }
    }

    private void SwipeDirection(Vector2 direction) {
        if (Vector2.Dot(Vector2.up, direction) > directionThreshold) {
            moveDirection = new Vector2(0f, 1f);
        }
        else if (Vector2.Dot(Vector2.down, direction) > directionThreshold) {
            moveDirection = new Vector2(0f, -1f);
        }
        else if (Vector2.Dot(Vector2.left, direction) > directionThreshold) {
            moveDirection = new Vector2(-1f, 0f);
        }
        else if (Vector2.Dot(Vector2.right, direction) > directionThreshold) {
            moveDirection = new Vector2(1f, 0f);
        }
    }
    private void RotateInMoveDirection() {
        float _angle = Mathf.Atan2(moveDirection.x, moveDirection.y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, -_angle));
    }

    public bool CanPlayerMove() {
        return canMove;
    }

    //private void OnDrawGizmos() {
    //    Gizmos.color = Color.green;
    //    if (direction2D.magnitude > 0.1f) {
    //        Gizmos.DrawRay(transform.position, moveDirection * raycastLength);
    //    }
    //}
}
