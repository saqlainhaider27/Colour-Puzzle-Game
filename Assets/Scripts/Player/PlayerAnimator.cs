using UnityEngine;

public class PlayerAnimator : MonoBehaviour {
    
    private Animator animator;
    private Player player;

    private const string MOVING = "Moving";

    private void Awake() {
        player = GetComponentInParent<Player>();
        animator = GetComponent<Animator>();
    }
    private void Update() {

        animator.SetBool(MOVING, player.CanPlayerMove());
    }
}
