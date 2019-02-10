using LastToTheGlobe.Scripts.Avatar;
using Photon.Pun;
using UnityEngine;

//Auteur : Margot
//Modification : Attika

namespace LastToTheGlobe.Scripts.Dev
{
    public class AvatarsController : MonoBehaviour
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
        
        #region MonoBehaviour Callbacks

        private void Awake()
        {
            startGameController.OnlinePlayReady += ChooseAndSubscribeToOnlineIntentReceivers;
            startGameController.PlayerJoined += InstantiateAvatar;
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
        
        #endregion
        
        #region RPC Methods

        [PunRPC]
        private void InstantiateAvatarRPC(int avatarId)
        {
            _spawnPoint = new Vector3(avatarId, 0, 0);
            PhotonNetwork.Instantiate(playerPrefab.name, _spawnPoint,
                Quaternion.identity, 0);
        }
        
        #endregion
    }
}
