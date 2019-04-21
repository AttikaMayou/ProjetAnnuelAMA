using LastToTheGlobe.Scripts.Camera;
using LastToTheGlobe.Scripts.Network;
using Photon.Pun;
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
        
        #region MonoBehaviour Callbacks

        private void Awake()
        {
            gameStarted = false;
            
            startMenuController.OnlinePlayReady += ChooseAndSubscribeToIntentReceivers;
            startMenuController.PlayerJoined += ActivateAvatar;
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
                    intent.Jump = false;
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

//            //Not sure if that the best way to do it
//            myCamera.targetPlayer = players[id].characterRootGameObject;
//            myCamera.playerExposer = players[id];
//            myCamera.InitializeCameraPosition();
//            myCamera.startFollowing = true;
        }

        #endregion

        #region RPC Methods

        [PunRPC]
        private void ActivateAvatarRPC(int avatarId)
        {
            players[avatarId].characterRootGameObject.SetActive(true);
        }

        [PunRPC]
        private void DeactivateAvatarRPC(int avatarId)
        {
            players[avatarId].characterRootGameObject.SetActive(false);
        }

        #endregion
    }
}
