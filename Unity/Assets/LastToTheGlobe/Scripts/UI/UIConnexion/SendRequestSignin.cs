using LastToTheGlobe.Scripts.httpRequests;
using UnityEngine;
using UnityEngine.UI;


//Auteur : Abdallah


/*
 
 Fonction qui va récuperer les champs de texte entré par l'utilisateur pour l'inscription puis
 faire appel à la fonction associé dans la classe httpRequest
   
*/

namespace LastToTheGlobe.Scripts.UI.UIConnexion
{
    public class SendRequestSignin : MonoBehaviour {
        public Button myButton;

        public GameObject PseudoGameObject;
        public GameObject MdpGameObject;
    
        private InputEventHandler _Pseudo;
        private InputEventHandler _Mdp;
    
        private httpRequest _httpRequest = new httpRequest();
        private string _requestResponse;
    
        void Start () {
            
            //On bind la fonction sur le click du bouton
            myButton.onClick.AddListener(TaskOnClick);
        
        }

        void TaskOnClick()
        {
            //On récupére le Pseudo et le Mot de passe entrée par l'utilisateur pour s'inscrire
            _Pseudo = PseudoGameObject.GetComponent<InputEventHandler>();
            _Mdp = MdpGameObject.GetComponent<InputEventHandler>();
            if (_Pseudo.inputEntered && _Mdp.inputEntered)
            {
                //On fait appel à la fonction d'envoi de Requete associé en Hashant le contenu du champ mot de passe
                StartCoroutine(_httpRequest.SigninRequest(_Pseudo.textInput, Hash128.Compute(_Mdp.textInput).ToString()));    
            }
            else 
            {
                Debug.Log("Remplissez les champs");
            }
        
        
        }
    }
}