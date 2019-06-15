using System;
using RPG.Saving;
using UnityEngine;

namespace RPG.Core {
    public class Health : MonoBehaviour, ISavable {
        [SerializeField] private float healthPoints = 100f;

        public bool IsDead { get; private set; } = false;

        public void TakeDamage(float damage) {
            healthPoints = Mathf.Max(healthPoints - damage, 0f);
            print(healthPoints);
            if (healthPoints <= 0) {
                DeathSequence();
            }
        }

        private void DeathSequence() {
            if (IsDead) return;
            GetComponent<Animator>().SetBool("death",true);
            IsDead = true;
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        public object CaptureState() {
            return healthPoints;
        }

        public void RestoreState(object state) {
            healthPoints = (float) state;
            if (healthPoints <= 0) {
                DeathSequence();
            }
            else {
                GetComponent<Animator>().SetBool("death",false);
                IsDead = false;
            }
        }
    }
}