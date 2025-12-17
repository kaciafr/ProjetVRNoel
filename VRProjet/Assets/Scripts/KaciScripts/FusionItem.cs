using UnityEngine;

public class FusionItem : MonoBehaviour
{
    [Header("Item Identity")]
    public string itemID; 
    
    [Header("Fusion Settings")]
    public float fusionDistance = 0.5f;
    

    private bool isBeingFused = false;
    
    void OnCollisionEnter(Collision collision)
    {
       
        if (isBeingFused)
        {
          
            return;
        }
        
       
        FusionItem otherItem = collision.gameObject.GetComponent<FusionItem>();
        
        if (otherItem != null && !otherItem.isBeingFused)
        {
            Debug.Log($"🔵 Collision détectée: {itemID} + {otherItem.itemID}");
            
            isBeingFused = true;
            otherItem.isBeingFused = true;
            
            Vector3 fusionPoint = (transform.position + otherItem.transform.position) / 2f;
            
            if (FusionManager.Instance != null)
            {
                FusionManager.Instance.TryFusion(this, otherItem, fusionPoint);
            }
            else
            {
                Debug.LogError("❌ FusionManager.Instance est null !");
            }
        }
    }
}