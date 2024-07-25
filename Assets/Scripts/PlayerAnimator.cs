using UnityEngine;

public class PlayerAnimator : MonoBehaviour {
    
    private Animator animator;
    [SerializeField] private Player player;

    private const string MOVING = "Moving";

    private void Awake() {
        animator = GetComponent<Animator>();
    }
    private void Update() {

        animator.SetBool(MOVING, player.CanPlayerMove());
    }
}
