using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.SceneManagement {
    public class Portal : MonoBehaviour {
        public enum PortalId {
            A, B, C, D, E
        }

        [SerializeField] private int sceneToLoad = -1;
        [SerializeField] private Transform spawnPoint;
        [SerializeField] public PortalId portalId;
        
        [SerializeField] private float fadeOutTime = 2f;
        [SerializeField] private float fadeInTime = 1f;
        [SerializeField] private float fadeWaitTime = 1f;

        private void OnTriggerEnter(Collider other) {
            if (other.CompareTag("Player")) {
                StartCoroutine(Transition());
            }
        }

        private IEnumerator Transition() {
            if (sceneToLoad < 0) {
                Debug.LogError("Scene to load not set.");
                yield break;
            }

            var fader = FindObjectOfType<Fader>();
            yield return fader.FadeOut(fadeOutTime);
            
            DontDestroyOnLoad(gameObject);
            yield return SceneManager.LoadSceneAsync(sceneToLoad);

            Portal otherPortal = GetOtherPortal();
            UpdatePlayer(otherPortal);
            
            yield return new WaitForSeconds(fadeWaitTime);
            yield return fader.FadeIn(fadeInTime);
            
            Destroy(gameObject);
        }

        private void UpdatePlayer(Portal otherPortal) {
            var player = GameObject.FindWithTag("Player");
            player.transform.position = otherPortal.spawnPoint.position;
            player.transform.rotation = otherPortal.spawnPoint.rotation;
        }

        private Portal GetOtherPortal() {
            foreach (Portal portal in FindObjectsOfType<Portal>()) {
                if (portal == this) continue;
                if (portal.portalId == portalId) {
                    return portal;
                }
            }
            return null;
        }
    }
}
