using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BudgetManagerUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _budgetText;

    private float _budgetAmount = 0.0f;
    
    private const string INITIAL_BUDGED = "$ 0.00";
    private void Awake()
    {
        _budgetText.text = INITIAL_BUDGED;
    }
    private void Start()
    {
        DeliveryManager.Instance.OnRecipeCompleted += DeliveryManager_OnRecipeCompleted;
    }

    private void DeliveryManager_OnRecipeCompleted(object sender, DeliveryManager.OnRecipeCompletedEventArgs e)
    {
        if (e.correctDelivery)
        {
            AddToBudget(e.recipePrice);
        }
        else
        {
            TakeFromBudget(e.recipePrice);
        }

        if (_budgetAmount < 0.0f)
        {
            Debug.Log("Faliu...");
        }

        _budgetText.text = $"$ {_budgetAmount.ToString("0.00")}";
    }

    private void AddToBudget(float value)
    {
        _budgetAmount += value;
    }

    private void TakeFromBudget(float value)
    {
        _budgetAmount -= value;
    }
}
