using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponShooting : MonoBehaviour {
    //GameObjects
    private Camera cam;
    public GameObject bullet;
    public GameObject flash;
    private PlayerHUD hud;

    //Transforms
    public Transform orientation;
    public Transform spawnerPosition;
    public Transform flashPosition;

    //Weapon
    public Weapon weapon;
    public float bulletSpeed = 1000;
    private float lastShootTime = 0;
    private int magazineAmmo = 0;
    private int ammoLeft = 0;

    private void Start() {
        GetReferences();
        InitilizeAmmo();
    }

    private void GetReferences() {
        cam = Camera.main;
        hud = GetComponent<PlayerHUD>();
    }

    private void Update() {
        if (Input.GetButtonDown("Fire1"))
            Shoot();

        if (Input.GetButtonDown("Reload"))
            Reload();
    }

    private void InitilizeAmmo() {
        magazineAmmo = weapon.magazineSize;
        ammoLeft = weapon.magazineSize * weapon.magazineCount;
        hud.UpdateAmmo(magazineAmmo, ammoLeft);
    }

    private void Shoot() {
        if (magazineAmmo == 0) return;
        if (Time.time > lastShootTime + weapon.fireRate) {
            SpawnBullet();
            lastShootTime = Time.time;
            magazineAmmo--;
            hud.UpdateAmmo(magazineAmmo, ammoLeft);
        }
    }

    private void Reload() {
        int ammoNeeded = weapon.magazineSize - magazineAmmo;
        if (ammoLeft >= ammoNeeded) {
            ammoLeft -= ammoNeeded;
            magazineAmmo += ammoNeeded;
        } else {
            magazineAmmo += ammoLeft;
            ammoLeft = 0;
        }
        hud.UpdateAmmo(magazineAmmo, ammoLeft);
    }

    private void SpawnBullet() {
        GameObject instantiatedBullet = Instantiate(bullet, spawnerPosition.position, orientation.rotation);
        Instantiate(flash, flashPosition.position, orientation.rotation);
        Rigidbody rb = instantiatedBullet.GetComponent<Rigidbody>();
        rb.AddForce(orientation.forward * bulletSpeed, ForceMode.Impulse);
        Destroy(instantiatedBullet, 5f);
    }

}
