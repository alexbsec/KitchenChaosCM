using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCounter : BaseCounter
{
    public event EventHandler OnPlayerGrabObject;
    
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
        if (player.HasKitchenObject())
        {
            return;
        }
        // Instantiate the kitchen object on top of the counter
        Transform kitchenObjectTransform = Instantiate(_kitchenSO.prefab);
            
        // Assings player as the owner of the object
        kitchenObjectTransform.GetComponent<KitchenObject>().SetKitchenObjectParent(player);
        
        OnPlayerGrabObject?.Invoke(this, EventArgs.Empty);
    }
}
