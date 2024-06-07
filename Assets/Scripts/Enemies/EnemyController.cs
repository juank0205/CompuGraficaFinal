using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {
    //GameObjects
    private Transform player;
    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject flash;

    //Parameters
    public float detectionRange = 100;
    public float rotationSpeed = 5.0f;
    public float bulletSpeed = 100f;
    public float fireRate = 3f;
    private float lastShootTime = 0;
    public int damage = 50;

    //Transforms
    [SerializeField] private Transform orientation;
    [SerializeField] private Transform spawnerPosition;
    [SerializeField] private Transform flashPosition;

    private void Start() {
        GetReferences();
    }

    private void Update() {
        if (CalculateDistanceToPlayer() <= detectionRange) {
            LookAtPlayer();
            Shoot();
        }
    }

    private void GetReferences() {
        player = GameObject.Find("Player").transform;
    }

    private void LookAtPlayer() {
        Vector3 direction = player.position - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed);
    }

    private void Shoot() {
        if (Time.time > lastShootTime + fireRate) {
            SpawnBullet();
            lastShootTime = Time.time;
            fireRate = Random.Range(1, fireRate);
        }
    }

    private void SpawnBullet() {
        GameObject instantiatedBullet = Instantiate(bullet, spawnerPosition.position, orientation.rotation);
        Instantiate(flash, flashPosition.position, orientation.rotation);
        instantiatedBullet.GetComponent<Bullet>().SetDamage(damage);
        Rigidbody rb = instantiatedBullet.GetComponent<Rigidbody>();
        rb.AddForce(orientation.forward * bulletSpeed, ForceMode.Impulse);
        Destroy(instantiatedBullet, 5f);
    }

    private float CalculateDistanceToPlayer() {
        return Vector3.Distance(transform.position, player.position);
    }


}
