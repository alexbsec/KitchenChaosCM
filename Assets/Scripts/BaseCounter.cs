using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCounter : MonoBehaviour, IKitchenObjectParent
{
    /// <summary>
    /// Reference to the empty object on top of the counter
    /// </summary>
    [SerializeField] private Transform _counterTopPoint;
    
    /// <summary>
    /// The actual object (i.e. tomato, cheese, patty...)
    /// </summary>
    protected KitchenObject _kitchenObject;

    /// <summary>
    /// Virtual interact method
    /// </summary>
    /// <param name="player">
    /// The player object
    /// </param>
    public virtual void Interact(Player player)
    {
        Debug.LogWarning("Interact not implemented");
    }

    /// <summary>
    /// Virtual alternate interact method
    /// </summary>
    /// <param name="player">
    /// The player object
    /// </param>
    public virtual void InteractAlternate(Player player)
    {
        Debug.LogWarning("InteractAlternate not implemented");
    }
    
    /// <summary>
    /// Gets the counter top transform for referencing
    /// </summary>
    /// <returns>
    /// The counter top point empty object transform
    /// </returns>
    public Transform GetKitchenObjectFollowTransform()
    {
        return _counterTopPoint;
    }

    /// <summary>
    /// Sets the kitchen object
    /// </summary>
    /// <param name="kitchenObject">
    /// The new kitchen object
    /// </param>
    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        _kitchenObject = kitchenObject;
    }

    /// <summary>
    /// Gets the current kitchen object
    /// </summary>
    /// <returns></returns>
    public KitchenObject GetKitchenObject()
    {
        return _kitchenObject;
    }

    /// <summary>
    /// Unsets the kitchen object
    /// </summary>
    public void ClearKitchenObject()
    {
        _kitchenObject = null;
    }

    /// <summary>
    /// Checks whether has kitchen object or not
    /// </summary>
    /// <returns>
    /// True if this has a kitchen object, false otherwise
    /// </returns>
    public bool HasKitchenObject()
    {
        return _kitchenObject != null;
    }
}
