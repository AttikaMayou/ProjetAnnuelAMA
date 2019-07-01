using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SendRequestSignin : MonoBehaviour {
    public Button myButton;

    public GameObject PseudoGameObject;
    public GameObject MdpGameObject;
    
    private InputEventHandler _Pseudo;
    private InputEventHandler _Mdp;
    
    private httpRequest _httpRequest = new httpRequest();
    private string _requestResponse;
    
    

    void TaskOnClick()
    {
        _Pseudo = PseudoGameObject.GetComponent<InputEventHandler>();
        _Mdp = MdpGameObject.GetComponent<InputEventHandler>();
        if (_Pseudo.inputEntered && _Mdp.inputEntered)
        {
            StartCoroutine(_httpRequest.SigninRequest(_Pseudo.textInput, Hash128.Compute(_Mdp.textInput).ToString()));    
        }
        else 
        {
            Debug.Log("Remplissez les champs");
        }
        
        
    }
}