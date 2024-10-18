using System;
using UnityEngine;

public class SwipeDetection : MonoBehaviour {

    private Vector2 swipeDirection;


    [Header("Swipe Settings")]
    [SerializeField] private float minimumDistance = .2f;
    [SerializeField] private float maximumTime = 1f;
    [SerializeField, Range(0f, 1f)] private float directionThreshold = .9f;

    private InputManager inputManager;

    private Vector2 startPosition;
    private float startTime;

    private Vector2 endPosition;
    private float endTime;

    private Vector2 direction2D;

    private void Awake() {
        inputManager = InputManager.Instance;
    }
    private void OnEnable() {
        inputManager.OnStartTouch += SwipeStart;
        inputManager.OnEndTouch += SwipeEnd;
    }

    private void OnDisable() {
        inputManager.OnStartTouch -= SwipeStart;
        inputManager.OnEndTouch -= SwipeEnd;
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
            swipeDirection = new Vector2(0f, 1f);
        }
        else if (Vector2.Dot(Vector2.down, direction) > directionThreshold) {
            swipeDirection = new Vector2(0f, -1f);
        }
        else if (Vector2.Dot(Vector2.left, direction) > directionThreshold) {
            swipeDirection = new Vector2(-1f, 0f);
        }
        else if (Vector2.Dot(Vector2.right, direction) > directionThreshold) {
            swipeDirection = new Vector2(1f, 0f);
        }
    }

    public Vector2 GetSwipeDirection() {
        return swipeDirection;
    }
    public void SetSwipeDirection(Vector2 _newSwipeDirection) {
        swipeDirection = _newSwipeDirection;
    }
}
