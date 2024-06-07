using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponShooting : MonoBehaviour {
    //GameObjects
    private Camera cam;
    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject flash;
    private PlayerHUD hud;
    private Animator animator;
    private PlayerStats playerStats;

    //Transforms
    [SerializeField] private Transform orientation;
    [SerializeField] private Transform spawnerPosition;
    [SerializeField] private Transform flashPosition;

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
        playerStats = GetComponentInChildren<PlayerStats>();
    }

    private void Update() {
        if (reloading || playerStats.IsDead()) return;
        if (Input.GetButtonDown("Fire1"))
            Shoot();

        if (Input.GetButtonDown("Reload"))
            Reload();
    }

    public void InitilizeAmmo() {
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
        if (ammoLeft == 0 || magazineAmmo == weapon.magazineSize) return;
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
        instantiatedBullet.GetComponent<Bullet>().SetDamage(weapon.damage);
        Rigidbody rb = instantiatedBullet.GetComponent<Rigidbody>();
        rb.AddForce(orientation.forward * bulletSpeed, ForceMode.Impulse);
        Destroy(instantiatedBullet, 5f);
    }

}
