using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    public Image fill;

    public void UpdateHealth(float currentHealth, float maxHealth)
    {
        float ratio = currentHealth / maxHealth;
        fill.fillAmount = ratio;
    }

}
