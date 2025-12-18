using UnityEngine;
using System.Collections;

public class CubeMovement2 : MonoBehaviour
{
    [Header("Déplacement")]
    public float distance = 3f;        // Distance entre gauche et droite
    public float speed = 2f;           // Vitesse de déplacement

    [Header("Pause")]
    public float waitTime = 1f;         // Temps d'attente avant de repartir

    private Vector3 leftPosition;
    private Vector3 rightPosition;

    void Start()
    {
        // Le point de spawn est le point de gauche
        leftPosition = transform.position;

        // Le point de droite est calculé depuis la gauche
        rightPosition = leftPosition + Vector3.right * distance;

        StartCoroutine(MoveLeftRight());
    }

    IEnumerator MoveLeftRight()
    {
        while (true)
        {
            // Aller à droite
            yield return MoveTo(rightPosition);

            // Pause
            yield return new WaitForSeconds(waitTime);

            // Retour à gauche (spawn)
            yield return MoveTo(leftPosition);

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