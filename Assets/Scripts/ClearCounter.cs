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

        if (HasKitchenObject() && player.HasKitchenObject())
        {
            TryPlaceOnPlate(player);
            return;
        }
    }

    private void TryPlaceOnPlate(Player player)
    {
        bool destroyObjectOnCounter = false;
        bool destroyObjectOnPlayer = false;
        PlateKitchenObject plateKitchenObject;
        
        if (GetKitchenObject().TryGetPlate(out plateKitchenObject))
        {
            plateKitchenObject = GetKitchenObject() as PlateKitchenObject;
            destroyObjectOnPlayer = plateKitchenObject.TryAddIngredient(player.GetKitchenObject().KitchenSO);
        }

        if (player.GetKitchenObject().TryGetPlate(out plateKitchenObject))
        {
            plateKitchenObject = player.GetKitchenObject() as PlateKitchenObject;
            destroyObjectOnCounter = plateKitchenObject.TryAddIngredient(GetKitchenObject().KitchenSO);
        }

        if (destroyObjectOnPlayer)
        {
            player.GetKitchenObject().SelfDestroy();
        }

        if (destroyObjectOnCounter)
        {
            GetKitchenObject().SelfDestroy();
        }
    }
}
