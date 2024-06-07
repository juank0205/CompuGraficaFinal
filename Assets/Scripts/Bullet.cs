using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private GameObject splash;
    private int damage;
    private bool hasCollided = false;

    public void SetDamage(int damage) {
        this.damage = damage;
    }

    private void OnCollisionEnter(Collision collision) {
        if (hasCollided) return;
        hasCollided = true;
        Instantiate(splash, gameObject.transform.position, gameObject.transform.rotation);
        CharacterStats hitStats = collision.gameObject.GetComponent<CharacterStats>();
        if ( hitStats != null) {
            hitStats.TakeDamage(damage);
        }
        Destroy(gameObject);
    }
}
