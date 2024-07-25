using UnityEngine;

public class UIController : Singleton<UIController> {

    [SerializeField] private GameObject winMenu;
    [SerializeField] private GameObject blur;
    private void Awake() {
        HideUIBlur();
        HideWinMenu();    
    }
    public void ShowWinMenu() {
        ShowUIBlur();
        winMenu.SetActive(true);
    }
    public void HideWinMenu() { 
        HideUIBlur();
        winMenu.SetActive(false);
    }
    private void ShowUIBlur() {
        blur.SetActive(true);
    }
    private void HideUIBlur() {
        blur.SetActive(false);
    }
}

