using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace RPG.Saving {
    public class SaveSystem : MonoBehaviour {
        private Transform playerTransform;

        private void Start() {
            playerTransform = GameObject.FindWithTag("Player").transform;
        }
        
        public void Save(string saveFile) {
            var path = GetPathFromSaveFile(saveFile);
            print("Saving to " + path);
            
            using (var stream = File.Open(path, FileMode.Create)) {
                
                var formatter = new BinaryFormatter();
                var position = new SerializableVector3(playerTransform.position);
                formatter.Serialize(stream, position);
                
            }
        }
        

        public void Load(string saveFile) {
            var path = GetPathFromSaveFile(saveFile);
            print("Loading from " + path);
            
            using (var stream = File.Open(path, FileMode.Open)) {
                var formatter = new BinaryFormatter();

                var position = (SerializableVector3) formatter.Deserialize(stream);
                playerTransform.position = position.ToVector3();
            }
        }

        private byte[] SerializeVector(Vector3 vector) {
            var vectorBytes = new byte[3*4];
            BitConverter.GetBytes(vector.x).CopyTo(vectorBytes,0);
            BitConverter.GetBytes(vector.y).CopyTo(vectorBytes,4);
            BitConverter.GetBytes(vector.z).CopyTo(vectorBytes,8);
            return vectorBytes;
        }

        private Vector3 DeSerializeVector(byte[] buffer) {
            var vector = new Vector3 {
                x = BitConverter.ToSingle(buffer, 0),
                y = BitConverter.ToSingle(buffer, 4),
                z = BitConverter.ToSingle(buffer, 8)
            };
            return vector;
        }

        private string GetPathFromSaveFile(string saveFile) {
            return Path.Combine(Application.persistentDataPath, saveFile + ".sav");
        }
    }
}