using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class httpRequest : MonoBehaviour
{
    // Start is called before the first frame update
    public string resp;
    // Update is called once per frame
    public IEnumerator LoginRequest(string username, string password)
    {
        
        WWWForm form = new WWWForm();
        form.AddField("Username",username, Encoding.UTF8);
        form.AddField("Password",password, Encoding.UTF8);
        UnityWebRequest www = UnityWebRequest.Post("https://ebelder.pythonanywhere.com/login", form);
        yield return www.SendWebRequest();
        
        if(www.isNetworkError || www.isHttpError) {
            Debug.Log(www.error);
            yield return null;
        }
        else
        {
            resp = www.downloadHandler.text;
            if (www.downloadHandler.text.Equals("OK"))
            {
                Debug.Log("Hello User "+username);
                SceneManager.LoadScene("_masterScene");
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
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
            resp = www.downloadHandler.text;
            Debug.Log(www.downloadHandler.text);
        }
    }


    
}
