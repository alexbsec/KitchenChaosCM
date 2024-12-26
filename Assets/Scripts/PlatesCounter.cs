using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounter : BaseCounter
{
    public event EventHandler OnPlatesSpawned;
    public event EventHandler OnPlatesRemoved;
    [SerializeField] private KitchenScriptableObject _plateKitchenSO;
    private float _spawnPlateTimer;
    private int _platesSpawnedAmountMax = 4;
    private int _platesSpawnedAmount;
    
    private const float SPAWN_PLATE_TIMER_MAX = 4f;
    
    private void Update()
    {
        _spawnPlateTimer += Time.deltaTime;
        if (_spawnPlateTimer <= SPAWN_PLATE_TIMER_MAX)
        {
            return;
        }

        _spawnPlateTimer = 0f;
        if (_platesSpawnedAmount >= _platesSpawnedAmountMax)
        {
            return;
        }

        _platesSpawnedAmount++;
        OnPlatesSpawned?.Invoke(this, EventArgs.Empty);
    }

    public override void Interact(Player player)
    {
        if (player.HasKitchenObject())
        {
            return;
        }

        if (_platesSpawnedAmount <= 0)
        {
            _platesSpawnedAmount = 0;
            return;
        }
        
        _platesSpawnedAmount--;
        KitchenObject.SpawnKitchenObject(_plateKitchenSO, player);
        OnPlatesRemoved?.Invoke(this, EventArgs.Empty);
    }
}
