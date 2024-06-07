using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tp : MonoBehaviour {
    [SerializeField] private float speed = 150f;
    void Update() {
        RotateCube();
    }

    private void RotateCube() {
        transform.Rotate(Vector3.up + Vector3.right, speed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision) {
        UIManager uimanager = collision.gameObject.GetComponent<UIManager>();
        PlayerStats playerStats = collision.gameObject.GetComponent<PlayerStats>();
        if (uimanager != null) {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            playerStats.SetEndTimer();
            playerStats.Die();
            uimanager.SetActiveHud(HUDStates.end);
        }
    }

}
