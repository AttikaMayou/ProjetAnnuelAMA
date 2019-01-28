using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

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
        [SerializeField] private List<string> messages = new List<string>();
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
        
        #region Photon Callbacks

        private void OnConnectedToServer()
        {
            createRoomButton.interactable = true;
            joinRoomButton.interactable = true;
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            Disconnected?.Invoke();
        }

        public override void OnMasterClientSwitched(Player player)
        {
            MasterClientSwitched?.Invoke();
        }

        public override void OnJoinedRoom()
        {
            localPlayButton.gameObject.SetActive(false);
            onlinePlayButton.gameObject.SetActive(false);
            createRoomButton.gameObject.SetActive(false);
            joinRoomButton.gameObject.SetActive(false);
            
            //TODO: add coroutine for welcome message
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                return;
            }

            var i = 0;
            for (; i < PlayerNumbering.SortedPlayers.Length; i++)
            {
                if (otherPlayer.ActorNumber == PlayerNumbering.SortedPlayers[i].ActorNumber)
                {
                    break;
                }
            }
            
            PlayerLeft?.Invoke(i);
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                //TODO: add coroutine for welcome message
            }
        }

        #endregion
        
        #region Public Methods

        public void ShowMainMenu()
        {
            localPlayButton.gameObject.SetActive(true);
            onlinePlayButton.gameObject.SetActive(true);
            localPlayButton.interactable = true;
            onlinePlayButton.interactable = true;
            
            createRoomButton.gameObject.SetActive(false);
            joinRoomButton.gameObject.SetActive(false);
            createRoomButton.interactable = false;
            joinRoomButton.interactable = false;

            welcomeMessageText.text = messages[0];
        }

        public void AskForRoomCreation()
        {
            PhotonNetwork.CreateRoom("Tmp", new RoomOptions
            {
                MaxPlayers = 4,
                PlayerTtl = 10000
            });
        }

        public void AskForRoomJoin()
        {
            PhotonNetwork.JoinRoom("Tmp");
        }

        #endregion
        
        #region Private Methods

        private void LocalPlaySetup()
        {
            localPlayButton.gameObject.SetActive(false);
            onlinePlayButton.gameObject.SetActive(false);
            createRoomButton.gameObject.SetActive(false);
            joinRoomButton.gameObject.SetActive(false);

            welcomeMessageText.text = messages[1];
            
            OfflinePlayReady?.Invoke();
            
            PlayerJoined?.Invoke(0);
        }

        private void OnlinePlaySetup()
        {
            localPlayButton.gameObject.SetActive(false);
            onlinePlayButton.gameObject.SetActive(false);
            createRoomButton.gameObject.SetActive(true);
            joinRoomButton.gameObject.SetActive(true);
            createRoomButton.interactable = false;
            joinRoomButton.interactable = false;

            welcomeMessageText.text = messages[2];

            PhotonNetwork.ConnectUsingSettings();
        }
        
        #endregion
    }
}
