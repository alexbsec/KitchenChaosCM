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

        KitchenObject.SpawnKitchenObject(_kitchenSO, player);
        OnPlayerGrabObject?.Invoke(this, EventArgs.Empty);
    }
}
