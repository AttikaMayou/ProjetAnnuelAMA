using System.Collections;
using System.Collections.Generic;
using LastToTheGlobe.Scripts.httpRequests;
using LastToTheGlobe.Scripts.UI.UIConnexion;
using UnityEngine;
using UnityEngine.UI;

//Auteur : Abdallah


/*
 
 Fonction qui va permettre de charger les items en vente en faisant un call à la 
 requete associé dans la classe httpRequest et accéder à la boutique
   
*/
public class getShopItems : MonoBehaviour
{
    // Start is called before the first frame update
    public Button myButton;
    
    public List<GameObject> toEnable = new List<GameObject>();
    public List<GameObject> toDisable = new List<GameObject>();

    public ButtonScript buttonScript;
    private bool shopOpen;
    private httpRequest _httpRequest = new httpRequest();
    private string _requestResponse;
    
    void Start () {
        //myButton.onClick.AddListener(TaskOnClick);
        
    }

    void Awake()
    {
        print("Trying to load shop");
        StartCoroutine(_httpRequest.ShopRequest());
        

    }

    
}
