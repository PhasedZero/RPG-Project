using System;
using UnityEngine;

namespace RPG.Core {
    public class PersistentObjectsSpawner : MonoBehaviour {
        [SerializeField] private GameObject persistentObjectsPrefab;

        private static bool hasSpawned = false;

        private void Awake() {
            if (hasSpawned) return;

            SpawnPersistentObjects();

            hasSpawned = true;
        }
        private void SpawnPersistentObjects() {
            GameObject persistentObject = Instantiate(persistentObjectsPrefab);
            DontDestroyOnLoad(persistentObject);
        }
    }
}
