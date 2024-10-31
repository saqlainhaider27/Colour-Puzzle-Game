using UnityEngine;

[CreateAssetMenu()]
public class AudioRefsSO : ScriptableObject {
    public AudioClip[] colourCollect;
    public AudioClip[] starCollect;
    public AudioClip[] hitWall;
    public AudioClip[] teleport;
    public AudioClip[] winPoint;
    public AudioClip[] lose;
    public AudioClip[] menuAppear;
    public AudioClip[] menuDisappear;
}