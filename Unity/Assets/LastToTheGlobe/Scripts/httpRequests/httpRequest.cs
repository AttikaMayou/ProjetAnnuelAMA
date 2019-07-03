using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

namespace LastToTheGlobe.Scripts.httpRequests
{
    public class httpRequest : MonoBehaviour
    {
        // Start is called before the first frame update
        private ParsedRequest _parsedRequest;
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
                _parsedRequest = JsonUtility.FromJson<ParsedRequest>(www.downloadHandler.text);
                if (_parsedRequest.status.Equals("OK"))
                {
                    Debug.Log("Hello User "+username);
                    SceneManager.LoadScene("_masterScene");
                }
                else
                {
                    Debug.Log(_parsedRequest.status);
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
                _parsedRequest = JsonUtility.FromJson<ParsedRequest>(www.downloadHandler.text);
                Debug.Log(_parsedRequest.status);
            }
        }


    
    }
}
