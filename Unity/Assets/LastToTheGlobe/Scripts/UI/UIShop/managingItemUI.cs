using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using LastToTheGlobe.Scripts.httpRequests;
using LastToTheGlobe.Scripts.httpRequests.JSONParsedClasses;
using UnityEngine;
using UnityEngine.UI;

//Auteur : Abdallah

public class managingItemUI : MonoBehaviour
{
    [Header("Navigation Skins Buttons")]
    public Button leftButton;
    public Button rightButton;

    [Header("Buying Skins Buttons")] public Button[] buyingButtons;
    [Header("Select Items")] public Button[] selectButtons;
    
    //Text Assigné de gauche a droite
    [Header("Texts")] 
    public Text[] textsName;
    public Text[] Sold;
    public Text Select;
    public Text playerMoney;
    
    private int _baseIndex = 0;

    private string[] _itemName;
    private int[] _itemPrice;
    private string[] _itemColor;

    private int _playerCoins;
    private int[] _itemOwnedByPlayer;

    private httpRequest _httpRequest = new httpRequest();
    

    private void OnEnable()
    {
        
        //Binding des boutons de navigation 
        leftButton.onClick.AddListener(PreviousItem);
        rightButton.onClick.AddListener(NextItem);
        
        //Binding des boutons d'achats sur une seule fonction à l'aide d'une lambda
        buyingButtons[0].onClick.AddListener(()=>BuyingEventHandler(0));
        buyingButtons[1].onClick.AddListener(()=>BuyingEventHandler(1));
        buyingButtons[2].onClick.AddListener(()=>BuyingEventHandler(2));
        
        selectButtons[0].onClick.AddListener(()=>SelectItem(0));
        selectButtons[1].onClick.AddListener(()=>SelectItem(1));
        selectButtons[2].onClick.AddListener(()=>SelectItem(2));
        
        _itemName = StaticShopClass.itemName;
        _itemPrice = StaticShopClass.itemPrice;
        _itemColor = StaticShopClass.itemColor;
        
        _playerCoins = StaticLoginSigninClass.coins;
        _itemOwnedByPlayer = StaticLoginSigninClass.itemOwned;
        
    }

    private void Update()
    {
        for (int i = 0; _baseIndex + i < _baseIndex + 3; i++)
        {
            textsName[i].text = _itemName[_baseIndex + i];
            if (StaticLoginSigninClass.itemOwned[_baseIndex + i] != 0)
            {
                selectButtons[i].gameObject.SetActive(true);
                Sold[i].text = "Sold Out !";
            }
            else
            {
                Sold[i].text = "Price : " + _itemPrice[_baseIndex + i];
            }
            
        }
        
        

        playerMoney.text = _playerCoins.ToString();
    }

    public void BuyingEventHandler(int idButton)
    {
        int idItem = _baseIndex + idButton;
        if (_itemPrice[idItem] <= _playerCoins && _itemOwnedByPlayer[idItem] == 0)
        {
            _playerCoins = _playerCoins - _itemPrice[idItem];
            _itemOwnedByPlayer[idItem] = 1;
            StartCoroutine(_httpRequest.BuyFromShopRequest(idItem, StaticLoginSigninClass.playerUsername, _playerCoins));

        }
        else
        {
            Debug.Log("You cant buy this item");
        }
       
    }

    public void SelectItem(int idButton)
    {
        PlayerPreferences.colorSelected = _itemColor[_baseIndex + idButton];
        Select.text = "The color picked is : " + PlayerPreferences.colorSelected;

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
