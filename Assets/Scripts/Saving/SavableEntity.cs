using System;
using UnityEditor;
using UnityEngine;

namespace RPG.Saving {
    [ExecuteAlways]
    public class SavableEntity : MonoBehaviour {
        [SerializeField] private string uniqueId;

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
            if(!gameObject.scene.IsValid()) return;
            
            var sObject = new SerializedObject(this);
            var property = sObject.FindProperty("uniqueId");

            if (string.IsNullOrEmpty(property.stringValue)) {
                property.stringValue = Guid.NewGuid().ToString();
                sObject.ApplyModifiedProperties();
            }
        }
    }
}
