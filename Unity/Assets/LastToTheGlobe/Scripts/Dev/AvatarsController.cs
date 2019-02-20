using System.Collections;
using System.Collections.Generic;
using LastToTheGlobe.Scripts.Avatar;
using LastToTheGlobe.Scripts.Camera;
using LastToTheGlobe.Scripts.Management;
using LastToTheGlobe.Scripts.Singleton;
using Photon.Pun;
using UnityEngine;

//Auteur : Margot
//Modification : Attika

namespace LastToTheGlobe.Scripts.Dev
{
    public class AvatarsController : MonoBehaviourSingleton<AvatarsController>
    {
        [Header("Photon and Replication Parameters")]
        //[SerializeField] private CharacterExposer[] players;
        [SerializeField] private AIntentReceiver[] onlineIntentReceivers;
        //[SerializeField] private AIntentReceiver[] offlineIntentReceivers;
        [SerializeField] private SceneMenuController startGameController;
        [SerializeField] private PhotonView photonView;
        private AIntentReceiver[] _activatedIntentReceivers;
        //private Transform _spawnPoint;
        private Vector3 _spawnPoint;
        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private GameObject bulletPrefab;

        public CameraScript camInScene;
        private static GameObject _localPlayerInstance;
        
        #region MonoBehaviour Callbacks

        private void Awake()
        {
            startGameController.OnlinePlayReady += ChooseAndSubscribeToOnlineIntentReceivers;
            startGameController.PlayerJoined += InstantiateAvatar;
            
            DontDestroyOnLoad(camInScene.gameObject);
        }
        
        #endregion
        
        #region Private Methods

        private void ChooseAndSubscribeToOnlineIntentReceivers()
        {
            _activatedIntentReceivers = onlineIntentReceivers;
            EnableIntentReceivers();
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
            }
        }

        private void SynchronizePlayersDirectory(CharacterExposer exposer)
        {
            if (PhotonNetwork.IsConnected)
            {
                photonView.RPC("SyncPlayersDirectory", RpcTarget.Others, exposer);
            }
            else
            {
                SyncPlayersDirectory(exposer);
            }
        }

        private void FixedUpdate()
        {
            // If on network, only the master client can move objects
            if (PhotonNetwork.IsConnected && !PhotonNetwork.IsMasterClient)
            {
                return;
            }
            
            // If intents and avatars are not setup properly
            if (_activatedIntentReceivers == null)
            {
                Debug.LogError("There is something wrong with avatars and intents setup !");
                return;
            }

            var activatedAvatarCount = 0;

            for (var i = 0; i < _activatedIntentReceivers.Length; i++)
            {
                var moveIntent = Vector3.zero;

                var intentReceiver = _activatedIntentReceivers[i];

                var exposers = PlayerColliderDirectoryScript.Instance.characterExposers;
                activatedAvatarCount = exposers.Count;

                if (intentReceiver.Shoot)
                {
                    for (var j = 0; j < exposers.Count; j++)
                    {
                        if (i == j)
                        {
                            continue;
                        }
                        
                        LaunchBullet(exposers[j]);
                    }
                }
            }
        }
        
        private void LaunchBullet(CharacterExposer player)
        {
            if (PhotonNetwork.IsConnected)
            {
                photonView.RPC("InstantiateBullet", RpcTarget.AllBuffered, player); 
            }
            else
            {
                InstantiateBullet(player);
            }
        }
        
        private void EndGame()
        {
            startGameController.ShowMainMenu();
            if (PhotonNetwork.IsConnected)
            {
                PhotonNetwork.LeaveRoom();
                PhotonNetwork.Disconnect();
            }
        }

        private void WinGame()
        {
            
        }
        #endregion
        
        #region Public Methods
        public IEnumerator WaitBeforeSyncData(CharacterExposer exposer)
        {
            yield return new WaitForSeconds(2.0f);
            SynchronizePlayersDirectory(exposer);
        }
        #endregion
        
        #region RPC Methods
        [PunRPC]
        private void InstantiateAvatarRPC(int avatarId)
        {
            //if (!PhotonNetwork.InRoom) return;
            if (_localPlayerInstance != null)
            {
                Debug.LogError("Calling this on :" + _localPlayerInstance.name);
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
        }

        [PunRPC]
        private void InstantiateBullet(CharacterExposer player)
        {
            if (!PhotonNetwork.IsMasterClient) return;

            var bullet = PhotonNetwork.Instantiate(bulletPrefab.name,
                (player.characterTransform.position + player.characterTransform.forward * 2.0f),
                Quaternion.identity, 0);
        }
        
        [PunRPC]
        private void SyncPlayersDirectory(CharacterExposer exposer)
        {
            if (PhotonNetwork.IsMasterClient) return;
            PlayerColliderDirectoryScript.Instance.SyncData(exposer);
        }
        
        #endregion
    }
}
