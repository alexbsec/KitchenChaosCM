using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter
{
    /// <summary>
    /// Kitchen Scriptable Object to instantiate different
    /// objects
    /// </summary>
    [SerializeField] private KitchenScriptableObject _kitchenSO;
    
    
    /// <summary>
    /// Counter-player interaction
    /// </summary>
    public override void Interact(Player player)
    {
        if (HasKitchenObject() && !player.HasKitchenObject())
        {
            GetKitchenObject().SetKitchenObjectParent(player);
            return;
        }

        if (!HasKitchenObject() && player.HasKitchenObject())
        {
            player.GetKitchenObject().SetKitchenObjectParent(this);
            return;
        }
    }
}
