using UnityEngine;

public class ResetPlayerPrefs : MonoBehaviour {
    [ContextMenu("Reset PlayerPrefs")]
    void ResetPrefs() {
        PlayerPrefs.DeleteAll();
        Debug.Log("PlayerPrefs have been reset.");
    }
}
