using UnityEngine;
using System.Collections.Generic;

public class FusionManager : MonoBehaviour
{
    public static FusionManager Instance;

    [Header("Recipes")]
    public List<FusionRecipe> recipes = new List<FusionRecipe>();

    [Header("Spawn Settings")]
    public Transform spawnPoint; 
    public bool useSpawnPoint = false;
    
    private HashSet<int> fusedObjects = new HashSet<int>();

    void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            Debug.Log("✅ FusionManager initialisé");
        }
        else
        {
            Debug.LogWarning("⚠️ Un deuxième FusionManager détecté, destruction");
            Destroy(gameObject);
        }
    }

    public void TryFusion(FusionItem item1, FusionItem item2, Vector3 fusionPoint)
    {
        int id1 = item1.GetInstanceID();
        int id2 = item2.GetInstanceID();
        
        if (fusedObjects.Contains(id1) || fusedObjects.Contains(id2))
        {
            Debug.Log("⚠️ Un des objets a déjà été fusionné, ignoré");
            return;
        }
        
        FusionRecipe matchingRecipe = FindRecipe(item1.itemID, item2.itemID);

        if (matchingRecipe != null)
        {
            Debug.Log($"✅ FUSION RÉUSSIE: {item1.itemID} + {item2.itemID} = {matchingRecipe.resultPrefab.name}");
            
            fusedObjects.Add(id1);
            fusedObjects.Add(id2);
            
            Vector3 spawnPosition = useSpawnPoint && spawnPoint != null 
                ? spawnPoint.position 
                : fusionPoint;

            GameObject fusedObject = Instantiate(matchingRecipe.resultPrefab, spawnPosition, Quaternion.identity);
            fusedObject.name = matchingRecipe.resultPrefab.name;
            
            Debug.Log($"📦 Objet créé à la position: {spawnPosition}");

            Destroy(item1.gameObject);
            Destroy(item2.gameObject);
            
            Debug.Log($"🗑️ Objets détruits: {item1.itemID} et {item2.itemID}");
        }
        else
        {
            Debug.Log($"❌ Aucune recette trouvée pour {item1.itemID} + {item2.itemID}");
        }
    }

    FusionRecipe FindRecipe(string id1, string id2)
    {
        foreach (FusionRecipe recipe in recipes)
        {
            if (recipe.Matches(id1, id2))
            {
                Debug.Log($"📋 Recette trouvée: {recipe.ingredient1ID} + {recipe.ingredient2ID}");
                return recipe;
            }
        }
        return null;
    }
}
