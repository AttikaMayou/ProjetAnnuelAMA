using System;
using System.Net.Mime;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using Button = UnityEngine.Experimental.UIElements.Button;

//Auteur : Attika 

namespace LastToTheGlobe.Scripts.Dev
{
    public class SceneLobbyController : MonoBehaviourPunCallbacks
    {
        [SerializeField] private Button createRoomButton;
        [SerializeField] private Button joinRoomButton;
        [SerializeField] private Text welcomeMessageText;
        
        
    }
}
