using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounterVisual : MonoBehaviour
{
    [SerializeField] private PlatesCounter _platesCounter;
    [SerializeField] private Transform _counterTopPoint;
    [SerializeField] private Transform _plateVisualPrefab;
    private Stack<GameObject> _plateVisualGameObjects;

    private int _stackSize;

    private void Awake()
    {
        _plateVisualGameObjects = new Stack<GameObject>();
        _stackSize = 0;
    }
    private void Start()
    {
        _platesCounter.OnPlatesSpawned += PlatesCounter_OnPlatesSpawned;
        _platesCounter.OnPlatesRemoved += PlatesCounter_OnPlatesRemoved;
    }

    private void PlatesCounter_OnPlatesSpawned(object sender, EventArgs e)
    {
        Transform plateVisualTransform = Instantiate(_plateVisualPrefab, _counterTopPoint);

        float plateOffsetY = 0.1f;
        plateVisualTransform.localPosition = new Vector3(0, plateOffsetY * _stackSize, 0);
        _plateVisualGameObjects.Push(plateVisualTransform.gameObject);
        _stackSize++;
    }

    private void PlatesCounter_OnPlatesRemoved(object sender, EventArgs e)
    {
        GameObject plateGameObject = _plateVisualGameObjects.Pop();
        _stackSize--;
        Destroy(plateGameObject);
    }
}
