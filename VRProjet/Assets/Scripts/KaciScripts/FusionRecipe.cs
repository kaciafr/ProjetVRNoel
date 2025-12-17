using UnityEngine;

[CreateAssetMenu(fileName = "NewRecipe", menuName = "Fusion/Recipe")]
public class FusionRecipe : ScriptableObject
{
    public string ingredient1ID;
    public string ingredient2ID;
    public GameObject resultPrefab;
    
    public bool Matches(string id1, string id2)
    {
        return (ingredient1ID == id1 && ingredient2ID == id2) ||
               (ingredient1ID == id2 && ingredient2ID == id1);
    }
}