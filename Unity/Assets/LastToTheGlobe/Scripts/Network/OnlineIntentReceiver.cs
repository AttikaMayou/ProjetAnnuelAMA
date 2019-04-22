using System;
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

        public bool debug = true;
        
        private void Update()
        {
            if (PlayerNumbering.SortedPlayers.Length <= playerIndex ||
                PlayerNumbering.SortedPlayers[playerIndex].ActorNumber != PhotonNetwork.LocalPlayer.ActorNumber)
            {
                return;
            }
            
            forward = Input.GetAxisRaw("Vertical");
            strafe = Input.GetAxisRaw("Horizontal");

            //Cooldown dash
            if (!canDash)
            {
                var timer = 0.0f;
                timer += Time.deltaTime;
                if (timer <= _dashTime)
                {
                    canDash = true;
                    return;
                }
            }
            
            //Movement Intent
            if (Input.GetKeyDown(KeyCode.Z))// && Math.Abs(forward) <  0.1f && Math.Abs(strafe) < 0.1f)
            {
                photonView.RPC("MoveForwardRPC", RpcTarget.MasterClient, true, forward, strafe);
            }
            
            if (Input.GetKeyUp(KeyCode.Z))
            {
                photonView.RPC("MoveForwardRPC", RpcTarget.MasterClient, false, 0, 0);
            }

            if (Input.GetKeyDown(KeyCode.S))// && Math.Abs(forward) <  0.1f && Math.Abs(strafe) < 0.1f)
            {
                photonView.RPC("MoveBackRPC", RpcTarget.MasterClient, true, forward, strafe);
            }

            if (Input.GetKeyUp(KeyCode.S))
            {
                photonView.RPC("MoveBackRPC", RpcTarget.MasterClient, false, 0, 0);
            }

            if (Input.GetKeyDown(KeyCode.Q))// && Math.Abs(forward) <  0.1f && Math.Abs(strafe) < 0.1f)
            {
                photonView.RPC("MoveLeftRPC", RpcTarget.MasterClient, true, forward, strafe);
            }

            if (Input.GetKeyUp(KeyCode.Q))
            {
                photonView.RPC("MoveLeftRPC", RpcTarget.MasterClient, false, 0, 0);
            }

            if (Input.GetKeyDown(KeyCode.D))// && Math.Abs(forward) <  0.1f && Math.Abs(strafe) < 0.1f)
            {
                photonView.RPC("MoveRightRPC", RpcTarget.MasterClient, true, forward, strafe);
            }

            if (Input.GetKeyUp(KeyCode.D))
            {
                photonView.RPC("MoveRightRPC", RpcTarget.MasterClient, false, 0, 0);
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

            if (Input.GetKeyUp(KeyCode.Space) && canJump)
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

        private void OnCollisionEnter(Collision other)
        {
            if (!other.gameObject.CompareTag("Planet"))
            {
                canJump = true;
            }
        }

        #region RPC
        [PunRPC]
        void MoveLeftRPC(bool intent, int forwardInput, int strafeInput)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                speed = walkSpeed;
                if (debug)
                {
                    Debug.Log("I get the message : Move Left on this avatar : " + playerIndex);
                    Debug.Log("Strafe value : " + strafeInput + " ; Forward value : " + forwardInput);
                }
                MoveLeft = intent;
                forward = forwardInput;
                strafe = strafeInput;
            }
        }
        
        [PunRPC]
        void MoveLeftRPC(bool intent, float forwardInput, float strafeInput)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                speed = walkSpeed;
                if (debug)
                {
                    Debug.Log("I get the message : Move Left on this avatar : " + playerIndex);
                    Debug.Log("Strafe value : " + strafeInput + " ; Forward value : " + forwardInput);
                }
                MoveLeft = intent;
                forward = forwardInput;
                strafe = strafeInput;
            }
        }

        [PunRPC]
        void MoveBackRPC(bool intent, int forwardInput, int strafeInput)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                speed = walkSpeed;
                if (debug)
                {
                    Debug.Log("I get the message : Move Left on this avatar : " + playerIndex);
                    Debug.Log("Strafe value : " + strafeInput + " ; Forward value : " + forwardInput);
                }
                MoveBack = intent;
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
                if (debug)
                {
                    Debug.Log("I get the message : Move Left on this avatar : " + playerIndex);
                    Debug.Log("Strafe value : " + strafeInput + " ; Forward value : " + forwardInput);
                }
                MoveBack = intent;
                forward = forwardInput;
                strafe = strafeInput;
            }
        }

        [PunRPC]
        void MoveRightRPC(bool intent, int forwardInput, int strafeInput)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                speed = walkSpeed;
                if (debug)
                {
                    Debug.Log("I get the message : Move Left on this avatar : " + playerIndex);
                    Debug.Log("Strafe value : " + strafeInput + " ; Forward value : " + forwardInput);
                }
                MoveRight = intent;
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
                if (debug)
                {
                    Debug.Log("I get the message : Move Left on this avatar : " + playerIndex);
                    Debug.Log("Strafe value : " + strafeInput + " ; Forward value : " + forwardInput);
                }
                MoveRight = intent;
                forward = forwardInput;
                strafe = strafeInput;
            }
        }

        [PunRPC]
        void MoveForwardRPC(bool intent, int forwardInput, int strafeInput)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                speed = walkSpeed;
                if (debug)
                {
                    Debug.Log("I get the message : Move Left on this avatar : " + playerIndex);
                    Debug.Log("Strafe value : " + strafeInput + " ; Forward value : " + forwardInput);
                }
                MoveForward = intent;
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
                if (debug)
                {
                    Debug.Log("I get the message : Move Left on this avatar : " + playerIndex);
                    Debug.Log("Strafe value : " + strafeInput + " ; Forward value : " + forwardInput);
                }
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
                if (debug)
                {
                    Debug.Log("I get the message : Move Left on this avatar : " + playerIndex);
                }
                canJump = false;
                Jump = true;
            }
        }

        [PunRPC]
        void DashRPC()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                if (debug)
                {
                    Debug.Log("I get the message : Move Left on this avatar : " + playerIndex);
                }
                Dash = true;
            }
        }

        [PunRPC]
        void RunRPC(bool intent)
        {
            if (PhotonNetwork.IsMasterClient)
            { 
                if (debug)
                {
                    Debug.Log("I get the message : Move Left on this avatar : " + playerIndex);
                }
                Run = intent;
            }
        }

        [PunRPC]
        void LaunchBulletRPC()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                if (debug)
                {
                    Debug.Log("I get the message : Move Left on this avatar : " + playerIndex);
                }
                Shoot = true;
            }
        }

        [PunRPC]
        void UseBumpRPC()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                if (debug)
                {
                    Debug.Log("I get the message : Move Left on this avatar : " + playerIndex);
                }
                Bump = true;
            }
        }

        [PunRPC]
        void InteractRPC()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                if (debug)
                {
                    Debug.Log("I get the message : Move Left on this avatar : " + playerIndex);
                }
                Interact = true;
            }
        }
        
        #endregion
    }
}
