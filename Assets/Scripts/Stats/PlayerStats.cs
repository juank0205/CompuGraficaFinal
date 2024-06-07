using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class PlayerStats : CharacterStats
{
    private PlayerHUD playerHUD;
    public Transform respawnPoint;
    private UIManager uIManager;

    private float playTime = 0;

    private void Start()
    {
        GetReferences();
        InitVariables();
    }

    private void Update() {
        CountTime();
    }

    private void CountTime() {
        playTime += Time.deltaTime;
        int minutes = Mathf.FloorToInt(playTime / 60);
        int seconds = Mathf.FloorToInt(playTime % 60);
        playerHUD.UpdateTimer(minutes, seconds);
    }

    public void SetEndTimer() {
        playTime += Time.deltaTime;
        int minutes = Mathf.FloorToInt(playTime / 60);
        int seconds = Mathf.FloorToInt(playTime % 60);
        playerHUD.SetEndTimer(minutes, seconds);
    }

    private void GetReferences()
    {
        playerHUD = GetComponent<PlayerHUD>();
        uIManager = GetComponent<UIManager>();
    }

    public override void CheckHealth()
    {
        base.CheckHealth();
        playerHUD.UpdateHealth(health, maxHealth);
    }

    public bool IsDead() {
        return isDead; 
    }

    private void LockCursor() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void UnlockCursor() {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    public override void Die() {
        base.Die();
        UnlockCursor(); 
        uIManager.SetActiveHud(HUDStates.dead);
    }
}
