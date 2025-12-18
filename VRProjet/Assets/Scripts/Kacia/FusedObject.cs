using UnityEngine;

namespace Kacia
{
    public class FusedObject : MonoBehaviour
    {
        public int damage = 1;
        public float knockbackForce = 100f;
        public bool destroyOnHit = true;

        [Header("Animation")] public Animator animator;

        private bool hasHit = false;

        void Start()
        {
            animator = GetComponent<Animator>();
        }

        void OnCollisionEnter(Collision collision)
        {
            Debug.Log($"{gameObject.name} a touché : {collision.gameObject.name} (Tag: {collision.gameObject.tag})");

            if (collision.gameObject.CompareTag("Enemy") && !hasHit)
            {
                hasHit = true;

                Ennemy ennemy = collision.gameObject.GetComponent<Ennemy>();

                if (ennemy != null)
                {
                    Debug.Log($" Script Ennemy trouvé sur {collision.gameObject.name}");

                    Vector3 hitDirection = (collision.transform.position - transform.position).normalized;
                    ennemy.TakeDamage(damage, hitDirection, knockbackForce);
                }
                else
                {
                    Debug.LogError($" {collision.gameObject.name} n'a PAS de script Ennemy !");
                }

                if (destroyOnHit)
                {
                    Destroy(gameObject, 0.2f);
                }
            }
        }
    }
}