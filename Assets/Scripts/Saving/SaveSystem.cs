using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using Object = UnityEngine.Object;

namespace RPG.Saving {
    public class SaveSystem : MonoBehaviour {
        
        public void Save(string saveFile) {

            SaveFile(saveFile, CaptureState());
        }
        
        private void SaveFile(string saveFile, object state) {
            var path = GetPathFromSaveFile(saveFile);
            print("Saving to " + path);
            
            using (var stream = File.Open(path, FileMode.Create)) {
                
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, state);
            }
        }

        public void Load(string saveFile) {
            
            RestoreState(LoadFile(saveFile));
        }
        
        private Dictionary<string, object> LoadFile(string saveFile) {
            var path = GetPathFromSaveFile(saveFile);
            print("Loading from " + path);
            
            using (var stream = File.Open(path, FileMode.Open)) {
                var formatter = new BinaryFormatter();

                return (Dictionary<string, object>) formatter.Deserialize(stream);
            }
        }

        private static Dictionary<string, object> CaptureState() {
            var state = new Dictionary<string, object>();
            foreach (var savable in FindObjectsOfType<SavableEntity>()) {
                state[savable.GetUniqueId()] = savable.CaptureState();
            }
            return state;
        }
        
        private static void RestoreState(IReadOnlyDictionary<string, object> state) {
            foreach (var savable in FindObjectsOfType<SavableEntity>()) {
                savable.RestoreState(state[savable.GetUniqueId()]);
            }
        }

        private string GetPathFromSaveFile(string saveFile) {
            return Path.Combine(Application.persistentDataPath, saveFile + ".sav");
        }
    }
}