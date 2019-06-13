using System;
using System.IO;
using UnityEngine;

namespace RPG.Saving {
    public class SaveSystem : MonoBehaviour {

        public void Save(string saveFile) {
            var path = GetPathFromSaveFile(saveFile);
            print("Saving to " + path);
            
            var stream = File.Open(path, FileMode.Create);
            var bytes = new byte[]{0xc2, 0xa1, 72, 111, 108, 97, 32, 77, 117, 110, 100, 111, 33};
            stream.Write(bytes, 0, bytes.Length);
            stream.Close();
        }
        
        public void Load(string saveFile) {
            var path = GetPathFromSaveFile(saveFile);
            print("Loading from " + path);
        }
        
        private string GetPathFromSaveFile(string saveFile) {
            return Path.Combine(Application.persistentDataPath, saveFile + ".sav");
        }
    }
}
