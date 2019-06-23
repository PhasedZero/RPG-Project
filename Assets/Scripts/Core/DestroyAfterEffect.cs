using System;
using UnityEngine;

namespace RPG.Core {
    public class DestroyAfterEffect : MonoBehaviour {
        [SerializeField] private ParticleSystem fxSystem;
        private void Update() {
            if (!fxSystem.IsAlive()) {
                Destroy(gameObject);
            }
        }
    }
}