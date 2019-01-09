using System;
using System.Net.Mime;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Button = UnityEngine.Experimental.UIElements.Button;

//Auteur : Attika

namespace LastToTheGlobe.Scripts.Dev
{
    public class SceneMenuController : MonoBehaviourPunCallbacks
    {
        
        #region Private Variables
        [SerializeField] private Button localPlayButton;
        [SerializeField] private Button onlinePlayButton;
        [SerializeField] private Button settingsButton;
        [SerializeField] private Button creditsButton;
        [SerializeField] private Button createRoomButton;
        [SerializeField] private Button joinRoomButton;
        [SerializeField] private Text welcomeMessageText;
        #endregion
        
        #region Public Variables
        public event Action OnlinePlayReady;
        public event Action OfflinePlayReady;
        public event Action<int> PlayerJoined;
        public event Action<int> PlayerLeft;
        public event Action Disconnected;
        public event Action MasterClientSwitched;
        #endregion

        #region MonoBehaviour Callbacks

        private void Awake()
        {
            //Add listener on Buttons
            
        }

        private void Start()
        {
            ShowMainMenu();
        }

        #endregion
        
        #region Public Methods

        public void ShowMainMenu()
        {
            
        }

        #endregion
    }
}
