using UnityEngine;

public class WallColourController : Singleton<WallColourController> {
    private Player player;
    private void Start() {
        player = Player.Instance;
    }

    public bool IsColourDifferent(Wall wall) {
        if (player.GetColour() != wall.GetColour()) {
            return true;
        }
        return false;
    }
}
