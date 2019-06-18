using RPG.Saving;
using UnityEngine;

namespace RPG.SceneManagement {
    public class SaveWrapper : MonoBehaviour {
        [SerializeField] private float fadeInTime = 1f;
        
        private const string defaultSaveFile = "save";

        private void OnEnable() {
            var fader = FindObjectOfType<Fader>();
            fader.FadeOutImmediate();
        }

        private void Start() {
            StartCoroutine(SaveSystem.LoadLastScene(defaultSaveFile));
            var fader = FindObjectOfType<Fader>();
            StartCoroutine(fader.FadeIn(fadeInTime));
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