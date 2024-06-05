using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour {
    public Image healtFill;
    public TextMeshProUGUI actualAmmo;
    public TextMeshProUGUI  remainingAmmo;

    public void UpdateHealth(float currentHealth, float maxHealth) {
        float ratio = currentHealth / maxHealth;
        healtFill.fillAmount = ratio;
    }

    public void UpdateAmmo(int actualAmmo, int remainingAmmo) {
        this.actualAmmo.text = actualAmmo.ToString();
        this.remainingAmmo.text = remainingAmmo.ToString();
    }

}
