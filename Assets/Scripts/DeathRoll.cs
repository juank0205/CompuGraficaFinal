using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathRoll : MonoBehaviour {
    [SerializeField] private Transform Point1;
    [SerializeField] private Transform Point2;
    [SerializeField] private float speed;
    private bool hasCollided;

    void Start() {
        transform.position = Point1.position;
        StartCoroutine(Move());
    }

    private IEnumerator Move() {
        Vector3 startPosition = Point1.position;
        Vector3 endPosition = Point2.position;
        float distanceBetweenPoints = Vector3.Distance(startPosition, endPosition);
        float startTime = Time.time;

        while (true) {
            float distCovered = (Time.time - startTime) * speed;
            float fractionOfJourney = distCovered / distanceBetweenPoints;

            transform.position = Vector3.Lerp(startPosition, endPosition, fractionOfJourney);

            if (fractionOfJourney >= 1.0f) {
                var temp = startPosition;
                startPosition = endPosition;
                endPosition = temp;
                distanceBetweenPoints = Vector3.Distance(startPosition, endPosition);
                startTime = Time.time;
            }
            yield return null;
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if (hasCollided) return;
        hasCollided = true;
        CharacterStats hitStats = collision.gameObject.GetComponent<CharacterStats>();
        if ( hitStats != null) {
            hitStats.Die();
        }
    }

}
