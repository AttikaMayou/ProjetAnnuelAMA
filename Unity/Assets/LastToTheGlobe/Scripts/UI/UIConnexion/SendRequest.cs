using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SendRequest : MonoBehaviour {
    public Button yourButton;

    public GameObject pseudo;
    public GameObject mdp;
    public bool boolPseudo;
    public bool boolMdp;
    
    void Start () {
        yourButton.onClick.AddListener(TaskOnClick);
        boolPseudo = pseudo.GetComponent<InputEventHandler>().mainInputField;
        boolMdp = mdp.GetComponent<InputEventHandler>().mainInputField;
    }

    void TaskOnClick()
    {
        if (boolPseudo && boolMdp)
        {
            Debug.Log("Requete envoyé!");    
        }
        else
        {
            Debug.Log("Remplissez les champs");
        }
        
        
    }
}