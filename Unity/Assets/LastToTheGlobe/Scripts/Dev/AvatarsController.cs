﻿using System.Collections;
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
            if (_activatedIntentReceivers == null) return;

            foreach (var intentReceiver in _activatedIntentReceivers)
            {
                intentReceiver.enabled = true;
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

        public void LaunchBullet(CharacterExposer player)
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