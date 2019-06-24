using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SendRequest : MonoBehaviour {
    public Button yourButton;

    public GameObject pseudo;
    public GameObject mdp;
    
    private bool boolPseudo = false;
    private bool boolMdp = false;
    
    void Start () {
        yourButton.onClick.AddListener(TaskOnClick);
        
    }

    void TaskOnClick()
    {
        boolPseudo = pseudo.GetComponent<InputEventHandler>().inputEntered;
        boolMdp = mdp.GetComponent<InputEventHandler>().inputEntered;
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