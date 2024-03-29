using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.Saving {
    public class SaveSystem : MonoBehaviour {
        private const string lastSceneBuildIndex = "lastSceneBuildIndex";
        private const string sav = ".sav";

        public static IEnumerator LoadLastScene(string saveFile) {
            var state = LoadFile(saveFile);
            
            if (!state.ContainsKey(lastSceneBuildIndex)) yield break;
            
            var buildIndex = (int) state[lastSceneBuildIndex];
            
            if (buildIndex != SceneManager.GetActiveScene().buildIndex) {
                yield return SceneManager.LoadSceneAsync(buildIndex);
            }

            RestoreState(state);
        }

        public static void Save(string saveFile) {
            var state = LoadFile(saveFile);
            CaptureState(state);
            SaveFile(saveFile, state);
        }

        private static void SaveFile(string saveFile, object state) {
            var path = GetPathFromSaveFile(saveFile);

            using (var stream = File.Open(path, FileMode.Create)) {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, state);
            }
        }

        public static void Load(string saveFile) {
            RestoreState(LoadFile(saveFile));
        }

        private static Dictionary<string, object> LoadFile(string saveFile) {
            var path = GetPathFromSaveFile(saveFile);

            if (!File.Exists(path)) return new Dictionary<string, object>();

            using (var stream = File.Open(path, FileMode.Open)) {
                var formatter = new BinaryFormatter();

                return (Dictionary<string, object>) formatter.Deserialize(stream);
            }
        }

        private static void CaptureState(IDictionary<string, object> state) {
            foreach (var savable in FindObjectsOfType<SavableEntity>()) {
                state[savable.GetUniqueId()] = savable.CaptureState();
            }

            state[lastSceneBuildIndex] = SceneManager.GetActiveScene().buildIndex;
        }

        private static void RestoreState(IReadOnlyDictionary<string, object> state) {
            foreach (var savable in FindObjectsOfType<SavableEntity>()) {
                var id = savable.GetUniqueId();

                if (state.ContainsKey(id)) {
                    savable.RestoreState(state[id]);
                }
            }
        }

        private static string GetPathFromSaveFile(string saveFile) {
            return Path.Combine(Application.persistentDataPath, saveFile + sav);
        }
    }
}