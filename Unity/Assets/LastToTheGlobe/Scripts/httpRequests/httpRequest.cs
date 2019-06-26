using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class httpRequest : MonoBehaviour
{
    // Start is called before the first frame update

    // Update is called once per frame
    IEnumerator LoginRequest(string username, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("Username",username, Encoding.UTF8);
        form.AddField("Password",password, Encoding.UTF8);
        UnityWebRequest www = UnityWebRequest.Post("https://ebelder.pythonanywhere.com/login", form);
        yield return www.SendWebRequest();
        
        if(www.isNetworkError || www.isHttpError) {
            Debug.Log(www.error);
        }
        else {
            Debug.Log(www.downloadHandler.text);
        }
    }
    
    IEnumerator SigninRequest(string username, string password)
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
            Debug.Log(www.downloadHandler.text);
        }
    }

    public void Login(string username, string password)
    {
        StartCoroutine(LoginRequest(username, password));
    }

    public void Signin(string username, string password)
    {
        StartCoroutine(SigninRequest(username, password));
    }

    
}
