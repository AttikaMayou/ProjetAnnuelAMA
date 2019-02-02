using System.Collections;
using System.Collections.Generic;
using LastToTheGlobe.Scripts.Avatar;
using UnityEngine;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;

//Auteur : Margot

namespace LastToTheGlobe.Scripts.Dev
{
    public class CharacterControllerRigidbody : MonoBehaviour
    {
        //Parameters for Photon and replication
        [SerializeField]
        private CharacterExposer[] players;

        [SerializeField]
        private Transform[] SpawnPoint;

        [SerializeField]
        private AIntentReceiver[] onlineIntentReceivers;

        [SerializeField]
        private SceneMenuController startGameControllerScript;

        [SerializeField]
        private PhotonView photonView;

        private AIntentReceiver[] activatedIntentReceivers;
        private bool GameStarted { get; set; }

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
        private float DashSpeed;

        [SerializeField]
        [Tooltip("Raycast position")]
        private Transform raycastOrigin;

        private float forward;
        private float strafe;
        private float speed = 5;
        private float ray = 0.4f;
        private bool isGrounded = false;
        private int jumpMax = 1;
        private bool dashAsked = false;



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
            forward = Input.GetAxis("Vertical");
            strafe = Input.GetAxis("Horizontal");
            Vector3 movement = new Vector3(strafe, 0.0f, forward);

            //Course
            Running();

            //Dash
            Dash();

            //Saut et double saut
            Jump();
        }

        void FixedUpdate()
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
            if (activatedIntentReceivers == null
                || players == null
                || players.Length != activatedIntentReceivers.Length)
            {
                Debug.LogError("There is something wrong with avatars and intents setup !");
                return;
            }

            //Movements
            var fallenAvatarsCount = 0;
            var activatedAvatarsCount = 0;

            for (var i = 0; i < activatedIntentReceivers.Length; i++)
            {
                var moveIntent = Vector3.zero;

                var intentReceiver = activatedIntentReceivers[i];
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

                        rb.velocity = new Vector3(strafe * speed, rb.velocity.y, forward * speed);
                    }

                    intentReceiver.Jump = false;
                }

                if (intentReceiver.MoveBack)
                {
                    moveIntent += rb.velocity = new Vector3(strafe * speed, rb.velocity.y, forward * speed); ;
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

                rb.velocity = new Vector3(strafe * speed, rb.velocity.y, forward * speed);

                fallenAvatarsCount++;

                if (activatedAvatarsCount - fallenAvatarsCount <= 1 && activatedAvatarsCount > 1)
                {
                    EndGame();
                }


            }





            //Dash
            if (dashAsked)
            {
                rb.velocity = new Vector3(strafe * DashSpeed, rb.velocity.y, forward * DashSpeed);
                dashAsked = false;
            }
            //Déplacement
            else
            {
                rb.velocity = new Vector3(strafe * speed, rb.velocity.y, forward * speed);
            }
        }

        //Functions for movements replication
        private void ChooseAndSubscribeToOnlineIntentReceivers()
        {
            activatedIntentReceivers = onlineIntentReceivers;
            ResetGame();
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
            if (activatedIntentReceivers == null)
            {
                return;
            }

            for (var i = 0; i < activatedIntentReceivers.Length; i++)
            {
                activatedIntentReceivers[i].enabled = false;
            }
        }

        private void EnableIntentReceivers()
        {
            if (activatedIntentReceivers == null)
            {
                return;
            }

            for (var i = 0; i < activatedIntentReceivers.Length; i++)
            {
                activatedIntentReceivers[i].enabled = true;
                activatedIntentReceivers[i].Dash = false;
                activatedIntentReceivers[i].Jump = false;
                activatedIntentReceivers[i].Run = false;
                activatedIntentReceivers[i].MoveLeft = false;
                activatedIntentReceivers[i].MoveBack = false;
                activatedIntentReceivers[i].MoveRight = false;
                activatedIntentReceivers[i].MoveForward = false;
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

        private void ResetGame()
        {
            for (var i = 0; i < players.Length; i++)
            {
                var avatar = players[i];
                avatar.rb.velocity = Vector3.zero;
                avatar.rb.angularVelocity = Vector3.zero;
                avatar.rb.position = SpawnPoint[i].position;
                avatar.rb.rotation = SpawnPoint[i].rotation;
                avatar.characterRbView.enabled = activatedIntentReceivers == onlineIntentReceivers;
            }

            EnableIntentReceivers();
            GameStarted = true;
        }

        private void EndGame()
        {
            GameStarted = false;
            activatedIntentReceivers = null;

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
            return Physics.Raycast(raycastOrigin.position, Vector3.down, ray);
        }

        private void Running()
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                speed = runSpeed;
            }
            else
            {
                speed = walkSpeed;
            }
        }

        private void Dash()
        {
            if (Input.GetKeyDown(KeyCode.LeftAlt))
            {
                dashAsked = true;
            }
        }

        private void Jump()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (jumpMax > 0)
                {
                    rb.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
                    jumpMax--;
                }
            }
            if (IsGrounded())
            {
                jumpMax = 1;
            }
        }










        [PunRPC]
        private void ActivateAvatarRPC(int avatarId)
        {
            players[avatarId].avatarRootGameObject.SetActive(true);
        }

        [PunRPC]
        private void DeactivateAvatarRPC(int avatarId)
        {
            players[avatarId].avatarRootGameObject.SetActive(false);
        }
    }
}



