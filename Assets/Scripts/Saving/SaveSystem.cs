using System;
using System.IO;
using UnityEngine;

namespace RPG.Saving {
    public class SaveSystem : MonoBehaviour {

        public void Save(string saveFile) {
            print("Saving to " + GetPathFromSaveFile("save"));
        }
        
        public void Load(string saveFile) {
            print("Loading from " + GetPathFromSaveFile("save"));
        }
        
        private string GetPathFromSaveFile(string savefile) {
            return Path.Combine(Application.persistentDataPath, savefile + ".sav");
        }
    }
}
