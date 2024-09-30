using UnityEngine;

public class TeleportController : Singleton<TeleportController> {

    [SerializeField] private TeleportPoint fromTeleportPoint;
    [SerializeField] private TeleportPoint toTeleportPoint;

    public void TeleportPlayer(Transform transform, out bool canTeleport) {
        transform.position = toTeleportPoint.transform.position;
        TeleportPoint tempTeleportPoint = fromTeleportPoint;
        fromTeleportPoint = toTeleportPoint;
        toTeleportPoint = tempTeleportPoint;
        canTeleport = false;
    }


}
