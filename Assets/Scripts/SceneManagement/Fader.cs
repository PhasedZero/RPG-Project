using System.Collections;
using UnityEngine;

namespace RPG.SceneManagement {
    public class Fader : MonoBehaviour {
        [SerializeField] private float timeToFade = 2f;
        
        private CanvasGroup canvasGroup;
        
        
        private void Start() {
            canvasGroup = GetComponent<CanvasGroup>();

            StartCoroutine(FadeIn(timeToFade));
            
        }

        public IEnumerator FadeOut(float time) {
            while (canvasGroup.alpha < 1) {
                canvasGroup.alpha += Time.deltaTime / timeToFade;
                yield return null;
            }
        }

        public IEnumerator FadeIn(float time) {
            while (canvasGroup.alpha > 0) {
                canvasGroup.alpha -= Time.deltaTime / timeToFade;
                yield return null;
            }
        }
    }
}
