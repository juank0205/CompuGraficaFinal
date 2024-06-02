using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterStats
{
    private PlayerHUD playerHUD;

    private void Start()
    {
        GetReferences();
        InitVariables();
    }

    private void GetReferences()
    {
        playerHUD = GetComponent<PlayerHUD>();
    }

    public override void CheckHealth()
    {
        base.CheckHealth();
        playerHUD.UpdateHealth(health, maxHealth);
    }
}
