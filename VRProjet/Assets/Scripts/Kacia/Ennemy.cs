using DG.Tweening;
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
        public Slider healthSlider;

        private void Start()
        {
            currentHealth = maxHealth;
            animator = GetComponent<Animator>();
            InitializeHealthSlider();
            Debug.Log($"👾 {gameObject.name} initialisé avec {maxHealth} PV");
        }

        public void TakeDamage(int damage, Collision collision)
        {
            if (isDead) return;

            currentHealth -= damage;
            UpdateHealthSlider();
            Debug.Log($"💥 {gameObject.name} a reçu {damage} dégâts ! Vie restante : {currentHealth}/{maxHealth}");

            if (currentHealth <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            if (isDead) return;
            isDead = true;
            Debug.Log($"☠️ {gameObject.name} est mort !");
            PlayDeathAnimation();

            var rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true;
                rb.detectCollisions = false;
            }

            transform
                .DOScale(0, 0.3f)
                .SetEase(Ease.InBack)
                .OnComplete(() => Destroy(this.gameObject));
        }

        private void PlayDeathAnimation()
        {
            if (animator != null)
            {
                animator.SetTrigger("Death");
                Debug.Log($"🎬 Animation Death déclenchée sur {gameObject.name}");
            }
        }

        private void InitializeHealthSlider()
        {
            if (healthSlider != null)
            {
                healthSlider.maxValue = maxHealth;
                healthSlider.value = currentHealth;
            }
            else
            {
                Debug.LogError("⚠ healthSlider n'est pas assigné dans l'inspecteur !");
            }
        }

        private void UpdateHealthSlider()
        {
            if (healthSlider != null)
            {
                StartCoroutine(AnimateHealthBar(currentHealth));
            }
        }

        private System.Collections.IEnumerator AnimateHealthBar(int targetHealth)
        {
            float startValue = healthSlider.value;
            float endValue = targetHealth;
            float duration = 0.3f;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                healthSlider.value = Mathf.Lerp(startValue, endValue, elapsed / duration);
                yield return null;
            }

            healthSlider.value = endValue;
        }
    }
}
