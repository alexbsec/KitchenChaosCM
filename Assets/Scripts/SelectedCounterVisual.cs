using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour
{

    [SerializeField] private BaseCounter _counter;

    [SerializeField] private GameObject[] _visualGameObjects;
    
    private void Start()
    {
        Player.Instance.OnSelectCounterChanged += Player_OnSelectedCounterChanged;
    }

    private void Player_OnSelectedCounterChanged(object sender, Player.OnSelectedCounterChangedEventArgs e)
    {
        if (e.selectedCounter == _counter)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void Show()
    {
        for (int i = 0; i < _visualGameObjects.Length; i++)
        {
            _visualGameObjects[i].SetActive(true);
        }
    }

    private void Hide()
    {
        for (int i = 0; i < _visualGameObjects.Length; i++)
        {
            _visualGameObjects[i].SetActive(false);
        }
    }
}
