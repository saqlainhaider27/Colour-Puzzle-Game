using UnityEngine;
using UnityEngine.InputSystem;


[DefaultExecutionOrder(-1)]
public class InputManager : Singleton<InputManager> {


    #region Events
    public delegate void StartTouch(Vector2 position, float time);
    public event StartTouch OnStartTouch;
    public delegate void EndTouch(Vector2 position, float time);
    public event EndTouch OnEndTouch;
    #endregion

    private InputActions inputActions;

    private void Awake() {
        inputActions = new InputActions();
    }

    private void Start() {
        inputActions.Touch.PrimaryContact.started += ctx => PrimaryContact_started(ctx);
        inputActions.Touch.PrimaryContact.canceled += ctx => PrimaryContact_canceled(ctx);

    }

    private void PrimaryContact_started(InputAction.CallbackContext context) {
        if (OnStartTouch != null) {
            OnStartTouch(Utils.ScreenToWorld(Camera.main, inputActions.Touch.PrimaryPosition.ReadValue<Vector2>()), (float)context.startTime);
        }
    }

    private void PrimaryContact_canceled(InputAction.CallbackContext context) {
        if (OnEndTouch != null) {
            OnEndTouch(Utils.ScreenToWorld(Camera.main, inputActions.Touch.PrimaryPosition.ReadValue<Vector2>()), (float)context.time);
        }
    }


    private void OnEnable() {
        inputActions.Enable();
    }
    private void OnDisable() {
        inputActions.Disable();
    }
}