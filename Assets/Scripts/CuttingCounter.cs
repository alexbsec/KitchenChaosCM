using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounter : BaseCounter, IHasProgress
{
    public event EventHandler OnCut;
    
    /// <summary>
    /// Event handler for progress bar
    /// </summary>
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    
    
    /// <summary>
    /// The cutting recipes (some objects cannot be sliced)
    /// </summary>
    [SerializeField] private CuttingRecipeScriptableObject[] _cuttingRecipeSOArray;

    private const int INITIAL_PROGRESS = 1;
    private int _cuttingProgress;
    private bool _isFirstFire = true;
    private KitchenScriptableObject _currentKitchenSO;
    
    /// <summary>
    /// Counter-player interaction
    /// </summary>
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
            _isFirstFire = true;
            return;
        }

        // Case when counter does not have object on top and
        // player is carrying it
        if (!HasKitchenObject() && player.HasKitchenObject())
        {
            // Sets the counter as the object's parent
            // And resets the cutting progress
            player.GetKitchenObject().SetKitchenObjectParent(this);
            _currentKitchenSO = GetKitchenObject().KitchenSO;
            _cuttingProgress = INITIAL_PROGRESS;
            FireEventIfPossible();
            
            // Flag next event fire as not the first one
            _isFirstFire = false;
            return;
        }
    }

    /// <summary>
    /// The alternate interaction that will cut
    /// kitchen objects
    /// </summary>
    /// <param name="player">
    /// The player object
    /// </param>
    public override void InteractAlternate(Player player)
    {
        if (!HasKitchenObject())
        {
            return;
        }

        // Gets the input and output kitchen scriptable objects
        CuttingRecipeScriptableObject cuttingRecipeSO = GetCuttingRecipeSOWithInput(_currentKitchenSO);

        // There is no recipe for this kitchen object
        if (cuttingRecipeSO == null || cuttingRecipeSO.output == null)
        {
            return;
        }
        
        KitchenScriptableObject outputKitchenSO = cuttingRecipeSO.output;
        
        // Until cutting progress is not maximum, we keep player pressing
        // alternate interaction key
        if (_cuttingProgress < cuttingRecipeSO.cuttingProgressTarget)
        {
            _cuttingProgress++;
            FireEventIfPossible();
            return;
        }
        
        // Needs to fire one more time because of 1 offset
        _cuttingProgress++;
        FireEventIfPossible();
        
        // Destroys current kitchen object and spawns its sliced version
        GetKitchenObject().SelfDestroy();
        KitchenObject.SpawnKitchenObject(outputKitchenSO, this);
        _currentKitchenSO = outputKitchenSO;
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
    private CuttingRecipeScriptableObject GetCuttingRecipeSOWithInput(KitchenScriptableObject kitchenSO)
    {
        foreach (CuttingRecipeScriptableObject cuttingRecipeSO in _cuttingRecipeSOArray)
        {
            if (cuttingRecipeSO.input == kitchenSO)
            {
                return cuttingRecipeSO;
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
    private int GetProgressBarTarget()
    {
        CuttingRecipeScriptableObject cuttingRecipeSO = GetCuttingRecipeSOWithInput(_currentKitchenSO);
        if (cuttingRecipeSO == null)
        {
            return -1;
        }

        return cuttingRecipeSO.cuttingProgressTarget;
    }

    /// <summary>
    /// Wrapper function to fire the progress bar event
    /// </summary>
    private void FireEventIfPossible() 
    {
        int progressBarTarget = GetProgressBarTarget();
        if (progressBarTarget == -1)
        {
            return;
        }
        
        // Weird code because offset
        int currentProgress = _cuttingProgress - INITIAL_PROGRESS;
            
        // Logic to reset progress bar on last slicing
        if (currentProgress == progressBarTarget)
        {
            // Since we reset _currentProgress in the Interact method
            // we just alter the local reference to the progress bar
            currentProgress = 0;
        }

        float pNormalized = (float)currentProgress / progressBarTarget;

        if (!_isFirstFire)
        {
            OnCut?.Invoke(this, EventArgs.Empty);
        }
        
        // We need this to reset the animation
        if (!_isFirstFire && currentProgress == 0)
        {
            _isFirstFire = true;
        }

        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
        {
           progressNormalized = pNormalized
        });
    }
    
}
