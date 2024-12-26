using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounterVisual : MonoBehaviour
{
    [SerializeField] private StoveCounter _stoveCounter;
    [SerializeField] private GameObject _stoveOnGameObject;
    [SerializeField] private GameObject _particlesGameObject;

    private void Start()
    {
        _stoveCounter.OnCookingNotBurned += StoveCounter_OnCookingNotBurned;
    }

    private void StoveCounter_OnCookingNotBurned(object sender, StoveCounter.OnCookingNotBurnedEventArgs e)
    {
        bool showVisual = e.isNotBurned && e.isCookable;
        _stoveOnGameObject.SetActive(showVisual);
        _particlesGameObject.SetActive(showVisual);
    }
}
