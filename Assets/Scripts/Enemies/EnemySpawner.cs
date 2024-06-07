using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private List<Transform> spawnPoints;
    [SerializeField] private List<GameObject> enemyPrefabs;

    private void Start() {
        InitializeSpawnPoints();
        SpawnEnemies();
    }

    private void InitializeSpawnPoints() {
        spawnPoints = gameObject.GetComponentsInChildren<Transform>().Skip(1).ToList();
    }

    private void SpawnEnemies() {
        foreach (Transform t in spawnPoints) {
            Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Count)], t);
        }
    }
}
