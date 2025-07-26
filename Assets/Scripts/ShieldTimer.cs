using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShieldTimer : MonoBehaviour {
    private float m_Time = 0;
    private float waitDuration = 3f;
    private bool timerComplete = false;
    [SerializeField] private Image fillImage;
    [SerializeField] private TextMeshProUGUI textMesh;
    [SerializeField] private Button button;
    private Animator animator;
    private const string PLAY = "Play";
    private bool used = false;
    
    private void Awake() {
        if (ShieldController.Instance.Shield == 0) {
            Destroy(this.gameObject);
        }
        animator = GetComponent<Animator>();
        textMesh.text = ShieldController.Instance.Shield.ToString();
    }
    private void Start() {
        ShieldController.Instance.OnShieldChanged += ShieldController_OnShieldChanged;
    }

    private void ShieldController_OnShieldChanged(int obj) {
        textMesh.text = obj.ToString();
    }
    private void OnEnable() {
        button.onClick.AddListener(OnUseShieldButtonClicked);
    }
    private void OnDisable() {
        button.onClick.RemoveAllListeners();
    }

    private void OnUseShieldButtonClicked() {
        if (used) {
            return;
        }
        used = true;
        m_Time = waitDuration;
        ShieldController.Instance.Shield -= 1;
        Player.Instance.UseShield();
        StartCoroutine(DisableSelfAfterDelay());
    }

    private void Update() {
        if (timerComplete) return;
        if (m_Time < waitDuration) {
            m_Time += Time.deltaTime;
            float normalizedTime = Mathf.Max(m_Time / waitDuration, 0);
            fillImage.fillAmount = 1 - normalizedTime;
        } else {
            animator.SetTrigger(PLAY);
            timerComplete = true;
            StartCoroutine(DisableSelfAfterDelay());
            
        }
    }

    private IEnumerator DisableSelfAfterDelay(float delay = 0.5f) {
        yield return new WaitForSeconds(delay);
        Destroy(this.gameObject);
    }
}