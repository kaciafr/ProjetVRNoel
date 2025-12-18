using UnityEngine;
using System.Collections;

public class CubeMovement : MonoBehaviour
{
    [Header("Déplacement")]
    public float distance = 3f;        // Distance entre droite et gauche
    public float speed = 2f;           // Vitesse de déplacement

    [Header("Pause")]
    public float waitTime = 1f;         // Temps d'attente avant de repartir

    private Vector3 rightPosition;
    private Vector3 leftPosition;

    void Start()
    {
        // Le point de spawn est le point de droite
        rightPosition = transform.position;

        // Le point de gauche est calculé depuis la droite
        leftPosition = rightPosition + Vector3.left * distance;

        StartCoroutine(MoveLeftRight());
    }

    IEnumerator MoveLeftRight()
    {
        while (true)
        {
            // Aller à gauche
            yield return MoveTo(leftPosition);

            // Pause
            yield return new WaitForSeconds(waitTime);

            // Retour à droite (spawn)
            yield return MoveTo(rightPosition);

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