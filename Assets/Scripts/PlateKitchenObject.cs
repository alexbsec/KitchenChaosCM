using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateKitchenObject : KitchenObject
{
    public event EventHandler<OnIngredientAddedEventArgs> OnIngredientAdded;

    public class OnIngredientAddedEventArgs : EventArgs
    {
        public KitchenScriptableObject kitchenSO;
    }
    [SerializeField] private List<KitchenScriptableObject> _validKitchenSOList;
    private List<KitchenScriptableObject> _kitchenSOList;

    private void Awake()
    {
        _kitchenSOList = new List<KitchenScriptableObject>();
    }
    public bool TryAddIngredient(KitchenScriptableObject kitchenSO)
    {
        if (!_validKitchenSOList.Contains(kitchenSO))
        {
            return false;
        }
        
        if (_kitchenSOList.Contains(kitchenSO))
        {
            return false;
        }
        
        _kitchenSOList.Add(kitchenSO);
        OnIngredientAdded?.Invoke(this, new OnIngredientAddedEventArgs { kitchenSO = kitchenSO });
        return true;
    }

    public List<KitchenScriptableObject> GetKitchenSOList()
    {
        return _kitchenSOList;
    }
}
