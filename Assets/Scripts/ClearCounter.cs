using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter
{
    /// <summary>
    /// Counter-player interaction
    /// </summary>
    public override void Interact(Player player)
    {
        // Case when counter has object on top of it and player
        // is not carrying
        if (HasKitchenObject() && !player.HasKitchenObject())
        {
            // Sets the player as the object's parent 
            GetKitchenObject().SetKitchenObjectParent(player);
            return;
        }

        // Case when counter does not have object on top and
        // player is carrying it
        if (!HasKitchenObject() && player.HasKitchenObject())
        {
            // Sets the counter as the object's parent
            player.GetKitchenObject().SetKitchenObjectParent(this);
            return;
        }
    }
}
