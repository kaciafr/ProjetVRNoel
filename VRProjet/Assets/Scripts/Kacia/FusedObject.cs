using UnityEngine;

namespace Kacia
{
    public class FusedObject : MonoBehaviour
    {
        public int damage = 1;
        public float knockbackForce = 100f;
        public bool destroyOnHit = true;

        [Header("Animation")] 
        public Animator animator;

        private bool hasHit = false;

        void Start()
        {
            animator = GetComponent<Animator>();
            
            // ✅ DEBUG complet au démarrage
            Rigidbody rb = GetComponent<Rigidbody>();
            Collider col = GetComponent<Collider>();
            
            Debug.Log($"=== 🔧 {gameObject.name} Setup ===");
            Debug.Log($"Tag: {gameObject.tag}");
            Debug.Log($"Layer: {LayerMask.LayerToName(gameObject.layer)}");
            Debug.Log($"Rigidbody: {(rb != null ? $"✅ (kinematic: {rb.isKinematic}, gravity: {rb.useGravity})" : "❌ MANQUANT")}");
            Debug.Log($"Collider: {(col != null ? $"✅ (isTrigger: {col.isTrigger}, enabled: {col.enabled})" : "❌ MANQUANT")}");
            Debug.Log($"Script enabled: {enabled}");
            Debug.Log($"GameObject active: {gameObject.activeSelf}");
        }

        void OnCollisionEnter(Collision collision)
        {
            Debug.Log($"🔥 COLLISION détectée ! {gameObject.name} → {collision.gameObject.name}");
            Debug.Log($"   Tag de l'objet touché: '{collision.gameObject.tag}'");
            Debug.Log($"   hasHit actuel: {hasHit}");
            
            HandleHit(collision.gameObject, collision.transform.position);
        }

        void OnTriggerEnter(Collider other)
        {
            Debug.Log($"🔥 TRIGGER détecté ! {gameObject.name} → {other.gameObject.name}");
            Debug.Log($"   Tag de l'objet touché: '{other.gameObject.tag}'");
            Debug.Log($"   hasHit actuel: {hasHit}");
            
            HandleHit(other.gameObject, other.transform.position);
        }

        void HandleHit(GameObject hitObject, Vector3 hitPosition)
        {
            Debug.Log($"📍 HandleHit appelé pour {hitObject.name}");
            Debug.Log($"   CompareTag('Enemy'): {hitObject.CompareTag("Enemy")}");
            Debug.Log($"   hasHit: {hasHit}");
            Debug.Log($"   Condition complète: {hitObject.CompareTag("Enemy") && !hasHit}");

            if (hitObject.CompareTag("Enemy") && !hasHit)
            {
                Debug.Log("✅ Condition validée ! Traitement du hit...");
                hasHit = true;

                Ennemy ennemy = hitObject.GetComponent<Ennemy>();
                Debug.Log($"   Script Ennemy trouvé: {ennemy != null}");

                if (ennemy != null)
                {
                    Vector3 hitDirection = (hitPosition - transform.position).normalized;
                    
                    Debug.Log($"💥 Appel TakeDamage({damage}, {hitDirection}, {knockbackForce})");
                    ennemy.TakeDamage(damage, hitDirection, knockbackForce);
                    Debug.Log($"✅ TakeDamage appelé avec succès");
                }
                else
                {
                    Debug.LogError($"❌ ERREUR: {hitObject.name} a le tag 'Enemy' mais PAS de script Ennemy !");
                }

                if (destroyOnHit)
                {
                    Debug.Log($"🗑️ Destruction de {gameObject.name} programmée dans 0.2s");
                    Destroy(gameObject, 0.2f);
                }
                else
                {
                    Debug.Log($"⚠️ destroyOnHit = false, objet non détruit");
                }
            }
            else
            {
                Debug.LogWarning($"❌ Condition NON validée:");
                Debug.LogWarning($"   - Tag est 'Enemy': {hitObject.CompareTag("Enemy")}");
                Debug.LogWarning($"   - hasHit est false: {!hasHit}");
            }
        }
    }
}
