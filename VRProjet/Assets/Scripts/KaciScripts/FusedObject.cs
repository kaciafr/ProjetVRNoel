using UnityEngine;

public class FusedObject : MonoBehaviour
{
    public bool destroyOnHit = true;

    public Animator animator;

    private bool isDead = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log($"{gameObject.name} a touché un enneims : {collision.gameObject.name}");
            
            
            Destroy(collision.gameObject);

            if (destroyOnHit)
            {
                Destroy(gameObject);
            }
        }
    }

    public void Die()
    {
        isDead = true;
        Debug.Log($"{gameObject.name} est mort" );

        if (animator != null)
        {
            animator.SetTrigger("Die");
        }
        
        Collider
    }
}
