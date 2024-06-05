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
    private Animator animator;

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

    private bool reloading = false;

    private void Start() {
        GetReferences();
        InitilizeAmmo();
    }

    private void GetReferences() {
        hud = GetComponent<PlayerHUD>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Update() {
        if (reloading) return;
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
        reloading = true;
        animator.SetTrigger("Reload");
        int ammoNeeded = weapon.magazineSize - magazineAmmo;
        if (ammoLeft >= ammoNeeded) {
            ammoLeft -= ammoNeeded;
            magazineAmmo += ammoNeeded;
        } else {
            magazineAmmo += ammoLeft;
            ammoLeft = 0;
        }
        hud.UpdateAmmo(magazineAmmo, ammoLeft);
        Invoke(nameof(ResetReloading), 1f);
    }

    private void ResetReloading() {
        reloading = false;
    }

    private void SpawnBullet() {
        GameObject instantiatedBullet = Instantiate(bullet, spawnerPosition.position, orientation.rotation);
        Instantiate(flash, flashPosition.position, orientation.rotation);
        Rigidbody rb = instantiatedBullet.GetComponent<Rigidbody>();
        rb.AddForce(orientation.forward * bulletSpeed, ForceMode.Impulse);
        Destroy(instantiatedBullet, 5f);
    }

}
