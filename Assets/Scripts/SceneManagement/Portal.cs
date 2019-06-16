using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

namespace RPG.SceneManagement {
    public class Portal : MonoBehaviour {
        public enum PortalId {
            A,
            B,
            C,
            D,
            E
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

            SaveWrapper.Save();
            yield return SceneManager.LoadSceneAsync(sceneToLoad);
            SaveWrapper.Load();
            
            var otherPortal = GetOtherPortal();
            UpdatePlayer(otherPortal);
            
            SaveWrapper.Save();

            yield return new WaitForSeconds(fadeWaitTime);
            yield return fader.FadeIn(fadeInTime);

            Destroy(gameObject);
        }

        private void UpdatePlayer(Portal otherPortal) {
            var player = GameObject.FindWithTag("Player");
            var navMeshAgentEnabled = player.GetComponent<NavMeshAgent>().enabled;
            navMeshAgentEnabled = false;
            player.transform.position = otherPortal.spawnPoint.position;
            player.transform.rotation = otherPortal.spawnPoint.rotation;
            navMeshAgentEnabled = true;
        }

        private Portal GetOtherPortal() {
            return FindObjectsOfType<Portal>()
                .Where(portal => portal != this)
                .FirstOrDefault(portal => portal.portalId == portalId);
        }
    }
}