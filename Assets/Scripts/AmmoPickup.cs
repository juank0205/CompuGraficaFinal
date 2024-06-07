using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision) {
        WeaponShooting weaponShooting = collision.gameObject.GetComponent<WeaponShooting>();
        if (weaponShooting != null) {
            weaponShooting.InitilizeAmmo();
            Destroy(gameObject);
        }
    }
}
