using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.UI;

//Auteur : Abdallah

public class managingItemUI : MonoBehaviour
{
    [Header("Buttons")]
    public Button leftButton;
    public Button rightButton;
    
    
    [Header("Texts")]
    public Text leftText;
    public Text centerText;
    public Text rightText;

    private int _baseIndex = 0;

    private string[] _itemName;
    private int[] _itemPrice;
    private string[] _itemColor;
    
    private void OnEnable()
    {
        
        leftButton.onClick.AddListener(PreviousItem);
        rightButton.onClick.AddListener(NextItem);
        _itemName = StaticShopClass.itemName;
        _itemPrice = StaticShopClass.itemPrice;
        _itemColor = StaticShopClass.itemColor;   
        
    }

    private void Update()
    {
        leftText.text = StaticShopClass.itemName[_baseIndex];
        centerText.text = StaticShopClass.itemName[_baseIndex+1];
        rightText.text = StaticShopClass.itemName[_baseIndex+2];
    }

    void PreviousItem()
    {
        if (_baseIndex - 3 >= 0)
        {
            _baseIndex -= 3;
        }
    }

    void NextItem()
    {
        if (_baseIndex + 3 < _itemName.Length)
        {
            _baseIndex += 3;
        }
    }
}
