using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class CookingRecipeScriptableObject : ScriptableObject
{ 
    public KitchenScriptableObject input;
    public KitchenScriptableObject output;
    public float cookingTimerTarget;
}
