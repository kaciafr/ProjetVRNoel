using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace Kacia
{
    public class FusionManager : MonoBehaviour
    {
        public static FusionManager Instance;

        [Header("Recipe Collections")]
        public List<FusionRecipeCollection> recipeCollections = new List<FusionRecipeCollection>();

        private Dictionary<string, FusionRecipe> _recipeDictionary;

        private void BuildDictionary()
        {
            _recipeDictionary = new Dictionary<string, FusionRecipe>();

            foreach (var recipeCollection in recipeCollections)
            {
                foreach (var recipe in recipeCollection.recipes)
                {
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
            return string.Join('_', keys);
        }

        public FusionRecipe GetRecipe(IEnumerable<FusionItem> ingredients)
        {
            string key = GetFusionKey(ingredients);
            return _recipeDictionary[key];
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
            if (recipe == null) return;
            
            Debug.Log($"fusion found: {recipe}");
            Vector3 spawnPosition = GetCenterPosition(items);
            
            GameObject created = Instantiate(recipe.result, spawnPosition, Quaternion.identity);
            created.name = recipe.result.name;
            
            Debug.Log($"fusion create {created}");

            foreach (var obj in items)
                Destroy(obj);
        }
    }
}