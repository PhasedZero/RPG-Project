using UnityEngine;

namespace RPG.Saving {
    public class SaveWrapper : MonoBehaviour {
        private const string defaultSaveFile = "save";

        private void OnEnable() {
            StartCoroutine(SaveSystem.LoadLastScene(defaultSaveFile));
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.S)) {
                Save();
            }

            if (Input.GetKeyDown(KeyCode.L)) {
                Load();
            }
        }

        public static void Load() {
            SaveSystem.Load(defaultSaveFile);
        }

        public static void Save() {
            SaveSystem.Save(defaultSaveFile);
        }
    }
}