using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using DG.Tweening;

namespace Kacia
{
    public class FusionManager : MonoBehaviour
    {
        public static FusionManager Instance;

        [Header("Recipe Collections")]
        public List<FusionRecipeCollection> recipeCollections = new List<FusionRecipeCollection>();

        private Dictionary<string, FusionRecipe> _recipeDictionary = new Dictionary<string, FusionRecipe>();

        private void BuildDictionary()
        {
            _recipeDictionary = new Dictionary<string, FusionRecipe>();

            foreach (var recipeCollection in recipeCollections)
            {
                Debug.Log($"BuildDictionary recipeCollection:{recipeCollection}");
                foreach (var recipe in recipeCollection.recipes)
                {
                    Debug.Log($"BuildDictionary recipe:{recipe}");
                    List<FusionItem> ingredients = new List<FusionItem>();
                    
                    foreach (GameObject obj in recipe.ingredients)
                    {
                        FusionItem item = obj.GetComponent<FusionItem>();
                        if (item == null)
                        {
                            Debug.LogError($"{obj.name} n'a pas de composant FusionItem", obj);
                            continue;
                        }
                        ingredients.Add(item);
                    }

                    if (recipe.result.GetComponent<FusionItem>() == null)
                        Debug.LogError($"{recipe.result.name} n'a pas de composant FusionItem", recipe.result);
                    
                    string key = GetFusionKey(ingredients);
                    Debug.Log($"BuildDictionary Add key:{key} {recipe}");
                    _recipeDictionary.Add(key, recipe);
                }
            }
        }
        
        private string GetFusionKey(IEnumerable<FusionItem> ingredients)
        {
            List<string> keys = new List<string>();
            foreach (FusionItem item in ingredients)
                keys.Add(item.key);
            keys.Sort(System.StringComparer.Ordinal);
            string key = string.Join('_', keys);
            Debug.Log($"GetFusionKey keys:{keys} key:{key}");
            return key;
        }

        private FusionRecipe GetRecipe(IEnumerable<FusionItem> ingredients)
        {
            string key = GetFusionKey(ingredients);
            Debug.Log($"GetRecipe key: {key}");
            // if (_recipeDictionary.Count == 0)
                BuildDictionary();
            foreach (var keyValuePair in _recipeDictionary)
                Debug.Log($"GetRecipe test key: {keyValuePair.Key}");
            return _recipeDictionary.GetValueOrDefault(key);
        }
        
        void Awake()
        {
            Instance = this;
            BuildDictionary();
            Debug.Log("FusionManager initialisé");
        }
        
        private Vector3 GetCenterPosition(List<FusionItem> items)
        {
            List<Vector3> positions = new List<Vector3>();
            
            foreach (FusionItem item in items)
                positions.Add(item.gameObject.transform.position);
            
            Vector3 sum = Vector3.zero;
            
            foreach (var pos in positions)
                sum += pos;
            
            return sum / positions.Count;
        }

        public void TryFusion(List<FusionItem> items)
        {
            FusionRecipe recipe = GetRecipe(items);
            Debug.Log($"fusion found: {recipe}");

            if (recipe == null) return;
            
            // Eviter les clones si collision desactivé
            foreach (var item in items)
            {
                Rigidbody rb = item.GetComponent<Rigidbody>();
                if (rb != null && rb.detectCollisions == false)
                {
                    Debug.Log("Fusion déjà en cours, annulation");
                    return; // Sort si AU MOINS un item est déjà en fusion
                }
            }
            
            // Désactive les collisions immédiatement pour éviter les clones
            foreach (var item in items)
            {
                Rigidbody rb = item.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.detectCollisions = false;
                    rb.isKinematic = true;
                }
            }

            Vector3 spawnPosition = GetCenterPosition(items);

            GameObject created = Instantiate(recipe.result, spawnPosition, Quaternion.identity);
            created.name = recipe.result.name;

            Debug.Log($"fusion create {created}");

            Rigidbody createdRb = created.GetComponent<Rigidbody>();
            createdRb.isKinematic = true;
            createdRb.detectCollisions = false;

            // ✅ Scale de 0 à 1 sur l'objet créé
            created.transform.localScale = Vector3.zero;
            
            created.transform
                .DOScale(1, 0.3f)
                .SetDelay(0.2f) // ✅ Commence quand les items convergent
                .SetEase(Ease.OutBack)
                .OnComplete(() => 
                {
                    createdRb.isKinematic = false;
                    createdRb.detectCollisions = true;
                });

            // ✅ Faire disparaître les items fusionnés
            foreach (var item in items)
            {
                item.transform
                    .DOMove(spawnPosition, 0.2f)
                    .SetEase(Ease.InQuad);
                
                item.transform
                    .DOScale(0, 0.3f)
                    .SetDelay(0.15f)
                    .SetEase(Ease.InBack)
                    .OnComplete(() => 
                    {
                        if (item != null && item.gameObject != null)
                        {
                            Destroy(item.gameObject);
                        }
                    });
            }
        }
    }
}