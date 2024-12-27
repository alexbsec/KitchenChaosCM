using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlateCompleteVisual : MonoBehaviour
{
    [Serializable]
    public struct KitchenScriptableObject_GameObject
    {
        public KitchenScriptableObject kitchenSO;
        public GameObject gameObject;
    }
    [SerializeField] private PlateKitchenObject _plateKitchenObject;
    [SerializeField] private List<KitchenScriptableObject_GameObject> _kitchenSOGameObjectList;

    private void Start()
    {
        _plateKitchenObject.OnIngredientAdded += PlateKitchenObject_OnIngredientAdded;

        foreach (KitchenScriptableObject_GameObject kitchenSOGameObject in _kitchenSOGameObjectList)
        {
            kitchenSOGameObject.gameObject.SetActive(false);
        }
    }

    private void PlateKitchenObject_OnIngredientAdded(object sender, PlateKitchenObject.OnIngredientAddedEventArgs e)
    {
        foreach (KitchenScriptableObject_GameObject kitchenSOGameObject in _kitchenSOGameObjectList)
        {
            if (kitchenSOGameObject.kitchenSO == e.kitchenSO)
            {
                kitchenSOGameObject.gameObject.SetActive(true);
            }
        }
    }
}
