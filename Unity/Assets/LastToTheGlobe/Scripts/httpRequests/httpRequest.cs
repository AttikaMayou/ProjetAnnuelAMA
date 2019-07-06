﻿using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

namespace LastToTheGlobe.Scripts.httpRequests
{
    public class httpRequest : MonoBehaviour
    {
        // Start is called before the first frame update
        private ParsedLoginSignRequest _parsedLoginSignRequest;
        private ParsedShopRequest _parsedShopRequest;
        public bool requestFinished = false;
        
        // Update is called once per frame
        public IEnumerator LoginRequest(string username, string password)
        {
        
            //On crée un JSON qui va être envoyé dans la requete
            WWWForm form = new WWWForm();
            form.AddField("Username",username, Encoding.UTF8);
            form.AddField("Password",password, Encoding.UTF8);
            
            //On envoie notre requete avec le JSON appelé form
            UnityWebRequest www = UnityWebRequest.Post("https://ebelder.pythonanywhere.com/login", form);
            yield return www.SendWebRequest();
        
            if(www.isNetworkError || www.isHttpError) {
                Debug.Log(www.error);
                yield return null;
            }
            else
            {
                //On parse le contenu de la réponse dans une classe afin de pouvoir y accéder
                _parsedLoginSignRequest = JsonUtility.FromJson<ParsedLoginSignRequest>(www.downloadHandler.text);
                if (_parsedLoginSignRequest.status.Equals("OK"))
                {
                    
                    Debug.Log("Hello User "+username);
                    SceneManager.LoadScene("_masterScene");
                }
                else
                {
                    Debug.Log(_parsedLoginSignRequest.status);
                }
            }
        }
        
    
        public IEnumerator SigninRequest(string username, string password)
        {
            WWWForm form = new WWWForm();
            form.AddField("Username", username, Encoding.UTF8);
            form.AddField("Password",password, Encoding.UTF8);
            UnityWebRequest www = UnityWebRequest.Post("https://ebelder.pythonanywhere.com/signin", form);
            yield return www.SendWebRequest();
        
            if(www.isNetworkError || www.isHttpError) {
                Debug.Log(www.error);
            }
            else {
                _parsedLoginSignRequest = JsonUtility.FromJson<ParsedLoginSignRequest>(www.downloadHandler.text);
                Debug.Log(_parsedLoginSignRequest.status);
            }
        }

        
        public IEnumerator ShopRequest()
        {
            requestFinished = false;
            //Ici la Requete est de type GET donc il n'y à pas de form à envoyé
            
            UnityWebRequest www = UnityWebRequest.Get("https://ebelder.pythonanywhere.com/shop");
            yield return www.SendWebRequest();
            if(www.isNetworkError || www.isHttpError) {
                Debug.Log(www.error);
                requestFinished = true;
                yield return null;
            }
            else
            {
                
                _parsedShopRequest = JsonUtility.FromJson<ParsedShopRequest>(www.downloadHandler.text);
                if (_parsedShopRequest.status.Equals("OK"))
                {
                    StaticShopClass.itemName = _parsedShopRequest.itemName;
                    StaticShopClass.itemPrice = _parsedShopRequest.itemPrice;
                    StaticShopClass.itemColor = _parsedShopRequest.itemColor;

                    Debug.Log("Shop loaded");
                }
                else
                {
                    Debug.Log(_parsedShopRequest.status);
                }

                requestFinished = true;
            }
        }
    
    }
}
