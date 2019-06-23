using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class httpRequest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Request());
    }

    // Update is called once per frame
    IEnumerator Request()
    {
        WWWForm form = new WWWForm();
        form.AddField("Username","EBElder", Encoding.UTF8);
        form.AddField("Password","EBElder", Encoding.UTF8);
        UnityWebRequest www = UnityWebRequest.Post("https://ebelder.pythonanywhere.com/login", form);
        yield return www.SendWebRequest();
        
        if(www.isNetworkError || www.isHttpError) {
            Debug.Log(www.error);
        }
        else {
            Debug.Log(www.downloadHandler.text);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            StartCoroutine(Request());
        }
    }
}
