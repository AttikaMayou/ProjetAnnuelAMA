using LastToTheGlobe.Scripts.Dev;
using Photon.Pun;
using UnityEngine;

//Auteur : Margot
//Modification : Attika

namespace LastToTheGlobe.Scripts.Avatar
{
    public class CharacterControllerRigidbody : MonoBehaviour
    {
        [Header("Photon and Replication Parameters")]
        [SerializeField]
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

        //Parameters for players and movements control
        [SerializeField]
        [Tooltip("Character")]
        private Rigidbody rb;

        [SerializeField]
        [Tooltip("Vitesse de la course")]
        private float runSpeed;

        [SerializeField]
        [Tooltip("Vitesse de la marche")]
        private float walkSpeed;

        [SerializeField]
        [Tooltip("Vitesse du saut")]
        private float jumpSpeed;

        [SerializeField]
        [Tooltip("Vitesse du dash")]
        private float dashSpeed;

        [SerializeField]
        [Tooltip("Raycast position")]
        private Transform raycastOrigin;

        private float _forward;
        private float _strafe;
        private float _speed = 5;
        private float _ray = 0.4f;
        private bool _isGrounded = false;
        private int _jumpMax = 1;
        private bool _dashAsked = false;



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
            rb = GetComponent<Rigidbody>();
        }

        void Update()
        {
            // Récupération des floats vertical et horizontal de l'animator au script
            _forward = Input.GetAxis("Vertical");
            _strafe = Input.GetAxis("Horizontal");
            Vector3 movement = new Vector3(_strafe, 0.0f, _forward);

            //Course
            Running();

            //Dash
            Dash();

            //Saut et double saut
            Jump();
        }

        private void FixedUpdate()
        {
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

            //Movements
            var fallenAvatarsCount = 0;
            var activatedAvatarsCount = 0;

            for (var i = 0; i < _activatedIntentReceivers.Length; i++)
            {
                var moveIntent = Vector3.zero;

                var intentReceiver = _activatedIntentReceivers[i];
                var avatar = players[i];

                activatedAvatarsCount += avatar.avatarRootGameObject.activeSelf ? 1 : 0;

                if (intentReceiver.Jump)
                {
                    for (var j = 0; j < players.Length; j++)
                    {
                        if (i == j)
                        {
                            continue;
                        }

                        rb.velocity = new Vector3(_strafe * _speed, rb.velocity.y, _forward * _speed);
                    }

                    intentReceiver.Jump = false;
                }

                if (intentReceiver.MoveBack)
                {
                    moveIntent += rb.velocity = new Vector3(_strafe * _speed, rb.velocity.y, _forward * _speed); ;
                }

                /*if (intentReceiver.MoveForward)
                {
                    moveIntent += rb.velocity = new Vector3(strafe * speed, rb.velocity.y, forward * speed); ;
                }

                if (intentReceiver.WantToMoveLeft)
                {
                    moveIntent += Vector3.left;
                }

                if (intentReceiver.WantToMoveRight)
                {
                    moveIntent += Vector3.right;
                }*/

                moveIntent = moveIntent.normalized;

                rb.velocity = new Vector3(_strafe * _speed, rb.velocity.y, _forward * _speed);

                fallenAvatarsCount++;

                if (activatedAvatarsCount - fallenAvatarsCount <= 1 && activatedAvatarsCount > 1)
                {
                    EndGame();
                }


            }





            //Dash
            if (_dashAsked)
            {
                rb.velocity = new Vector3(_strafe * dashSpeed, rb.velocity.y, _forward * dashSpeed);
                _dashAsked = false;
            }
            //Déplacement
            else
            {
                rb.velocity = new Vector3(_strafe * _speed, rb.velocity.y, _forward * _speed);
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

        private void Running()
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                _speed = runSpeed;
            }
            else
            {
                _speed = walkSpeed;
            }
        }

        private void Dash()
        {
            if (Input.GetKeyDown(KeyCode.LeftAlt))
            {
                _dashAsked = true;
            }
        }

        private void Jump()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (_jumpMax > 0)
                {
                    rb.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
                    _jumpMax--;
                }
            }
            if (IsGrounded())
            {
                _jumpMax = 1;
            }
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