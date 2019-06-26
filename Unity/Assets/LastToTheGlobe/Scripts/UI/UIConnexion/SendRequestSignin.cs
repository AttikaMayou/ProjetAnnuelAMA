using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SendRequestSignin : MonoBehaviour {
    public Button yourButton;

    public GameObject PseudoGameObject;
    public GameObject MdpGameObject;
    
    private InputEventHandler _Pseudo;
    private InputEventHandler _Mdp;
    
    private httpRequest httpRequest = new httpRequest();
    
    void Start () {
        yourButton.onClick.AddListener(TaskOnClick);
        
    }

    void TaskOnClick()
    {
        _Pseudo = PseudoGameObject.GetComponent<InputEventHandler>();
        _Mdp = MdpGameObject.GetComponent<InputEventHandler>();
        if (_Pseudo.inputEntered && _Mdp.inputEntered)
        {
            httpRequest.Signin(_Pseudo.textInput, Hash128.Parse(_Mdp.textInput).ToString());
            Debug.Log("Requete envoyé!");    
        }
        else 
        {
            Debug.Log("Remplissez les champs");
        }
        
        
    }
}