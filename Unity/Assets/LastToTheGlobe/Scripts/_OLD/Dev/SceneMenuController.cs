using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using LastToTheGlobe.Scripts.Avatar;
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
        [SerializeField] private ActivateObjects mainMenu;
        [SerializeField] private Button localPlayButton;
        [SerializeField] private Button onlinePlayButton;
       
        [Header(("Second Menu Objects"))]
        [SerializeField] private ActivateObjects playMenu;
        [SerializeField] private Button createRoomButton;
        [SerializeField] private Button joinRoomButton;
        
//        [Header("Feedback Objects")]
//        [SerializeField] private Text welcomeMessageText;
//        [SerializeField] private List<string> messages = new List<string>();

        [Header("Room Parameters")] 
        [SerializeField] private int maxPlayersPerRoom;
        #endregion

        #region Public Variables
        public event Action OnlinePlayReady;
        public event Action OfflinePlayReady;
        public event Action<int> PlayerJoined;
        public event Action<int> PlayerLeft;
        public event Action GameCanStart;
        public event Action Disconnected;
        public event Action MasterClientSwitched;

//        [Tooltip("Prefab for Player")]
//        public GameObject playerPrefab;
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
                    
            //welcomeMessageText.text = messages[0];
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
        
            //welcomeMessageText.text = messages[1];
                    
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
        
            //welcomeMessageText.text = messages[2];
        
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

        private IEnumerator InvokePlayerJoinedMethod(int actorNumber)
        {
            yield return new WaitForSeconds(0.1f);
            var i = 0;
            for (; i < PlayerNumbering.SortedPlayers.Length; i++)
            {
                if (actorNumber == PlayerNumbering.SortedPlayers[i].ActorNumber)
                {
                    break;
                }
            }

            PlayerJoined?.Invoke(i);
        }

        private IEnumerator InvokeRoomJoinedMethod()
        {
            yield return new WaitForSeconds(0.1f);
            var i = 0;
            for (; i < PlayerNumbering.SortedPlayers.Length; i++)
            {
                if (PhotonNetwork.LocalPlayer.ActorNumber == PlayerNumbering.SortedPlayers[i].ActorNumber)
                {
                    break;
                }
            }
            
            Debug.Log("You are Actor : " + PhotonNetwork.LocalPlayer.ActorNumber + " \n You are controlling Avatar " + i);
            
            OnlinePlayReady?.Invoke();

            //Only the MasterClient should instantiate players
            if (PhotonNetwork.IsMasterClient)
            {
                PlayerJoined?.Invoke(i);
            }
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
            
            //Load level "Lobby"
            LevelLoadingManager.Instance.SwitchToScene(LastToTheGlobeScene.GameRoom);

            StartCoroutine(InvokeRoomJoinedMethod());

//            //Attribute index to player
//            var index = IndexAttribution.AttributeIndexToPlayers();
//            
//            OnlinePlayReady?.Invoke();
//
//            if (PhotonNetwork.IsMasterClient)
//            {
//                PlayerJoined?.Invoke(index);
//            }
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
                StartCoroutine(InvokePlayerJoinedMethod(newPlayer.ActorNumber));
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
