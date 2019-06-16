using UnityEngine;

namespace RPG.Combat {
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]
    public class Weapon : ScriptableObject {
        [SerializeField] private AnimatorOverrideController animatorOverride = null;
        [SerializeField] private GameObject weaponPrefab = null;
        [SerializeField] private float damage = 0f;
        [SerializeField] private float range = 2f;

        public float GetDamage() => damage;
        public float GetRange() => range;

        public void Spawn(Transform handTransform, Animator animator) {
            if (weaponPrefab != null) {
                Instantiate(weaponPrefab, handTransform);
            }

            if (animatorOverride != null) {
                animator.runtimeAnimatorController = animatorOverride;
            }
        }
    }
}