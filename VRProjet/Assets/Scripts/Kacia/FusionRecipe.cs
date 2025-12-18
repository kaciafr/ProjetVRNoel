using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Kacia
{
    [System.Serializable]
    public class FusionRecipe
    {
        public List<GameObject> ingredients;
        public GameObject result;

        public override string ToString()
        {
            return $"FusionRecipe({string.Join("+", ingredients.Select(i => i.name))}->{result.name})";;
        }
    }
}