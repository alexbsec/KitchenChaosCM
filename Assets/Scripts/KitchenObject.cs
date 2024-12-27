using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    /// <summary>
    /// Reference to the kitchen scriptable object
    /// </summary>
    [SerializeField] private KitchenScriptableObject _kitchenSO;
    
    /// <summary>
    /// Reference to clear counter this kitchen object belongs to
    /// </summary>
    private IKitchenObjectParent _kitchenObjectParent;
    
    /// <summary>
    /// The kitchen scriptable object getter
    /// </summary>
    public KitchenScriptableObject KitchenSO => _kitchenSO;
    
    private float _cookedPercentage = 0.0f;

    /// <summary>
    /// Sets the kitche object parent for this kitchen object
    /// </summary>
    /// <param name="clearCounter">
    /// The kitchen parent object to be set
    /// </param>
    public void SetKitchenObjectParent(IKitchenObjectParent parent)
    {
        // Clears parent clear counter (i.e., previous owner of this kitchen object)
        if (_kitchenObjectParent != null)
        {
            _kitchenObjectParent.ClearKitchenObject();
        }
        
        // Sets the new clear counter to this kitche object
        _kitchenObjectParent = parent;
        if (_kitchenObjectParent.HasKitchenObject())
        {
            Debug.LogError("Assigned counter already has an object on top of it!");
        }
        _kitchenObjectParent.SetKitchenObject(this);
        
        // Sets the parent position to the new clear counter 
        // top point and makes local position to local origin
        transform.parent = _kitchenObjectParent.GetKitchenObjectFollowTransform();
        transform.localPosition = Vector3.zero;
    }

    /// <summary>
    /// Gets current kitchen parent object owner of this
    /// kitchen object
    /// </summary>
    /// <returns>
    /// The current kitchen parent object
    /// </returns>
    public IKitchenObjectParent GetKitchenObjectParent()
    {
        return _kitchenObjectParent;
    }

    /// <summary>
    /// Destroys itself and clears the parent
    /// </summary>
    public void SelfDestroy()
    {
        _kitchenObjectParent.ClearKitchenObject();
        Destroy(gameObject);
    }

    public bool TryGetPlate(out PlateKitchenObject plateKitchenObject)
    {
        if (this is PlateKitchenObject)
        {
            plateKitchenObject = this as PlateKitchenObject;
            return true;
        }

        plateKitchenObject = null;
        return false;
    }

    public void SetCookedPercentage(float percentage)
    {
        if (percentage < 0) percentage = 0;
        if (percentage > 1) percentage = 1;
        _cookedPercentage = percentage;
    }

    public float GetCookedPercentage()
    {
        return _cookedPercentage;
    }

    /// <summary>
    /// Instantiates a kitchen object on given parent
    /// </summary>
    /// <param name="kitchenSO">
    /// The kitchen scriptable object
    /// </param>
    /// <param name="kitchenObjectParent">
    /// The parent of the to be spawned kitchen object
    /// </param>
    /// <returns>
    /// The spawned kitchen object
    /// </returns>
    public static KitchenObject SpawnKitchenObject(KitchenScriptableObject kitchenSO, IKitchenObjectParent kitchenObjectParent)
    {
        Transform kitchenObjectTransform = Instantiate(kitchenSO.prefab);
        KitchenObject kitchenObject = kitchenObjectTransform.GetComponent<KitchenObject>();
        kitchenObject.SetKitchenObjectParent(kitchenObjectParent);
        return kitchenObject;
    }
}
