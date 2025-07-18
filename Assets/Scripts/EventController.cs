using System;
using UnityEngine;

public static class EventController {
    public static Action<Vector2> OnStarCollected;
    public static Action<Vector2> OnCoinCollected;
    public static Action<Vector2> OnPaintCollected;
    public static Action<Vector2> OnTeleport;
    public static Action<Vector2> OnWallCollision;
    public static void Invoke(Action action) {
        action.Invoke();
    }

    public static void Invoke(Action<Vector2> onTeleport, Vector2 vector) {
        onTeleport.Invoke(vector);
    }
}