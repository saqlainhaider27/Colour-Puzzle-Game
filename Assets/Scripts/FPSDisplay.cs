using TMPro;
using UnityEngine;

public class FPSDisplay : MonoBehaviour {

    private float fps;
    private TextMeshProUGUI textMeshPro;

    private void Start() {
        textMeshPro = GetComponent<TextMeshProUGUI>();
        InvokeRepeating("GetFPS", 1, 1);
    }

    private void GetFPS() {
        fps = (int)(1f / Time.unscaledDeltaTime);
        textMeshPro.text = $"FPS: {fps}";

    }



}
