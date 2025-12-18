using System.Collections.Generic;
using UnityEngine;

namespace Kacia
{
    [CreateAssetMenu(fileName = "FusionRecipeCollection", menuName = "Fusion/Recipe Collection")]
    public class FusionRecipeCollection : ScriptableObject
    {
        public List<FusionRecipe> recipes;
    }
}