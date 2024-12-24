using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounterVisual : MonoBehaviour
{
    [SerializeField] private CuttingCounter _cuttingCounter;
    private Animator _animator;

    private const string TRIGGER = "Cut";

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        _cuttingCounter.OnCut += ContainerCounter_OnPlayerCut;
    }

    private void ContainerCounter_OnPlayerCut(object sender, System.EventArgs e)
    {
        _animator.SetTrigger(TRIGGER);
    }
}
