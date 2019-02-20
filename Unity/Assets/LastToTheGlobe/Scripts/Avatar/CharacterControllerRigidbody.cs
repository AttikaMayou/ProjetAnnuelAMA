using LastToTheGlobe.Scripts.Dev;
using LastToTheGlobe.Scripts.Camera;
using LastToTheGlobe.Scripts.Environment.Planets;
using Photon.Pun;
using UnityEngine;

//Auteur : Margot
//Modification : Attika

namespace LastToTheGlobe.Scripts.Avatar
{
    public class CharacterControllerRigidbody : MonoBehaviour
    {
        [SerializeField] private AttractedScript attractedScript;

        [Header("Character Exposer")]
        public CharacterExposer playerExposer;

        [Header("Photon and Replication Parameters")]
        private CharacterExposer[] players;
        [SerializeField]
        private AIntentReceiver[] onlineIntentReceivers;
        [SerializeField]
        private SceneMenuController startGameControllerScript;
        [SerializeField]
        private PhotonView photonView;
        private AIntentReceiver[] _activatedIntentReceivers;
        private bool GameStarted { get; set; }
        private Transform _spawnPoint;

        [Header("Camera Parameters")]
        public CameraScript myCamera;
        [SerializeField]
        private float rotationSpeed = 5.0f;

        //Parameters for players and movements control
        [Header("Parameters for players and movements control")]
        [SerializeField]
        [Tooltip("Character")]
        private Rigidbody rb;

        [SerializeField]
        [Tooltip("Vitesse du joueur")]
        private float _speed;

        [SerializeField]
        [Tooltip("Vitesse de la course")]
        private float runSpeed;
       
        [SerializeField]
        [Tooltip("Vitesse du saut")]
        private float jumpSpeed;

        [SerializeField]
        [Tooltip("Vitesse du dash")]
        private float dashSpeed;

        [SerializeField]
        [Tooltip("Raycast position")]
        private Transform raycastOrigin;

        [Header("Orb Objects")]
        public GameObject orb;
        public GameObject orbSpawned;

        private float _ray = 0.4f;
        private bool _isGrounded = false;
        private int _jumpMax = 1;
        private Quaternion _rotation;

        void Awake()
        {
            startGameControllerScript.OnlinePlayReady += ChooseAndSubscribeToOnlineIntentReceivers;
            startGameControllerScript.PlayerJoined += ActivateAvatar;
            startGameControllerScript.PlayerLeft += DeactivateAvatar;
            startGameControllerScript.Disconnected += EndGame;
            startGameControllerScript.MasterClientSwitched += EndGame;
        }

        void Start()
        {
        }

