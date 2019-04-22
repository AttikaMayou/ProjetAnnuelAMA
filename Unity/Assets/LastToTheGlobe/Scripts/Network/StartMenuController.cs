using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using LastToTheGlobe.Scripts.UI;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//Auteur : Attika
//Modification : Margot

namespace LastToTheGlobe.Scripts.Network
{
    public class StartMenuController : MonoBehaviourPunCallbacks
    {
        #region Private Variables

        [Header("MainMenu Objects")] 
        [SerializeField] private ActivateObjects mainMenu;
        [SerializeField] private Button onlinePlayButton;

        [Header("Lobby Menu Objects")] 
        [SerializeField] private ActivateObjects playMenu;
        [SerializeField] private Button createRoomButton;
        [SerializeField] private Button joinRoomButton;

        [Header("LobbyMenu objects")]
        [SerializeField] private ActivateObjects lobbyMenu;
        [SerializeField] private TextMeshProUGUI countdown;
        
        [Header("Room Parameters")] 
        [SerializeField] private int maxPlayersPerRoom;
        
        #endregion
        
        #region Public Variabes

        public event Action OnlinePlayReady;
        public event Action<int> PlayerJoined;
        public event Action<int> PlayerLeft;
        public event Action GameCanStart;
        public event Action Disconnected;
        public event Action MasterClientSwitched;

        #endregion
        
        #region Monobehaviour Callbacks

        private void Awake()
        {
            onlinePlayButton.onClick.AddListener(OnlinePlaySetup);
            createRoomButton.onClick.AddListener(AskForRoomCreation);
            joinRoomButton.onClick.AddListener(AskForRoomJoin);

            PhotonNetwork.AutomaticallySyncScene = true;
        }

        private void Start()
        {
            ShowMainMenu();
            
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
            lobbyMenu.Deactivation();
        }
        
        /// <summary>
        /// Called to show the lobby countdown when the game is ready to begin !
        /// </summary>
        public void ShowLobbyCountdown()
        {
            lobbyMenu.Activation();
            playMenu.Deactivation();
            mainMenu.Deactivation();
        }

        /// <summary>
        /// Update the countdown value
        /// </summary>
        /// <param name="time"></param>
        public void UpdateCountdownValue(float time)
        {
            countdown.text = time.ToString(CultureInfo.InvariantCulture);
        }
        
        #endregion
        
        #region Private Methods

        private void OnlinePlaySetup()
        {
            mainMenu.Deactivation();
            playMenu.Activation();
            lobbyMenu.Deactivation();
            createRoomButton.interactable = false;
            joinRoomButton.interactable = false;
            
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
            yield return new WaitForSeconds(1.0f);

            var i = 0;
            for (; i < PlayerNumbering.SortedPlayers.Length; i++)
            {
                if (actorNumber == PlayerNumbering.SortedPlayers[i].ActorNumber)
                {
                    break;
                }
            }
            
            //PlayerJoined?.Invoke(i);
        }

        private IEnumerator InvokeRoomJoinedMethod()
        {
            yield return new WaitForSeconds(1.0f);
            var i = 0;
            for (; i < PlayerNumbering.SortedPlayers.Length; i++)
            {
                if (PhotonNetwork.LocalPlayer.ActorNumber == PlayerNumbering.SortedPlayers[i].ActorNumber)
                {
                    break;
                }
            }
            
            OnlinePlayReady?.Invoke();
            
            //Only the MasterClient should activate players
            if (PhotonNetwork.IsMasterClient)
            {
                PlayerJoined?.Invoke(i);
                GameCanStart?.Invoke();
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
            
            //TODO : Load physical level Lobby 

            StartCoroutine(InvokeRoomJoinedMethod());
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            if (!PhotonNetwork.IsMasterClient) return;
            
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
            PhotonNetwork.CreateRoom(null, new RoomOptions()
            {
                MaxPlayers = (byte) maxPlayersPerRoom
            }, null); 
        }
        
        #endregion
    }
}
