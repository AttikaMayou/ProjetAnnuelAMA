using System;
using System.Runtime.CompilerServices;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

//Auteur : Margot
//Modification : Attika

namespace LastToTheGlobe.Scripts.Network
{
    public class OnlineIntentReceiver : AIntentReceiver
    {
        public bool debug = true;
        
        [SerializeField] private int playerIndex;

        [SerializeField] private PhotonView photonView;

        private float _dashTime = 1.0f;
        
        private void Update()
        {
            if (PlayerNumbering.SortedPlayers.Length <= playerIndex ||
                PlayerNumbering.SortedPlayers[playerIndex].ActorNumber != PhotonNetwork.LocalPlayer.ActorNumber)
            {
                return;
            }
            
            forward = Input.GetAxisRaw("Vertical");
            strafe = Input.GetAxisRaw("Horizontal");
            rotationOnX = Input.GetAxis("Mouse X");
            rotationOnY = Input.GetAxis("Mouse Y");

            //TODO : check if the rotation updates relative to previous rotation
            photonView.RPC("UpdateCameraRotation", RpcTarget.MasterClient, rotationOnX, rotationOnY);
            
            //Attack Intent
            if (Input.GetMouseButton(0) && canShoot)
            {
                canShoot = false;
            }
            
            if (!canShoot)
            {
                loadShotValue += Time.deltaTime;
                if(loadShotValue >= 1.5f && Input.GetMouseButtonUp(0))
                {
                    //shootLoaded = true;
                    photonView.RPC("LaunchLoadedBulletRPC", RpcTarget.MasterClient);
                    loadShotValue = 0.0f;
                }

                if (Input.GetMouseButtonUp(0))
                {
                    photonView.RPC("LaunchBulletRPC", RpcTarget.MasterClient);
                }
            }
            
            //Cooldown dash
            if (!canDash)
            {
                var timer = 0.0f;
                timer += Time.deltaTime;
                if (timer <= _dashTime)
                {
                    //canDash = true;
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
                //canDash = false;
            }

            if (Input.GetKeyUp(KeyCode.Space) && canJump)
            {
                photonView.RPC("JumpRPC", RpcTarget.MasterClient);
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                photonView.RPC("UseBumpRPC", RpcTarget.MasterClient);
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
            if (other.gameObject.CompareTag("Planet"))
            {
                canJump = true;
            }
        }

        #region RPC

        [PunRPC]
        void UpdateCameraRotation(float rotationX, float rotationY)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                if (debug)
                {
                    Debug.Log("I get the message : UpdateRotation on this avatar : " + playerIndex);
                    Debug.Log("X rotation : " + rotationX + " and Y : " + rotationY);
                    rotationOnX = rotationX;
                    rotationOnY = rotationY;
                }
            }
        }
        
        [PunRPC]
        void UpdateCameraRotation(int rotationX, int rotationY)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                if (debug)
                {
                    Debug.Log("I get the message : UpdateRotation on this avatar : " + playerIndex);
                    Debug.Log("X rotation : " + rotationX + " and Y : " + rotationY);
                    rotationOnX = rotationX;
                    rotationOnY = rotationY;
                }
            }
        }
        
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
                    Debug.Log("I get the message : Move Back on this avatar : " + playerIndex);
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
                    Debug.Log("I get the message : Move Back on this avatar : " + playerIndex);
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
                    Debug.Log("I get the message : Move Right on this avatar : " + playerIndex);
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
                    Debug.Log("I get the message : Move Right on this avatar : " + playerIndex);
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
                    Debug.Log("I get the message : Move Forward on this avatar : " + playerIndex);
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
                    Debug.Log("I get the message : Move Forward on this avatar : " + playerIndex);
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
                    Debug.Log("I get the message : Jump on this avatar : " + playerIndex);
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
                    Debug.Log("I get the message : Dash on this avatar : " + playerIndex);
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
                    Debug.Log("I get the message : Run on this avatar : " + playerIndex);
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
                    Debug.Log("I get the message : Shoot on this avatar : " + playerIndex);
                }

                canShoot = false;
                Shoot = true;
            }
        }

        [PunRPC]
        void LaunchLoadedBulletRPC()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                if (debug)
                {
                    Debug.Log("I get the message : Shoot Loaded on this avatar : " + playerIndex);
                }

                canShoot = false;
                ShootLoaded = true;
            }
        }

        [PunRPC]
        void UseBumpRPC()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                if (debug)
                {
                    Debug.Log("I get the message : Bump on this avatar : " + playerIndex);
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
                    Debug.Log("I get the message : Interact on this avatar : " + playerIndex);
                }
                Interact = true;
            }
        }
        
        #endregion
    }
}
