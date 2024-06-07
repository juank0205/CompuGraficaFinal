using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour {
    [SerializeField] protected int health;
    [SerializeField] protected int maxHealth;

    [SerializeField] protected bool isDead;

    public virtual void CheckHealth() {
        if (health <= 0) {
            health = 0;
            Die();
        }
        if (health >= maxHealth)
            health = maxHealth;
    }

    public virtual void Die() {
        isDead = true;
    }

    private void SetHealth(int healthTo) {
        health = healthTo;
        CheckHealth();
    }

    public void TakeDamage(int damage) {
        int healthAfterDamage = health - damage;
        SetHealth(healthAfterDamage);
    }

    public void Heal(int heal) {
        int healthAfterHeal = health + heal;
        SetHealth(healthAfterHeal);
    }

    public void InitVariables() {
        maxHealth = 100;
        SetHealth(maxHealth);
        isDead = false;
    }

    private void Start() {
        InitVariables();
    }
}
