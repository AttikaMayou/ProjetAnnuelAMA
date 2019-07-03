using System.Collections;
using System.Collections.Generic;
using LastToTheGlobe.Scripts._OLD.Dev;
using LastToTheGlobe.Scripts.Avatar;
using LastToTheGlobe.Scripts.Camera;
using LastToTheGlobe.Scripts.Management;
using LastToTheGlobe.Scripts.Singleton;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Serialization;

//Auteur : Margot
//Modification : Attika

namespace LastToTheGlobe.Scripts.Dev
{
    public class AvatarsController : MonoBehaviourSingleton<AvatarsController>
    {
        /*[Header("Photon and Replication Parameters")]
        [SerializeField] private CharacterExposer[] players;
        
        [SerializeField] private AIntentReceiver[] onlineIntentReceivers;
        
        //[SerializeField] private AIntentReceiver[] offlineIntentReceivers;
        
        [SerializeField] private SceneMenuController startGameController;
        
        [SerializeField] private PhotonView photonView;
        
        [SerializeField] private AIntentReceiver[] _activatedIntentReceivers;
        
        [FormerlySerializedAs("_gameStarted")] [SerializeField] private bool gameStarted;
        
        private Vector3 _spawnPoint;
        
        [SerializeField] private GameObject playerPrefab;
        
        [SerializeField] private GameObject bulletPrefab;

        [SerializeField] private float speed;
        

        public CameraScript camInScene;
        
        private static GameObject _localPlayerInstance;
        
        #region MonoBehaviour Callbacks
        private void Awake()
        {
            gameStarted = false;
            startGameController.OnlinePlayReady += ChooseAndSubscribeToOnlineIntentReceivers;
            startGameController.PlayerJoined += InstantiateAvatar;
            
            DontDestroyOnLoad(camInScene.gameObject);
        }
      
        private void FixedUpdate()
        {
            // If on network, only the master client can move objects
            if (PhotonNetwork.IsConnected && !PhotonNetwork.IsMasterClient)
            {
                return;
            }

            //Do nothing if the game is not started 
            if (!gameStarted) return;
            
            // If intents and avatars are not setup properly
            if (_activatedIntentReceivers == null 
                || players == null
                || players.Length != _activatedIntentReceivers.Length)
            {
                Debug.LogError("There is something wrong with avatars and intents setup !");
                //SyncPlayersArray();
                return;
            }

            for (var i = 0; i < _activatedIntentReceivers.Length; i++)
            {
                var moveIntent = Vector3.zero;

                var intentReceiver = _activatedIntentReceivers[i];
                var player = players[i];

                if (player == null)
                {
                    continue;
                }
//                if (intentReceiver.Shoot)
//                {
//                    if (PhotonNetwork.IsConnected)
//                    {
//                        photonView.RPC("InstantiateBullet", RpcTarget.AllBuffered, player); 
//                    }
//
//                    intentReceiver.Shoot = false;
//                }

                if (intentReceiver.MoveBack || intentReceiver.MoveForward
                 || intentReceiver.MoveLeft || intentReceiver.MoveRight)
                {
                    Debug.Log(intentReceiver.strafe + "----------------");
                    moveIntent += new Vector3(intentReceiver.strafe, 0.0f, intentReceiver.forward);
                }

                var rb = player.characterRb;
                var tr = player.characterTransform;
                player.characterRb.MovePosition(rb.position + tr.TransformDirection(moveIntent) * speed * Time.deltaTime);
            }
        }
        #endregion
        
        #region Private Methods
//        private void SyncPlayersArray()
//        {
//            for (var j = 0; j < PlayerColliderDirectoryScript.Instance.characterExposers.Count; j++)
//            {
//                players[j] = PlayerColliderDirectoryScript.Instance.characterExposers[j];
//            }
//        }
        
        private void ChooseAndSubscribeToOnlineIntentReceivers()
        {
            _activatedIntentReceivers = onlineIntentReceivers;
            EnableIntentReceivers();
            gameStarted = true;
        } 

        private void InstantiateAvatar(int id)
        {
            if (PhotonNetwork.IsConnected)
            {
                photonView.RPC("InstantiateAvatarRPC", RpcTarget.AllBuffered, id);
            }
            else
            {
                InstantiateAvatarRPC(id);
            }
        }

        private void EnableIntentReceivers()
        {
            if (_activatedIntentReceivers == null)
            {
                Debug.Log("there is no intent receivers");
                return;
            }

            foreach (var intentReceiver in _activatedIntentReceivers)
            {
                intentReceiver.enabled = true;
                intentReceiver.Shoot = false;
                intentReceiver.Dash = false;
                intentReceiver.MoveBack = false;
                intentReceiver.MoveLeft = false;
                intentReceiver.MoveRight = false;
                intentReceiver.MoveForward = false;
                intentReceiver.Run = false;
                intentReceiver.Jump = false;
                intentReceiver.Bump = false;
                intentReceiver.Interact = false;
            }
        }
        
        private void EndGame()
        {
            startGameController.ShowMainMenu();
            
//            if (!PhotonNetwork.IsConnected) return;
            PhotonNetwork.LeaveRoom();
            PhotonNetwork.Disconnect();
        }

        private void WinGame()
        {
            
        }
        #endregion
        
        #region RPC Methods
        [PunRPC]
        private void InstantiateAvatarRPC(int avatarId)
        {
            //if (!PhotonNetwork.InRoom) return;
            if (_localPlayerInstance != null)
            {
                Debug.LogError("Calling instantiation on :" + _localPlayerInstance.name + " which avatar id is : " + avatarId);
                //SyncPlayersArray();
                return;
            }
            
            _spawnPoint = new Vector3(avatarId, 0, 0);
            var newPlayer = PhotonNetwork.Instantiate(playerPrefab.name, _spawnPoint,
                Quaternion.identity, 0);
            Debug.Log("Nom du joueur avant le rename : " +newPlayer.name);
            newPlayer.gameObject.name = "Player " + avatarId.ToString();
            newPlayer.gameObject.SetActive(true);
            //Reference the localPlayerInstance with this new gameObject
            _localPlayerInstance = newPlayer;
            camInScene.targetPlayer = newPlayer;
            
            //SyncPlayersArray();
        }

        [PunRPC]
        private void InstantiateBullet(CharacterExposer player)
        {
            if (!PhotonNetwork.IsMasterClient) return;

            var bullet = PhotonNetwork.Instantiate(bulletPrefab.name,
                (player.characterTransform.position + player.characterTransform.forward * 2.0f),
                Quaternion.identity, 0);
        }
        
        #endregion*/
    }
}
