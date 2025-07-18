using UnityEngine;

public class LevelMenuScroll : MonoBehaviour {
    public float scrollSpeed = 1.0f;         // Scroll sensitivity
    public float minY = -450f;               // Minimum Y position
    public float maxY = 0f;                  // Maximum Y position

    private Vector2 lastTouchPosition;       // For tracking drag
    private bool isDragging = false;

    void Update() {
#if UNITY_EDITOR || UNITY_STANDALONE
        HandleMouseScroll();
#endif

#if UNITY_ANDROID || UNITY_IOS
        HandleTouchScroll();
#endif
    }

    void HandleMouseScroll() {
        if (Input.GetMouseButtonDown(0)) {
            isDragging = true;
            lastTouchPosition = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0)) {
            isDragging = false;
        }

        if (isDragging) {
            Vector2 currentPosition = Input.mousePosition;
            float deltaY = currentPosition.y - lastTouchPosition.y;

            MoveScroll(deltaY);
            lastTouchPosition = currentPosition;
        }
    }

    void HandleTouchScroll() {
        if (Input.touchCount == 1) {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began) {
                lastTouchPosition = touch.position;
            } else if (touch.phase == TouchPhase.Moved) {
                float deltaY = touch.position.y - lastTouchPosition.y;

                MoveScroll(deltaY);
                lastTouchPosition = touch.position;
            }
        }
    }

    void MoveScroll(float deltaY) {
        // Convert pixel delta to world units using scrollSpeed
        float newY = transform.localPosition.y + deltaY * scrollSpeed * Time.deltaTime;
        newY = Mathf.Clamp(newY, minY, maxY);

        Vector3 newPosition = transform.localPosition;
        newPosition.y = newY;

        transform.localPosition = newPosition;
    }
}
