using System.Linq;
using System.Security.Cryptography;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

//Auteur : Margot

namespace LastToTheGlobe.Scripts.Dev
{

    public class GameManager : MonoBehaviourPunCallbacks
    { 
  
        //Called when the local player left the room
        public override void OnLeftRoom()
        {
            SceneManager.LoadScene(0);
        }

        //Load level lobby
        public void LoadLevel()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.LoadLevel("GameScene");
            }

        }



        #region Public Methods

        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }

        #endregion    
    }

}