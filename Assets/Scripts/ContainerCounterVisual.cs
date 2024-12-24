using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCoutnerVisual : MonoBehaviour
{
    [SerializeField] private ContainerCounter _containerCounter;
    private Animator _animator;

    private const string TRIGGER = "OpenClose";

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        _containerCounter.OnPlayerGrabObject += ContainerCounter_OnPlayerGrabObject;
    }

    private void ContainerCounter_OnPlayerGrabObject(object sender, System.EventArgs e)
    {
        _animator.SetTrigger(TRIGGER);
    }
}
