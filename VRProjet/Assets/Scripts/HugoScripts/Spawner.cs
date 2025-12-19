using UnityEngine;
using System.Collections; // nécessaire pour les coroutines

public class Spawner : MonoBehaviour
{
    // Référence vers le prefab du cube
    public GameObject cubePrefab;

    // Temps d'attente avant de spawn un cube, modifiable dans l'Inspector
    public float spawnDelay = 3f;
    
    public bool inverted = false;

    void Start()
    {
        // Lancer la coroutine qui spawn le cube
        StartCoroutine(SpawnCubeWithDelay());
    }

    IEnumerator SpawnCubeWithDelay()
    {
        // Attendre le temps défini
        yield return new WaitForSeconds(spawnDelay);

        // Instancier le cube
        var o = Instantiate(cubePrefab, transform.position, transform.rotation);

        if (inverted)
        {
            var m = o.GetComponent<CubeMovement>();
            if (m != null)
            {
                m.distance = -m.distance;
            }
        }
    }
}