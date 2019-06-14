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
                SaveSystem.Save(DefaultSaveFile);
            }
            if (Input.GetKeyDown(KeyCode.L)) {
                SaveSystem.Load(DefaultSaveFile);
            }
        }
    }
}
