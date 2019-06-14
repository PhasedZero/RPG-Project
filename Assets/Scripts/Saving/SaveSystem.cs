using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using Object = UnityEngine.Object;

namespace RPG.Saving {
    public class SaveSystem : MonoBehaviour {
        
        public void Save(string saveFile) {
            var path = GetPathFromSaveFile(saveFile);
            print("Saving to " + path);
            
            using (var stream = File.Open(path, FileMode.Create)) {
                
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, CaptureState());
            }
        }
        
        public void Load(string saveFile) {
            var path = GetPathFromSaveFile(saveFile);
            print("Loading from " + path);
            
            using (var stream = File.Open(path, FileMode.Open)) {
                var formatter = new BinaryFormatter();

                RestoreState(formatter.Deserialize(stream));
            }
        }

        private object CaptureState() {
            var state = new Dictionary<string, object>();
            foreach (var savable in FindObjectsOfType<SavableEntity>()) {
                state[savable.GetUniqueId()] = savable.CaptureState();
            }
            return state;
        }
        
        private void RestoreState(object state) {
            var dictionary = (Dictionary<string, object>) state;
            foreach (var savable in FindObjectsOfType<SavableEntity>()) {
                savable.RestoreState(dictionary[savable.GetUniqueId()]);
            }
        }

        private string GetPathFromSaveFile(string saveFile) {
            return Path.Combine(Application.persistentDataPath, saveFile + ".sav");
        }
    }
}