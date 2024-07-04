using UnityEngine;

public class GameInput : MonoBehaviour {

    private InputActions inputActions;


    private Vector2 dragStartVector;
    private Vector2 dragEndVector;

    private bool dragStarted;

    public static GameInput Instance { 
        get {
            return instance; 
        } 
        private set { 
            instance = value; 
        } 
    }
    private static GameInput instance;

    private void Awake() {
        if (instance != null && instance != this){
            Destroy(this.gameObject);
        }
        else {
            instance = this;
        }
        inputActions = new InputActions();

        inputActions.Player.Drag.started += Drag_started;
        inputActions.Player.Drag.canceled += Drag_canceled;
    }


    private void Drag_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        dragStarted = false;
        dragEndVector = inputActions.Player.DragPosition.ReadValue<Vector2>();
    }


    private void Drag_started(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        dragStarted = true;
        dragStartVector = inputActions.Player.DragPosition.ReadValue<Vector2>();
    }

    public Vector2 GetDragPosition(out Vector2 startPosition, out Vector2 endPosition) {

        startPosition = dragStartVector;
        endPosition = dragEndVector;

        return inputActions.Player.DragPosition.ReadValue<Vector2>();
    }

    public bool IsDragging() { 
        return dragStarted;
    }
    private void OnEnable() {
        inputActions.Enable();
    }
    private void OnDisable() {
        inputActions.Disable();
    }
}
