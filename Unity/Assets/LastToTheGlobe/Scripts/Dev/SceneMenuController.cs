using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using LastToTheGlobe.Scripts.UI;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

//Auteur : Attika
// Modifications : Margot

namespace LastToTheGlobe.Scripts.Dev
{
    public class SceneMenuController : MonoBehaviourPunCallbacks
    {
        
        #region Private Variables

        [SerializeField] private ActivateObjects _mainMenu;
        [SerializeField] private Button _localPlayButton;
        [SerializeField] private Button _onlinePlayButton;
        
        [SerializeField] private ActivateObjects _playMenu;
        [SerializeField] private Button _createRoomButton;
        [SerializeField] private Button _joinRoomButton;
        [SerializeField] private Text _welcomeMessageText;
        [SerializeField] private List<string> _messages = new List<string>();

        // Player parameters
        [SerializeField] private GameObject PlayerPrefab;
        [SerializeField] private Transform SpawnPoint;
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
            _localPlayButton.onClick.AddListener(LocalPlaySetup);
            _onlinePlayButton.onClick.AddListener(OnlinePlaySetup);
            _createRoomButton.onClick.AddListener(AskForRoomCreation);
            _joinRoomButton.onClick.AddListener(AskForRoomJoin);
            PhotonNetwork.AutomaticallySyncScene = true;
        }

        private void Start()
        {
            ShowMainMenu();
        }

        #endregion
        
        #region Photon Callbacks

        public override void OnConnectedToMaster()
        {
            _createRoomButton.interactable = true;
            _joinRoomButton.interactable = true;
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
            _mainMenu.Deactivation();
            _playMenu.Deactivation();

            if(PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.LoadLevel("GameScene");
            }

            if (!PhotonNetwork.InRoom)
                return;
            //Player Instantiate
            PhotonNetwork.Instantiate(PlayerPrefab.name, SpawnPoint.transform.position, SpawnPoint.rotation, 0);

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

        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            Debug.LogError("OnJoinRoomFailed() was called by PUN. \nCreate a new room.");
            PhotonNetwork.CreateRoom(null, new RoomOptions() {MaxPlayers = 4}, null);
        }
        
        #endregion
        
        #region Public Methods

        public void ShowMainMenu()
        {
            _mainMenu.Activation();
            _playMenu.Deactivation();
            
            _welcomeMessageText.text = _messages[0];
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
            _mainMenu.Deactivation();
            _playMenu.Deactivation();

           _welcomeMessageText.text = _messages[1];
            
            OfflinePlayReady?.Invoke();
            
            PlayerJoined?.Invoke(0);
        }

        private void OnlinePlaySetup()
        {
            _mainMenu.Deactivation();
            _playMenu.Activation();
            _createRoomButton.interactable = false;
            _joinRoomButton.interactable = false;

            _welcomeMessageText.text = _messages[2];

            if (!PhotonNetwork.IsConnected)
            {
                PhotonNetwork.ConnectUsingSettings();
            }
        }
        
        #endregion
    }
}
