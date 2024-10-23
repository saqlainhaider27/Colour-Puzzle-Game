using UnityEngine;

public class StarAnimator : MonoBehaviour {
    private const string STAR_UI_ENTRY = "StarUIEntry";
    private Animator animator;
    private void Awake() {
        
        animator = GetComponent<Animator>();
    }
    public void PlayEntryAnimation() {
        animator.Play(STAR_UI_ENTRY);
    }
}
