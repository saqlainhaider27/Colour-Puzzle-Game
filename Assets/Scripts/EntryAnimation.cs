using UnityEngine;

public class EntryAnimation : MonoBehaviour {
    [SerializeField] private AudioRefsSO audioRefsSO;
    public void PlayColourChangeSound() {
        PlaySound(audioRefsSO.colourCollect, Vector3.zero);
    }
    private void PlaySound(AudioClip[] audioClipArray, Vector3 position, float volume = 1f) {
        PlaySound(audioClipArray[UnityEngine.Random.Range(0, audioClipArray.Length)], position, volume);
    }
    public void StartMusic() {
        MenuController.Instance.FadeInMusic();
    }
    private void PlaySound(AudioClip audioClip, Vector3 position, float volume = 1f) {
        AudioSource.PlayClipAtPoint(audioClip, position, volume);
    }
}