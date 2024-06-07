using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour {
    [SerializeField] private Image healtFill;
    [SerializeField] private TextMeshProUGUI actualAmmo;
    [SerializeField] private TextMeshProUGUI  remainingAmmo;
    [SerializeField] private TextMeshProUGUI  minutes;
    [SerializeField] private TextMeshProUGUI  seconds;
    [SerializeField] private TextMeshProUGUI  endTimer;

    public void UpdateHealth(float currentHealth, float maxHealth) {
        float ratio = currentHealth / maxHealth;
        healtFill.fillAmount = ratio;
    }

    public void UpdateAmmo(int actualAmmo, int remainingAmmo) {
        this.actualAmmo.text = actualAmmo.ToString();
        this.remainingAmmo.text = remainingAmmo.ToString();
    }

    public void UpdateTimer(int minutes, int seconds) {
        this.minutes.text = $"{minutes:00}";
        this.seconds.text = $"{seconds:00}";
    }

    public void SetEndTimer(int minutes, int seconds) {
        endTimer.text = "Time: " + $"{minutes:00}" + ":" + $"{seconds:00}"; 
    }

}
