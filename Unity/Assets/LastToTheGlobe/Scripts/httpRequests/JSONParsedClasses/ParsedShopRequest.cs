using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Auteur : Abdallah


/*
  Classe sur laquelle est parsé la requete de la boutique 
*/


[Serializable]
public class ParsedShopRequest
{
    
    public string status;
    public string[] itemName;
    public int[] itemPrice;
    public string[] itemColor;

}
