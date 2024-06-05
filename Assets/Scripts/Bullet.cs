using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject splash;

    private void OnCollisionEnter(Collision collision) {
        Instantiate(splash, gameObject.transform.position, gameObject.transform.rotation);
        Destroy(gameObject);
    }
}
