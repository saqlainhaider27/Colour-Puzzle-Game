using UnityEngine;

public class ColourSwitcher : Singleton<ColourSwitcher> {

    private Player player;

    private void Start() {
        player = Player.Instance;
    }
    public bool IsColourDifferent(Paint paint) {
        if (player.GetPlayerColour() != paint.GetPaintColour()) {
            return true;
        }
        return false;
    }
    public void SwitchColour(Paint paint) {
        player.HideMeshWithColour(player.GetPlayerColour());
        player.ShowMeshWithColour(paint.GetPaintColour());
        player.SetCurrentPlayerColour(paint.GetPaintColour());
        paint.DestroySelf();
    }

}