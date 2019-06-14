using UnityEngine;

namespace RPG.Saving {
    public class SaveWrapper : MonoBehaviour {
        private SaveSystem saveSystem;

        private const string DefaultSaveFile = "save";

        private void Start() {
            saveSystem = GetComponent<SaveSystem>();
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

            SaveSystem.Load(DefaultSaveFile);
        }
        
        public static void Save() {

            SaveSystem.Save(DefaultSaveFile);
        }
    }
}
