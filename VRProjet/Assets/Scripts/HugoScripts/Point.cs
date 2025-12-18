using UnityEngine;
using UnityEngine.InputSystem;

public class CubeScore : MonoBehaviour
{
    [Header("Score")]
    public int pointsGiven = 1;   // Modifiable dans l'Inspector

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            if (ScoreManager.Instance != null)
            {
                ScoreManager.Instance.AddPoint(pointsGiven);
            }
        }
    }
}