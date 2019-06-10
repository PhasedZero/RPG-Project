using RPG.Control;
using RPG.Core;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics {
    public class CinematicControlRemover : MonoBehaviour {
        private GameObject player;

        private void Start() {
            var playableDirector = GetComponent<PlayableDirector>();
            playableDirector.played += DisableControl;
            playableDirector.stopped += EnableControl;
            player = GameObject.FindWithTag("Player");
        }

        private void DisableControl(PlayableDirector pd) {
            player.GetComponent<ActionScheduler>().CancelCurrentAction();
            player.GetComponent<PlayerController>().enabled = false;
        }

        void EnableControl(PlayableDirector pd) {
            player.GetComponent<PlayerController>().enabled = true;
        }
    }
}
