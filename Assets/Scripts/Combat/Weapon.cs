using UnityEngine;

namespace RPG.Combat {
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]
    public class Weapon : ScriptableObject {
        [SerializeField] private AnimatorOverrideController animatorOverride = null;
        [SerializeField] private GameObject weaponPrefab = null;
        [SerializeField] private float damage = 0f;
        [SerializeField] private float range = 2f;
        [SerializeField] private bool isLeftHanded = false;

        public float GetDamage() => damage;
        public float GetRange() => range;

        public GameObject Spawn(Transform rightHand, Transform leftHand, Animator animator) {
            if (animatorOverride != null) {
                animator.runtimeAnimatorController = animatorOverride;
            }
            return weaponPrefab != null ? Instantiate(weaponPrefab, isLeftHanded ? leftHand : rightHand) : null;
        }
        
        
    }
}