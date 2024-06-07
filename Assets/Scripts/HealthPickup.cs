using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour {
    private void OnCollisionEnter(Collision collision) {
        PlayerStats playerStats = collision.gameObject.GetComponent<PlayerStats>();
        if (playerStats != null) {
            playerStats.Heal(100);
            Destroy(gameObject);
        }
    }
}
