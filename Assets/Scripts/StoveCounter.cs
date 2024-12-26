using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class StoveCounter : BaseCounter, IHasProgress
{
    public event EventHandler<OnCookingNotBurnedEventArgs> OnCookingNotBurned;

    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    public class OnCookingNotBurnedEventArgs : EventArgs
    {
        public bool isNotBurned;
        public bool isCookable;
    }
    [SerializeField] private CookingRecipeScriptableObject[] _cookingRecipeSOArray;
    
    private KitchenScriptableObject _currentKitchenSO;
    private float _cookingProgress;
    private float _lastCookingProgress = 0.0f;
    
    private const float INITIAL_PROGRESS = 0.0f;

    private const string PATTY_BURNED = "PattyBurned";

    private bool _isBurned = false;

    public override void Interact(Player player)
    {
        // Case when counter has object on top of it and player
        // is not carrying
        if (HasKitchenObject() && !player.HasKitchenObject())
        {
            // Sets the player as the object's parent and resets
            // private variables
            GetKitchenObject().SetKitchenObjectParent(player);
            _currentKitchenSO = null;
            _lastCookingProgress = _cookingProgress;
            return;
        }

        // Case when counter does not have object on top and
        // player is carrying it
        if (!HasKitchenObject() && player.HasKitchenObject())
        {
            // Sets the counter as the object's parent
            // And resets the cooking progress
            player.GetKitchenObject().SetKitchenObjectParent(this);
            _currentKitchenSO = GetKitchenObject().KitchenSO;
            _cookingProgress = _lastCookingProgress;
            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
            {
                progressNormalized =
                    _cookingProgress / GetCookingRecipeSOWithInput(_currentKitchenSO).cookingTimerTarget
            });
            return;
        }
    }
    
    private void Update()
    {
        if (_currentKitchenSO != null && _currentKitchenSO.objectName == PATTY_BURNED)
        {
            _isBurned = true;
        }
        else
        {
            _isBurned = false;
        }
        
        OnCookingNotBurned?.Invoke(this, new OnCookingNotBurnedEventArgs
        {
            isNotBurned = !_isBurned,
            isCookable = HasKitchenObject() && _currentKitchenSO.cookable 
        });
        
        if (!HasKitchenObject() || _isBurned)
        {
            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
            {
                progressNormalized = INITIAL_PROGRESS
            });
            return;
        }
        
        _cookingProgress += Time.deltaTime;
        CookingRecipeScriptableObject cookingRecipeSO = GetCookingRecipeSOWithInput(_currentKitchenSO);

        // Not cooked yet
        if (cookingRecipeSO == null || _cookingProgress <= cookingRecipeSO.cookingTimerTarget)
        {
            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
            {
                progressNormalized = _cookingProgress / cookingRecipeSO.cookingTimerTarget
            });
            return;
        }
        
        _cookingProgress = INITIAL_PROGRESS;
        GetKitchenObject().SelfDestroy();
        _currentKitchenSO = cookingRecipeSO.output;
        KitchenObject.SpawnKitchenObject(_currentKitchenSO, this);
    }

    /// <summary>
    /// Gets the cutting recipe scriptable object of the passed kitchen
    /// scriptable object
    /// </summary>
    /// <param name="kitchenSO">
    /// The kitchen scriptable object
    /// </param>
    /// <returns>
    /// The cutting recipe if exists or null
    /// </returns>
    private CookingRecipeScriptableObject GetCookingRecipeSOWithInput(KitchenScriptableObject kitchenSO)
    {
        foreach (CookingRecipeScriptableObject cookingRecipeSO in _cookingRecipeSOArray)
        {
            if (cookingRecipeSO.input == kitchenSO)
            {
                return cookingRecipeSO;
            }
        }
        return null;
    }

    /// <summary>
    /// Gets the current target progress bar int of
    /// the current cutting recipe scriptable object
    /// </summary>
    /// <returns>
    /// The progress bar target
    /// </returns>
    private float GetProgressBarTarget()
    {
        CookingRecipeScriptableObject cookingRecipeSO = GetCookingRecipeSOWithInput(_currentKitchenSO);
        if (cookingRecipeSO == null)
        {
            return -1;
        }

        return cookingRecipeSO.cookingTimerTarget;
    }
}
