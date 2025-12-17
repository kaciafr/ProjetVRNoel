using UnityEngine;
using UnityEngine.UI;

namespace KaciScripts
{
    public class Ennemy : MonoBehaviour
    {
        [SerializeField] private int maxHealth = 3;
        private int currentHealth;
        private bool isDead = false;
        [SerializeField] private float destroyDelay = 2f;
        public Animator animator;
        public Text healthText; 

        private void Start()
        {
            currentHealth = maxHealth;
            animator = GetComponent<Animator>();
            UpdateTextHealth();
            Debug.Log($"👾 {gameObject.name} initialisé avec {maxHealth} PV");
        }

        public void TakeDamage(int damage, Vector3 hitDirection, float knockbackForce)
        {
            if (isDead) return;

            currentHealth -= damage;
            UpdateTextHealth();
            Debug.Log($" {gameObject.name} a reçu {damage} dégâts ! Vie restante : {currentHealth}/{maxHealth}");

            ApplyKnockback(hitDirection, knockbackForce);

            if (currentHealth <= 0)
            {
                Die();
            }
            else
            {
                PlayHitAnimation();
            }
        }

        private void ApplyKnockback(Vector3 direction, float force)
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null && !rb.isKinematic)
            {
                rb.AddForce(direction * force, ForceMode.Impulse);
                Debug.Log($" Knockback appliqué : {direction * force}");
            }
            else
            {
                Debug.LogWarning($" {gameObject.name} n'a pas de Rigidbody ou est Kinematic !");
            }
        }

        private void PlayHitAnimation()
        {
            if (animator != null)
            {
                animator.SetTrigger("Hit");
            }
        }

        private void Die()
        {
            if (isDead) return;
            isDead = true;
            Debug.Log($"☠️ {gameObject.name} est mort !");
            PlayDeathAnimation();
            Destroy(gameObject, destroyDelay);
        }

        private void PlayDeathAnimation()
        {
            if (animator != null)
            {
                animator.SetTrigger("Death");
                Debug.Log($" Animation Death déclenchée sur {gameObject.name}");
            }
        }

        public void UpdateTextHealth()
        {
            if (healthText != null)
            {
                healthText.text = currentHealth + "/" + maxHealth;
            }
            else
            {
                Debug.LogError("⚠ healthText n'est pas assigné dans l'inspecteur !");
            }
        }
    }
}
