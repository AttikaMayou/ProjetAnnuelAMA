using System.Runtime.CompilerServices;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using UnityEngine;

//Auteur : Margot
//Modification : Attika

namespace LastToTheGlobe.Scripts.Network
{
    public class OnlineIntentReceiver : AIntentReceiver
    {
        [SerializeField] private int playerIndex;

        [SerializeField] private PhotonView photonView;

        private float _dashTime = 1.0f;

        private void FixedUpdate()
        {
            if (PlayerNumbering.SortedPlayers.Length <= playerIndex ||
                PlayerNumbering.SortedPlayers[playerIndex].ActorNumber != PhotonNetwork.LocalPlayer.ActorNumber)
            {
                return;
            }
            
            forward = Input.GetAxis("Vertical");
            strafe = Input.GetAxis("Horizontal");

            if (!canDash)
            {
                var timer = 0.0f;
                timer += Time.deltaTime;
                if (timer <= _dashTime)
                {
                    canDash = true;
                }
            }
            
            //Movement Intent
            if (Input.GetKeyDown(KeyCode.Z))
            {
                photonView.RPC("MoveForwardRPC", RpcTarget.MasterClient, true, forward, strafe);
            }
            
            if (Input.GetKeyUp(KeyCode.Z))
            {
                photonView.RPC("MoveForwardRPC", RpcTarget.MasterClient, false, forward, strafe);
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                photonView.RPC("MoveBackRPC", RpcTarget.MasterClient, true, forward, strafe);
            }

            if (Input.GetKeyUp(KeyCode.S))
            {
                photonView.RPC("MoveBackRPC", RpcTarget.MasterClient, false, forward, strafe);
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                photonView.RPC("MoveLeftRPC", RpcTarget.MasterClient, true, forward, strafe);
            }

            if (Input.GetKeyUp(KeyCode.Q))
            {
                photonView.RPC("MoveLeftRPC", RpcTarget.MasterClient, false, forward, strafe);
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                photonView.RPC("MoveRightRPC", RpcTarget.MasterClient, true, forward, strafe);
            }

            if (Input.GetKeyUp(KeyCode.D))
            {
                photonView.RPC("MoveRightRPC", RpcTarget.MasterClient, false, forward, strafe);
            }
            
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                speed = runSpeed;
                photonView.RPC("RunRPC", RpcTarget.MasterClient, true);
            }

            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                speed = walkSpeed;
                photonView.RPC("RunRPC", RpcTarget.MasterClient, false);
            }

            if (Input.GetKeyDown(KeyCode.LeftAlt) && canDash)
            {
                speed = dashSpeed;
                photonView.RPC("DashRPC", RpcTarget.MasterClient);
                canDash = false;
            }

            if (Input.GetKeyUp(KeyCode.Space))
            {
                photonView.RPC("JumpRPC", RpcTarget.MasterClient);
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                photonView.RPC("UseBumpRPC", RpcTarget.MasterClient);
            }
            
            //Attack Intent
            if (Input.GetMouseButtonDown(0))
            {
                photonView.RPC("LaunchBulletRPC", RpcTarget.MasterClient);
            }
            
            //Interaction Intent
            if (Input.GetKeyDown(KeyCode.E))
            {
                photonView.RPC("InteractRPC", RpcTarget.MasterClient);
            }

            //TODO : Add double jump
        }
        
        [PunRPC]
        void MoveLeftRPC(bool intent, float forwardInput, float strafeInput)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                speed = walkSpeed;
                Debug.Log("I get the message : Move Left on this avatar : " + playerIndex);
                MoveLeft = intent;
                forward = forwardInput;
                strafe = strafeInput;
            }
        }

        [PunRPC]
        void MoveBackRPC(bool intent, float forwardInput, float strafeInput)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                speed = walkSpeed;
                Debug.Log("I get the message : Move Back on this avatar : " + playerIndex);
                MoveBack = intent;
                forward = forwardInput;
                strafe = strafeInput;
            }
        }

        [PunRPC]
        void MoveRightRPC(bool intent, float forwardInput, float strafeInput)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                speed = walkSpeed;
                Debug.Log("I get the message : Move Right on this avatar : " + playerIndex);
                MoveRight = intent;
                forward = forwardInput;
                strafe = strafeInput;
            }
        }

        [PunRPC]
        void MoveForwardRPC(bool intent, float forwardInput, float strafeInput)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                speed = walkSpeed;
                Debug.Log("I get the message : Move Froward on this avatar : " + playerIndex);
                MoveForward = intent;
                forward = forwardInput;
                strafe = strafeInput;
            }
        }

        [PunRPC]
        void JumpRPC()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                Debug.Log("I get the message : Jump on this avatar : " + playerIndex);
                Jump = true;
            }
        }

        [PunRPC]
        void DashRPC()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                Debug.Log("I get the message : Dash on this avatar : " + playerIndex);
                Dash = true;
            }
        }

        [PunRPC]
        void RunRPC(bool intent)
        {
            if (PhotonNetwork.IsMasterClient)
            { 
                Debug.Log("I get the message : Run on this avatar : " + playerIndex);
                Run = intent;
            }
        }

        [PunRPC]
        void LaunchBulletRPC()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                Debug.Log("I get the message : Shoot on this avatar : " + playerIndex);
                Shoot = true;
            }
        }

        [PunRPC]
        void UseBumpRPC()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                Debug.Log("I get the message : Bump on this avatar : " + playerIndex);
                Bump = true;
            }
        }

        [PunRPC]
        void InteractRPC()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                Debug.Log("I get the message : Interact on this avatar : " + playerIndex);
                Interact = true;
            }
        }
        
    }
}
