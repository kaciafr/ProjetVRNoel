using UnityEngine;
using UnityEngine.UI;

namespace Kacia
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
            
            // ✅ DEBUG complet au démarrage
            Rigidbody rb = GetComponent<Rigidbody>();
            Collider col = GetComponent<Collider>();
            
            Debug.Log($"=== 👾 {gameObject.name} Ennemy Setup ===");
            Debug.Log($"Tag: '{gameObject.tag}'");
            Debug.Log($"Layer: {LayerMask.LayerToName(gameObject.layer)}");
            Debug.Log($"PV: {currentHealth}/{maxHealth}");
            Debug.Log($"Rigidbody: {(rb != null ? $"✅ (kinematic: {rb.isKinematic}, gravity: {rb.useGravity})" : "❌ MANQUANT")}");
            Debug.Log($"Collider: {(col != null ? $"✅ (isTrigger: {col.isTrigger}, enabled: {col.enabled})" : "❌ MANQUANT")}");
            Debug.Log($"Animator: {(animator != null ? "✅" : "❌")}");
            Debug.Log($"HealthText: {(healthText != null ? "✅" : "❌")}");
        }

        public void TakeDamage(int damage, Vector3 hitDirection, float knockbackForce)
        {
            Debug.Log($"╔════════════════════════════════════════╗");
            Debug.Log($"║ 🎯 TakeDamage appelé sur {gameObject.name}");
            Debug.Log($"╠════════════════════════════════════════╣");
            Debug.Log($"║ isDead: {isDead}");
            Debug.Log($"║ Dégâts reçus: {damage}");
            Debug.Log($"║ Vie AVANT: {currentHealth}/{maxHealth}");
            
            if (isDead)
            {
                Debug.LogWarning($"║ ⚠️ Déjà mort, dégâts ignorés");
                Debug.Log($"╚════════════════════════════════════════╝");
                return;
            }

            currentHealth -= damage;
            Debug.Log($"║ Vie APRÈS: {currentHealth}/{maxHealth}");
            
            UpdateTextHealth();
            ApplyKnockback(hitDirection, knockbackForce);

            if (currentHealth <= 0)
            {
                Debug.Log($"║ ☠️ Vie <= 0 → Appel Die()");
                Debug.Log($"╚════════════════════════════════════════╝");
                Die();
            }
            else
            {
                Debug.Log($"║ 💔 Vie restante: {currentHealth} → Animation Hit");
                Debug.Log($"╚════════════════════════════════════════╝");
                PlayHitAnimation();
            }
        }

        private void ApplyKnockback(Vector3 direction, float force)
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            
            Debug.Log($"   🔨 ApplyKnockback:");
            Debug.Log($"      Direction: {direction}");
            Debug.Log($"      Force: {force}");
            
            if (rb != null && !rb.isKinematic)
            {
                Vector3 knockback = direction * force;
                rb.AddForce(knockback, ForceMode.Impulse);
                Debug.Log($"      ✅ Knockback appliqué: {knockback}");
            }
            else if (rb == null)
            {
                Debug.LogWarning($"      ❌ Pas de Rigidbody sur {gameObject.name}!");
            }
            else if (rb.isKinematic)
            {
                Debug.LogWarning($"      ⚠️ Rigidbody est Kinematic, pas de knockback");
            }
        }

        private void PlayHitAnimation()
        {
            if (animator != null)
            {
                animator.SetTrigger("Hit");
                Debug.Log($"      🎬 Animation 'Hit' déclenchée");
            }
            else
            {
                Debug.LogWarning($"      ⚠️ Pas d'Animator pour jouer 'Hit'");
            }
        }

        private void Die()
        {
            if (isDead)
            {
                Debug.LogWarning($"⚠️ Die() appelé mais {gameObject.name} est déjà mort!");
                return;
            }
            
            isDead = true;
            Debug.Log($"╔════════════════════════════════════════╗");
            Debug.Log($"║ ☠️ {gameObject.name} MEURT");
            Debug.Log($"╠════════════════════════════════════════╣");
            
            PlayDeathAnimation();
            
            Debug.Log($"║ 🗑️ Destruction dans {destroyDelay}s");
            Debug.Log($"╚════════════════════════════════════════╝");
            
            Destroy(gameObject, destroyDelay);
        }

        private void PlayDeathAnimation()
        {
            if (animator != null)
            {
                animator.SetTrigger("Death");
                Debug.Log($"      ☠️ Animation 'Death' déclenchée");
            }
            else
            {
                Debug.LogWarning($"      ⚠️ Pas d'Animator pour jouer 'Death'");
            }
        }

        public void UpdateTextHealth()
        {
            if (healthText != null)
            {
                healthText.text = currentHealth + "/" + maxHealth;
                Debug.Log($"      📊 UI mis à jour: {currentHealth}/{maxHealth}");
            }
            else
            {
                Debug.LogWarning($"      ⚠️ healthText non assigné!");
            }
        }

        // ✅ DEBUG: Affiche toutes les collisions
        private void OnCollisionEnter(Collision collision)
        {
            Debug.Log($"👾 {gameObject.name} a détecté collision avec: {collision.gameObject.name} (Tag: {collision.gameObject.tag})");
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log($"👾 {gameObject.name} a détecté trigger avec: {other.gameObject.name} (Tag: {other.gameObject.tag})");
        }
    }
}
