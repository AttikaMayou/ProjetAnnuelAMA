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
        public static bool debug = true;
        
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
            
            Forward = Input.GetAxisRaw("Vertical");
            Strafe = Input.GetAxisRaw("Horizontal");
            RotationOnX = Input.GetAxis("Mouse X");
            RotationOnY = Input.GetAxis("Mouse Y");

            //TODO : check if the rotation updates relative to previous rotation
            photonView.RPC("UpdateCameraRotation", RpcTarget.MasterClient, RotationOnX, RotationOnY);
            
            //Attack Intent
            if (Input.GetMouseButton(0) && CanShoot)
            {
                CanShoot = false;
            }
            
            if (!CanShoot)
            {
                LoadShotValue += Time.deltaTime;
                if(LoadShotValue >= 1.5f && Input.GetMouseButtonUp(0))
                {
                    //shootLoaded = true;
                    photonView.RPC("LaunchLoadedBulletRPC", RpcTarget.MasterClient);
                    LoadShotValue = 0.0f;
                }

                if (Input.GetMouseButtonUp(0))
                {
                    photonView.RPC("LaunchBulletRPC", RpcTarget.MasterClient);
                }
            }
            
            //Cooldown dash
            if (!CanDash)
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
            if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.S) 
                                            || Input.GetKeyDown(KeyCode.Q) 
                                            || Input.GetKeyDown(KeyCode.D))
            {
                photonView.RPC("MoveRPC", RpcTarget.MasterClient, true, Forward, Strafe);
            }
            
            if (Input.GetKeyUp(KeyCode.Z) || Input.GetKeyUp(KeyCode.S) 
                                          || Input.GetKeyUp(KeyCode.Q) 
                                          || Input.GetKeyUp(KeyCode.D))
            {
                photonView.RPC("MoveRPC", RpcTarget.MasterClient, false, 0, 0);
            }
            
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                Speed = RunSpeed;
                photonView.RPC("RunRPC", RpcTarget.MasterClient, true);
            }

            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                Speed = WalkSpeed;
                photonView.RPC("RunRPC", RpcTarget.MasterClient, false);
            }

            if (Input.GetKeyDown(KeyCode.LeftAlt) && CanDash)
            {
                Speed = DashSpeed;
                photonView.RPC("DashRPC", RpcTarget.MasterClient);
                //canDash = false;
            }

            if (Input.GetKeyUp(KeyCode.Space) && CanJump)
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
                CanJump = true;
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
                }
                RotationOnX = rotationX;
                RotationOnY = rotationY;
            }
        }
        
        [PunRPC]
        void UpdateCameraRotation(int rotationX, int rotationY)
        {
            if (PhotonNetwork.IsMasterClient)
            {
               /* if (debug)
                {
                    Debug.Log("I get the message : UpdateRotation on this avatar : " + playerIndex);
                    Debug.Log("X rotation : " + rotationX + " and Y : " + rotationY);
                }*/
                RotationOnX = rotationX;
                RotationOnY = rotationY;
            }
        }

        [PunRPC]
        void MoveRPC(bool intent, int forwardInput, int strafeInput)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                Speed = WalkSpeed;
                if (debug)
                {
                    Debug.LogFormat("I get the message : Move on this avatar : {0}", playerIndex);
                    Debug.LogFormat("Strafe value : {0}; Forward value : {1}", 
                        strafeInput, forwardInput);
                }
                Move = intent;
                Forward = forwardInput;
                Strafe = strafeInput;
            }
        }
        
        [PunRPC]
        void MoveRPC(bool intent, float forwardInput, float strafeInput)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                Speed = WalkSpeed;
                if (debug)
                {
                    Debug.LogFormat("I get the message : Move on this avatar : {0}", playerIndex);
                    Debug.LogFormat("Strafe value : {0}; Forward value : {1}", 
                        strafeInput, forwardInput);
                }
                Move = intent;
                Forward = forwardInput;
                Strafe = strafeInput;
            }
        }

        [PunRPC]
        void JumpRPC()
        {
            if (PhotonNetwork.IsMasterClient)
            {/*
                if (debug)
                {
                    Debug.Log("I get the message : Jump on this avatar : " + playerIndex);
                }*/
                CanJump = false;
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

                CanShoot = false;
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

                CanShoot = false;
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
