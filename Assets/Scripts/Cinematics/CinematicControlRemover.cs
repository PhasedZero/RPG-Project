using System;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics {
    public class CinematicControlRemover: MonoBehaviour {
        private void Start() {
            var playableDirector = GetComponent<PlayableDirector>();
            playableDirector.played += DisableControl;
            playableDirector.stopped += EnableControl;
        }

        void DisableControl(PlayableDirector pd) {
            print("DisableControl");
        }
        
        void EnableControl(PlayableDirector pd) {
            print("EnableControl");
        }
    }
}
