using System.Linq;
using System.Security.Cryptography;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//Auteur : Margot

namespace LastToTheGlobe.Scripts.Dev
{

    public class GameManager : MonoBehaviourPunCallbacks
    {
        [Tooltip("Player's prefab")]
        [SerializeField]
        private GameObject playerPrefab;

        private Scene scene;

        void Start()
        {
            if (playerPrefab == null)
            {
                Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'", this);
            }
            else
            {
                Debug.Log("We are Instantiating LocalPlayer");
                PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(0f, 5f, 0f), Quaternion.identity, 0);
            }
        }

        //Called when the local player left the room
        public override void OnLeftRoom()
        {
            SceneManager.LoadScene(0);
        }

        #region Public Methods

        //loading de level
        public void LoadingLevelGame()
        {
            PhotonNetwork.LoadLevel("GameScene");
        }

        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }

        #endregion    
    }

}