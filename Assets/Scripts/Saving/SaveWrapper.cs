using UnityEngine;

namespace RPG.Saving {
    public class SaveWrapper : MonoBehaviour {
        private SaveSystem saveSystem;

        private const string defaultSaveFile = "save";

        private void Start() {
            saveSystem = GetComponent<SaveSystem>();
        }
        private void Update() {
            if (Input.GetKeyDown(KeyCode.S)) {
                saveSystem.Save(defaultSaveFile);
            }
            if (Input.GetKeyDown(KeyCode.L)) {
                saveSystem.Load(defaultSaveFile);
            }
        }
    }
}
