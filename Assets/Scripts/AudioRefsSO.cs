using UnityEngine;

[CreateAssetMenu()]
public class AudioRefsSO : ScriptableObject {
    public AudioClip[] colourCollect;
    public AudioClip[] starCollect;
    public AudioClip[] hitWall;
    public AudioClip[] teleport;
    public AudioClip[] winPoint;
    public AudioClip[] lose;
    public AudioClip[] starUIEntry;
    public AudioClip[] revive;
}