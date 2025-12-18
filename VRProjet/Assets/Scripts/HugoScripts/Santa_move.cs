using UnityEngine;
using System.Collections;

public class Santa : MonoBehaviour
{
    [Header("Déplacement")]
    public float distance = 3f;        // Distance entre derrière et devant
    public float speed = 2f;           // Vitesse de déplacement

    [Header("Pause")]
    public float waitTime = 1f;         // Temps d'attente avant de repartir

    private Vector3 backPosition;
    private Vector3 forwardPosition;

    void Start()
    {
        // Spawn = point arrière
        backPosition = transform.position;

        // Point devant calculé depuis l'arrière
        forwardPosition = backPosition + transform.forward * distance;

        StartCoroutine(MoveForwardBackward());
    }

    IEnumerator MoveForwardBackward()
    {
        while (true)
        {
            // Aller derrière (sens inversé)
            yield return MoveTo(backPosition);

            yield return new WaitForSeconds(waitTime);

            // Aller devant
            yield return MoveTo(forwardPosition);

            yield return new WaitForSeconds(waitTime);
        }
    }

    IEnumerator MoveTo(Vector3 target)
    {
        while (Vector3.Distance(transform.position, target) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                target,
                speed * Time.deltaTime
            );
            yield return null;
        }
    }
}