using System;
using UnityEngine;

namespace RPG.Saving {
    [ExecuteAlways]
    public class SavableEntity : MonoBehaviour {
        [SerializeField] private string uniqueId = Guid.NewGuid().ToString();

        public string GetUniqueId() {
            return uniqueId;
        }

        public object CaptureState() {
            print("Capturing state for " + GetUniqueId());
            return null;
        }

        public void RestoreState(object state) {
            print("Restoring state for " + GetUniqueId());
        }
        
        private void Update() {
            if (Application.isPlaying) return;
            
            print("Editing....");
        }
    }
}
