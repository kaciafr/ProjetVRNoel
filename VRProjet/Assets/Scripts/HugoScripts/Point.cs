using UnityEngine;
using UnityEngine.InputSystem;

public class CubeScore : MonoBehaviour
{
    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            if (ScoreManager.Instance != null)
            {
                ScoreManager.Instance.AddPoint(1);
            }
        }
    }
}