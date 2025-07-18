using UnityEngine;

public class Life : MonoBehaviour {
    [SerializeField] private GameObject fill;

    public void TakeLife() {
        fill.SetActive(false);
    }
    public void GiveLife() {
        fill.SetActive(true);
    }
}