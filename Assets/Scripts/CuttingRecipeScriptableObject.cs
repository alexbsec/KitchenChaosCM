using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class CuttingRecipeScriptableObject : ScriptableObject
{ 
    public KitchenScriptableObject input;
    public KitchenScriptableObject output;
    public int cuttingProgressTarget;
}
