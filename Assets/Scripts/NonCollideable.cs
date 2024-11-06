using UnityEngine;

public class NonCollideable : MonoBehaviour {

    public void OnCollided() {
        // Disable the collider until player passes by
        GetComponent<Collider2D>().enabled = false;
        // Reset after sometime
        Invoke(nameof(ResetCollider), 0.1f);
    }
    public void ResetCollider() {
        GetComponent<Collider2D>().enabled = true;
    }
    
}