        private void FixedUpdate()
        {
            //Rotate the character so the camera can follow
            transform.Rotate(new Vector3(0,
                Input.GetAxis("Mouse X") * rotationSpeed,
                0));


            //Rotate cameraRotatorX to give the camera the right vertical axe
            playerExposer.cameraRotatorX.transform.Rotate(new Vector3(-(Input.GetAxis("Mouse Y") * rotationSpeed),
                0,
                0), Space.Self);


            //Prevent the camera from going too high or too low
            //Les valeurs qui était mise sont des valeurs qui peuvent être pris par la variable cameraRotatorX.transform.rotation.x (-1 - 1)
            if (playerExposer.cameraRotatorX.transform.rotation.x >= 0.42f)
            {
                _rotation =
                    new Quaternion(0.42f, _rotation.y,
                    _rotation.z, _rotation.w);
                playerExposer.cameraRotatorX.transform.rotation = _rotation;
            }

            if (playerExposer.cameraRotatorX.transform.rotation.x <= -0.2f)
            {
                _rotation =
                    new Quaternion(-0.2f, _rotation.y,
                        _rotation.z, _rotation.w);
                playerExposer.cameraRotatorX.transform.rotation = _rotation;
            }

            // If on network, only the master client can move objects
            if (PhotonNetwork.IsConnected && !PhotonNetwork.IsMasterClient)
            {
                return;
            }

            // Do nothing if the game is not started
            if (!GameStarted)
            {
                return;
            }

            // If intents and avatars are not setup properly
            if (_activatedIntentReceivers == null
                || players == null
                || players.Length != _activatedIntentReceivers.Length)
            {
                Debug.LogError("There is something wrong with avatars and intents setup !");
                return;
            }

            //Intents for movement
            var fallenAvatarsCount = 0;
            var activatedAvatarsCount = 0;

            for (var i = 0; i < _activatedIntentReceivers.Length; i++)
            {
                Vector3 movement = Vector3.zero;

                var intentReceiver = _activatedIntentReceivers[i];
                var avatar = players[i];

                activatedAvatarsCount += avatar.avatarRootGameObject.activeSelf ? 1 : 0;

                if (intentReceiver.MoveBack)
                {
                    movement += Vector3.back;
                }

                if (intentReceiver.MoveForward)
                {
                    movement += Vector3.forward;
                }

                if (intentReceiver.MoveLeft)
                {
                    movement += Vector3.left;
                }

                if (intentReceiver.MoveRight)
                {
                    movement += Vector3.right;
                }

                if(intentReceiver.Dash)
                {
                    _speed = dashSpeed;
                }

                if(intentReceiver.Run)
                {
                    _speed = runSpeed;
                }

                if (intentReceiver.Jump)
                {
                    if (_jumpMax > 0)
                    {
                        rb.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
                        _jumpMax--;
                        intentReceiver.Jump = false;
                    }
                    if (IsGrounded())
                    {
                        _jumpMax = 1;
                    }
                }

                movement = movement.normalized;
                rb.AddForce(movement * _speed);

                fallenAvatarsCount++;

                if (activatedAvatarsCount - fallenAvatarsCount <= 1 && activatedAvatarsCount > 1)
                {
                    EndGame();
                }
            }
        }

        //Functions for movements replication
        private void ChooseAndSubscribeToOnlineIntentReceivers()
        {
            _activatedIntentReceivers = onlineIntentReceivers;
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

        private void DisableIntentReceivers()
        {
            if (_activatedIntentReceivers == null)
            {
                return;
            }

            for (var i = 0; i < _activatedIntentReceivers.Length; i++)
            {
                _activatedIntentReceivers[i].enabled = false;
            }
        }

        private void EnableIntentReceivers()
        {
            if (_activatedIntentReceivers == null)
            {
                return;
            }

            for (var i = 0; i < _activatedIntentReceivers.Length; i++)
            {
                _activatedIntentReceivers[i].enabled = true;
                _activatedIntentReceivers[i].Dash = false;
                _activatedIntentReceivers[i].Jump = false;
                _activatedIntentReceivers[i].Run = false;
                _activatedIntentReceivers[i].MoveLeft = false;
                _activatedIntentReceivers[i].MoveBack = false;
                _activatedIntentReceivers[i].MoveRight = false;
                _activatedIntentReceivers[i].MoveForward = false;
            }
        }

        private void DeactivateAvatar(int id)
        {
            if (PhotonNetwork.IsConnected)
            {
                photonView.RPC("DeactivateAvatarRPC", RpcTarget.AllBuffered, id);
            }
            else
            {
                DeactivateAvatarRPC(id);
            }
        }

        private void EndGame()
        {
            GameStarted = false;
            _activatedIntentReceivers = null;

            for (var i = 0; i < players.Length; i++)
            {
                players[i].avatarRootGameObject.SetActive(false);
            }

            startGameControllerScript.ShowMainMenu();

            DisableIntentReceivers();

            if (PhotonNetwork.IsConnected)
            {
                PhotonNetwork.Disconnect();
            }
        }

        //Functions for players movements
        private bool IsGrounded()
        {
            return Physics.Raycast(raycastOrigin.position, Vector3.down, _ray);
        }

        [PunRPC]
        private void ActivateAvatarRPC(int avatarId)
        {
            _spawnPoint.position = new Vector3(avatarId, 0, 0);
            //TODO : change the path to get the right prefab to be instantiated
            PhotonNetwork.Instantiate("Resources/PrefabTest",_spawnPoint.position, Quaternion.identity, 0);
        }

        [PunRPC]
        private void DeactivateAvatarRPC(int avatarId)
        {
            players[avatarId].avatarRootGameObject.SetActive(false);
        }
    }
}