using System.Collections;
using LastToTheGlobe.Scripts.Camera;
using LastToTheGlobe.Scripts.Dev.LevelManager;
using LastToTheGlobe.Scripts.Network;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

//Auteur : Attika
//Modification : Margot

namespace LastToTheGlobe.Scripts.Avatar
{
    public class AvatarsController : MonoBehaviour
    {
        [Header("Photon and Replication Parameters")] 
        [SerializeField] private CharacterExposerScript[] players;
        [SerializeField] private AIntentReceiver[] onlineIntentReceivers;
        [SerializeField] private AIntentReceiver[] _activatedIntentReceivers;
        [SerializeField] private PhotonView photonView;

        [Header("Camera Parameters")] 
        public CameraControllerScript myCamera;
        [SerializeField] private float rotationSpeed = 5.0f;
        
        [Header("Game Control Parameters And References")]
        [SerializeField] private StartMenuController startMenuController;
        [SerializeField] private bool gameStarted;
        [SerializeField] private bool onLobby;
        [SerializeField] private int nbMinPlayers = 2;
        [SerializeField] private float countdown = 10.0f;
        private float _countdownStartValue;
        
        #region MonoBehaviour Callbacks

        private void Awake()
        {
            gameStarted = false;
            onLobby = false;

            _countdownStartValue = countdown;
            
            startMenuController.OnlinePlayReady += ChooseAndSubscribeToIntentReceivers;
            startMenuController.PlayerJoined += ActivateAvatar;
            startMenuController.GameCanStart += LaunchGameRoom;
        }

        private void FixedUpdate()
        {
            if (!PhotonNetwork.IsMasterClient && PhotonNetwork.IsConnected)
            {
                return;
            }

            if (!gameStarted) return;

            if (_activatedIntentReceivers == null
                || players == null
                || players.Length != _activatedIntentReceivers.Length)
            {
                Debug.LogError("There is something wrong with avatars and intents setup !");
                return;
            }

            if (onLobby)
            {
                countdown -= Time.deltaTime;
                startMenuController.UpdateCountdownValue(countdown);
                if (countdown <= 0.0f)
                {
                    countdown = 0.0f;
                    onLobby = false;
                }
            }
            
            var i = 0;
            for (; i < _activatedIntentReceivers.Length; i++)
            {
                var moveIntent = Vector3.zero;

                var intent = _activatedIntentReceivers[i];
                var player = players[i];

                var rb = player.characterRb;
                var tr = player.characterTr;

                if (player == null) continue;

                if (intent.MoveBack || intent.MoveForward
                                    || intent.MoveRight || intent.MoveLeft)
                {
                    moveIntent += new Vector3(intent.strafe, 0.0f, intent.forward);
                }

                if (intent.Jump)
                {
                    var jumpDir = player.attractor.dirForce;
                    rb.AddForce(jumpDir * 250);
                }

                if (intent.Shoot)
                {
                    
                }

                if (intent.Bump)
                {
                    
                }

                if (intent.Interact)
                {
                    
                }
                
                rb.MovePosition(rb.position + tr.TransformDirection(moveIntent) * intent.speed * Time.deltaTime);
            }
        }

        #endregion

        #region Private Methods

        private void ChooseAndSubscribeToIntentReceivers()
        {
            _activatedIntentReceivers = onlineIntentReceivers;
            EnableIntentReceivers();
            gameStarted = true;
        }

        private void EnableIntentReceivers()
        {
            if (_activatedIntentReceivers == null)
            {
                Debug.Log("there is no intent receivers");
                return;
            }

            foreach (var intent in _activatedIntentReceivers)
            {
                intent.enabled = true;
                intent.MoveBack = false;
                intent.MoveForward = false;
                intent.MoveLeft = false;
                intent.MoveRight = false;
                intent.Run = false;
                intent.Jump = false;
                intent.Dash = false;
                intent.Shoot = false;
                intent.Bump = false;
                intent.Interact = false;
                intent.forward = 0.0f;
                intent.strafe = 0.0f;
            }
        }

        private void ActivateAvatar(int id)
        {
            if (PhotonNetwork.IsConnected)
            {
                photonView.RPC("ActivateAvatarRPC", RpcTarget.AllBuffered, id);
            }
            else
            {
                ActivateAvatarRPC(id);
            }
        }

        /// <summary>
        /// Each time a player join the lobby, we check if we're enough. If yes, we load the GameRoom after a countdown
        /// </summary>
        private void LaunchGameRoom()
        {
            if (!CheckIfEnoughPlayers()) return;
            onLobby = true;
            startMenuController.ShowLobbyCountdown();
            StartCoroutine(CountdownBeforeSwitchingScene(_countdownStartValue));
        }

        /// <summary>
        /// Check if there is enough players to start the game and leave Lobby
        /// </summary>
        /// <returns></returns>
        private bool CheckIfEnoughPlayers()
        {
            if (!gameStarted) return false;

            var j = 0;
            var i = 0;
            for (; i < players.Length; i++)
            {
                if (players[i].isActiveAndEnabled)
                {
                    j++;
                }
                else
                {
                    break;
                }
            }

            return j >= nbMinPlayers;
        }

        /// <summary>
        /// Wait the time indicated before switching scene to GameRoom
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        private IEnumerator CountdownBeforeSwitchingScene(float time)
        {
            yield return new WaitForSeconds(time);
            LevelLoadingManager.Instance.SwitchToScene(LastToTheGlobeScene.GameRoom);
        }

        #endregion

        #region RPC Methods

        [PunRPC]
        private void ActivateAvatarRPC(int avatarId)
        {
            players[avatarId].characterRootGameObject.SetActive(true);
            if (photonView.IsMine != players[avatarId].characterPhotonView) return;
            myCamera.playerExposer = players[avatarId];
            myCamera.InitializeCameraPosition();
            myCamera.startFollowing = true;
        }

        [PunRPC]
        private void DeactivateAvatarRPC(int avatarId)
        {
            players[avatarId].characterRootGameObject.SetActive(false);
        }

        #endregion
    }
}
