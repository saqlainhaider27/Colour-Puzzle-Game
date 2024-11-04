using UnityEngine;

public class Menu : MonoBehaviour {
    public void ShowMenu() {
        this.gameObject.SetActive(true);
    }
    
    public void HideMenu() {
        this.gameObject.SetActive(false);
    }


}
