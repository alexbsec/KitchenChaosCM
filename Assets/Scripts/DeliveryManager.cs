using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManager : MonoBehaviour
{
    public static DeliveryManager Instance { get; private set; }
    [SerializeField] private RecipeListScriptableObject _recipeListSO;
    private List<RecipeScriptableObject> _waitingRecipeSOList;
    private float _spawnRecipeTimer;
    private float _spawnRecipeTimerMax = 4f;
    private int _waitingMax = 4;

    private void Awake()
    {
        Instance = this;
        _waitingRecipeSOList = new List<RecipeScriptableObject>();
    }
    private void Update()
    {
        _spawnRecipeTimer -= Time.deltaTime;
        
        if (_spawnRecipeTimer > 0f)
        {
            return;
        }
        
        _spawnRecipeTimer = _spawnRecipeTimerMax;
        
        if (_waitingRecipeSOList.Count < _waitingMax)
        {
            RecipeScriptableObject waitingRecipeSO = _recipeListSO.recipeScriptableObjectList[
                Random.Range(0, _recipeListSO.recipeScriptableObjectList.Count)];
            Debug.Log(waitingRecipeSO.recipeName);
            _waitingRecipeSOList.Add(waitingRecipeSO);
        }
    }

    public void DeliverRecipe(PlateKitchenObject plateKitchenObject)
    {
        for (int i = 0; i < _waitingRecipeSOList.Count; i++)
        {
            RecipeScriptableObject waitingRecipeSO = _waitingRecipeSOList[i];
            if (waitingRecipeSO.kitchenScriptableObjectList.Count != plateKitchenObject.GetKitchenSOList().Count)
            {
                // does not have same number of ingredients
                continue;
            }

            bool plateContentsMatchesRecipe = true;
            foreach (KitchenScriptableObject kitchenSO in waitingRecipeSO.kitchenScriptableObjectList)
            {
                bool ingredientFound = false;
                // Cycling through all ingredients in the recipe
                foreach (KitchenScriptableObject plateKitchenSO in plateKitchenObject.GetKitchenSOList())
                {
                    // Cycling through all ingredients on plate
                    if (kitchenSO == plateKitchenSO)
                    {
                        // Ingredients match
                        ingredientFound = true;
                        break;
                    }
                }

                if (!ingredientFound)
                {
                    // recipe does not match what was delivered
                    plateContentsMatchesRecipe = false;
                }
            }

            if (plateContentsMatchesRecipe)
            {
                // Deliver was successfull (correct recipe)
                Debug.Log("Correct recipe!");
                _waitingRecipeSOList.RemoveAt(i);
                return;
            }
        }
        
        // no matches
        Debug.Log("Failed to deliver recipe!");
    }

}
