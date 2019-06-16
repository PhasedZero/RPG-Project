using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace RPG.Saving {
    [ExecuteAlways]
    public class SavableEntity : MonoBehaviour {
        [Header("Don't touch. This is auto-generated.")] [SerializeField]
        private string uniqueId;

        public string GetUniqueId() {
            return uniqueId;
        }

        public Dictionary<string, object> CaptureState() {
            return GetComponents<ISavable>()
                .ToDictionary(
                    savable => savable.GetType().ToString(),
                    savable => savable.CaptureState()
                );
        }

        public void RestoreState(object state) {
            var states = (Dictionary<string, object>) state;
            
            foreach (var savable in GetComponents<ISavable>()) {
                var type = savable.GetType().ToString();
                
                if (states.ContainsKey(type)) {
                    savable.RestoreState(states[type]);
                }
            }
        }

#if UNITY_EDITOR
        private void OnValidate() {
            print("validating " + name);
            if (string.IsNullOrEmpty(gameObject.scene.path)) return;
            if (!string.IsNullOrEmpty(uniqueId) && IsUnique()) return;

            SetGuid();
        }

        private void SetGuid() {
            var sObject = new SerializedObject(this);
            var property = sObject.FindProperty("uniqueId");

            property.stringValue = Guid.NewGuid().ToString();
            sObject.ApplyModifiedPropertiesWithoutUndo();
        }

        private bool IsUnique() {
            return FindObjectsOfType<SavableEntity>()
                .Where(entity => entity != this)
                .All(entity => entity.GetUniqueId() != uniqueId);
        }
#endif
    }
}