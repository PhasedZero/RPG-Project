using System;
using System.IO;
using System.Text;
using UnityEngine;

namespace RPG.Saving {
    public class SaveSystem : MonoBehaviour {

        public void Save(string saveFile) {
            var path = GetPathFromSaveFile(saveFile);
            print("Saving to " + path);
            
            using (var stream = File.Open(path, FileMode.Create)) {
                var bytes = Encoding.UTF8.GetBytes("¡Hola Mundo!");
                stream.Write(bytes, 0, bytes.Length);
            }
        }

        public void Load(string saveFile) {
            var path = GetPathFromSaveFile(saveFile);
            print("Loading from " + path);
            
            using (var stream = File.Open(path, FileMode.Open)) {
                var buffer = new byte[stream.Length];
                stream.Read(buffer,0,buffer.Length);
                print(Encoding.UTF8.GetString(buffer));
            }

        }

        private string GetPathFromSaveFile(string saveFile) {
            return Path.Combine(Application.persistentDataPath, saveFile + ".sav");
        }
    }
}
