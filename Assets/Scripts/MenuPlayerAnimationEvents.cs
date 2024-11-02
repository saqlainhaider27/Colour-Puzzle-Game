using UnityEngine;


public class MenuPlayerAnimationEvents : MonoBehaviour {


    [SerializeField] private GameObject animationMenu;
    [SerializeField] private AudioSource colourChangeAS;
    [SerializeField] private ParticleSystem particle;

    public void PlayParticles() {
        particle.Play();
    }
    public void ShowMainMenu() {
        animationMenu.SetActive(false);
        MenuController.Instance.ShowMainMenu();
        MenuController.Instance.InvokeMusicFadeIn();
    }
    public void PlayColourChangeAudio() {
        colourChangeAS.Play();
    }
}
