using System;
using UnityEngine;

public class Wall : MonoBehaviour, ICollidable {
    [SerializeField] private Colour wallColour;
    public Colour GetColour() {
        return wallColour;
    }
    public void DestroySelf() {
        Destroy(gameObject);
    }

    public void Collide() {
        EventController.Invoke(EventController.OnWallCollision, this.transform.position);
    }
}
