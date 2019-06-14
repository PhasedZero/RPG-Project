using System;
using System.Linq;
using RPG.Core;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Saving {
    [ExecuteAlways]
    public class SavableEntity : MonoBehaviour {
        [Header("Don't touch. This is auto-generated.")]
        [SerializeField] private string uniqueId;
        
        public string GetUniqueId() {
            return uniqueId;
        }

        public object CaptureState() {
            return new SerializableVector3(transform.position);
        }

        public void RestoreState(object state) {
            var serializableVector3 = (SerializableVector3) state;
            GetComponent<ActionScheduler>().CancelCurrentAction();
            GetComponent<NavMeshAgent>().enabled = false;
            transform.position = serializableVector3.ToVector3();
        }

#if UNITY_EDITOR
        private void OnValidate() {
            if (!gameObject.scene.IsValid()) return;
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
            var savableEntities = FindObjectsOfType<SavableEntity>();

            return savableEntities
                .Where(entity => entity != this)
                .All(entity => entity.GetUniqueId() != uniqueId);
        }
    }
#endif
}
