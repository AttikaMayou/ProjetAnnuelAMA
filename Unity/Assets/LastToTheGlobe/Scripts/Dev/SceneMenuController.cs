using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using LastToTheGlobe.Scripts.Dev.LevelManager;
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
        [Header("First Menu Objects")]
        [FormerlySerializedAs("_mainMenu")] [SerializeField] private ActivateObjects mainMenu;
        [FormerlySerializedAs("_localPlayButton")] [SerializeField] private Button localPlayButton;
        [FormerlySerializedAs("_onlinePlayButton")] [SerializeField] private Button onlinePlayButton;
       
        [Header(("Second Menu Objects"))]
        [FormerlySerializedAs("_playMenu")] [SerializeField] private ActivateObjects playMenu;
        [FormerlySerializedAs("_createRoomButton")] [SerializeField] private Button createRoomButton;
        [FormerlySerializedAs("_joinRoomButton")] [SerializeField] private Button joinRoomButton;
        
        [Header("Feedback Objects")]
        [FormerlySerializedAs("_welcomeMessageText")] [SerializeField] private Text welcomeMessageText;
        [FormerlySerializedAs("_messages")] [SerializeField] private List<string> messages = new List<string>();

        [Header("Room Parameters")] 
        [SerializeField] private int maxPlayersPerRoom;
        #endregion

        #region Public Variables
        public event Action OnlinePlayReady;
        public event Action OfflinePlayReady;
        public event Action<int> PlayerJoined;
        public event Action<int> PlayerLeft;
        public event Action Disconnected;
        public event Action MasterClientSwitched;

        [Tooltip("Prefab for Player")]
        public GameObject playerPrefab;
        #endregion

        #region MonoBehaviour Callbacks
        private void Awake()
        {
            //Setup first menu buttons and deactivate others
            localPlayButton.onClick.AddListener(LocalPlaySetup);
            onlinePlayButton.onClick.AddListener(OnlinePlaySetup);
            createRoomButton.onClick.AddListener(AskForRoomCreation);
            joinRoomButton.onClick.AddListener(AskForRoomJoin);

            //Make sure to load a level on the master client and all clients in the same room sync automatically
            PhotonNetwork.AutomaticallySyncScene = true;
        }

        private void Start()
        {
            ShowMainMenu();
            //Make sure to load a level on the master client and all clients in the same room sync automatically
            PhotonNetwork.AutomaticallySyncScene = true;
        }

        #endregion
        
        #region Public Methods
        /// <summary>
        /// Called to show the main menu in which the player is not connected yet
        /// </summary>
        public void ShowMainMenu()
        {
            mainMenu.Activation();
            playMenu.Deactivation();
                    
            welcomeMessageText.text = messages[0];
        }
        #endregion
                
        #region Private Methods  
        /// <summary>
        /// Initialize the game to be played offline
        /// </summary>
        private void LocalPlaySetup()
        {
            mainMenu.Deactivation();
            playMenu.Deactivation();
        
            welcomeMessageText.text = messages[1];
                    
            OfflinePlayReady?.Invoke();
                    
            PlayerJoined?.Invoke(0);
        }
        
        /// <summary>
        /// Initialize the game to be played online
        /// </summary>
        private void OnlinePlaySetup()
        {
            mainMenu.Deactivation();
            playMenu.Activation();
            createRoomButton.interactable = false;
            joinRoomButton.interactable = false;
        
            welcomeMessageText.text = messages[2];
        
            if (!PhotonNetwork.IsConnected)
            {
                PhotonNetwork.ConnectUsingSettings();
            }
        }
        
        private void AskForRoomCreation()
        {
            PhotonNetwork.CreateRoom("Lobby", new RoomOptions
            {
                MaxPlayers = (byte)maxPlayersPerRoom,
                PlayerTtl = 10000
            });
        }
                
        private void AskForRoomJoin()
        {
            PhotonNetwork.JoinRoom("Lobby");
        }
        #endregion
                
        #region Photon Callbacks
        public override void OnConnectedToMaster()
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
            mainMenu.Deactivation();
            playMenu.Deactivation();

            //TODO : add here the call for the class LevelLoadingManager
            
            //Load level "Lobby"
            if(PhotonNetwork.IsMasterClient)
            {
                LevelLoadingManager.Instance.SwitchToScene(LastToTheGlobeScene.Lobby);
            }

            if (!PhotonNetwork.InRoom)
                return;

            OnlinePlayReady?.Invoke();

            if (PhotonNetwork.IsMasterClient)
            {
                //PhotonNetwork.Instantiate("PlayerControlled/PrefabTest", new Vector3(0f, 5f, 0f), Quaternion.identity, 0);
                PlayerJoined?.Invoke(0);
            }
            //TODO: add logic for index attribution per players
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
            PhotonNetwork.CreateRoom(null, new RoomOptions() {MaxPlayers = (byte)maxPlayersPerRoom}, null);
            //TODO : handle this case (with try and catch errors) : is the player cannot connect ?  
        }
        
        #endregion
        
    }
}
