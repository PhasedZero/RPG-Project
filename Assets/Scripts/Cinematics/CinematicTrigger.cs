﻿using System;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics {
    public class CinematicTrigger : MonoBehaviour {
        private bool alreadyTriggered = false;
        
        private void OnTriggerEnter(Collider other) {
            if (!alreadyTriggered && other.gameObject.CompareTag("Player")) {
                GetComponent<PlayableDirector>().Play();
                alreadyTriggered = true;
            }
        }
    }
}
