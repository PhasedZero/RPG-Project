using UnityEngine;

namespace RPG.Saving {
    public class SavableEntity : MonoBehaviour {
         public string GetUniqueId() {
             return "";
         }
         
         public object CaptureState() {
             print("Capturing state for " + GetUniqueId());
             return null;
         }
         
         public void RestoreState(object state) {
             print("Restoring state for " + GetUniqueId());
         }
    }
}
