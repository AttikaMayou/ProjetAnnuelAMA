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
        /*//Timer Parameters
        private float timeLeft = 20;
        private Text timerSeconds;

        void Start()
        {
            timerSeconds = GetComponent<Text>();
        }

        void Update()
        {
            timeLeft -= Time.deltaTime;
            timerSeconds.text = timeLeft.ToString("f2");

        }*/

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