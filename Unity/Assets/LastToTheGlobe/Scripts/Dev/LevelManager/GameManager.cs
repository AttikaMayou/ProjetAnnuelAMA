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
        public void LoadArena()
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
            }

            PhotonNetwork.LoadLevel("Lobby");

        }



        #region Public Methods

        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }

        #endregion    
    }

}