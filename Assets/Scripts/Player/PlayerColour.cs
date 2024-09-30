using UnityEngine;

public class PlayerColour : MonoBehaviour {
    [SerializeField] private Colour currentMeshColour;

    public Colour GetCurrentPlayerMeshColour() {
        return currentMeshColour;
    }
}