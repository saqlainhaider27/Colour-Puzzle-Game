using System;
using UnityEngine;

public static class EventController {
    public static Action<Vector2> OnStarCollected;
    public static Action<Vector2> OnCoinCollected;
    public static Action<Vector2> OnPaintCollected;
    public static Action<Vector2> OnTeleport;
    public static Action<Vector2> OnWallCollision;
    public static Action OnLevelChanged;
    public static Action OnZeroLifes;
    public static void Invoke(Action action) {
        action?.Invoke();
    }
    public static void InvokeOnLevelChanged() {
        OnLevelChanged?.Invoke();
    }
    public static void Invoke(Action<Vector2> action, Vector2 vector) {
        action?.Invoke(vector);
    }
    public static void Invoke(Action<int> action, int a) {
        action?.Invoke(a);
    }
}