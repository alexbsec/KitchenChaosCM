using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlateIconSingleUI : MonoBehaviour
{
    [SerializeField] private Image _image;
    public void SetKitchenScriptableObject(KitchenScriptableObject kitchenSO)
    {
        _image.sprite = kitchenSO.sprite;
    }
}
