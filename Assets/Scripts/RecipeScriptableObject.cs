using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class RecipeScriptableObject : ScriptableObject
{
    public List<KitchenScriptableObject> kitchenScriptableObjectList;
    public string recipeName;
    public float recipePrice;
}
