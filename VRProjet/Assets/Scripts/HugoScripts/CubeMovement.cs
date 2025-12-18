using UnityEngine;
using System.Collections;

public class CubeMovement : MonoBehaviour
{
    [Header("Déplacement")]
    public float distance = 3f;        // Distance gauche / droite
    public float speed = 2f;           // Vitesse de déplacement

    [Header("Pause")]
    public float waitTime = 1f;         // Temps d'attente avant de repartir

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
        StartCoroutine(MoveLeftRight());
    }

    IEnumerator MoveLeftRight()
    {
        while (true)
        {
            // Aller à gauche
            yield return MoveTo(startPosition + Vector3.left * distance);

            // Pause
            yield return new WaitForSeconds(waitTime);

            // Aller à droite
            yield return MoveTo(startPosition + Vector3.right * distance);

            // Pause
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