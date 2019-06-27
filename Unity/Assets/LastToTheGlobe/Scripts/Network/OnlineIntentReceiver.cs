using Assets.LastToTheGlobe.Scripts.Management;
using Assets.LastToTheGlobe.Scripts.Network;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using UnityEngine;
using UnityEngine.Serialization;

//Auteur : Margot
//Modification : Attika

namespace LastToTheGlobe.Scripts.Network
{
    public class OnlineIntentReceiver : AIntentReceiver
    {
        private const bool Debug = true;

        [FormerlySerializedAs("_playerIndex")] [SerializeField] private int playerIndex;

        [FormerlySerializedAs("_photonView")] [SerializeField] private PhotonView photonView;

        //Cooldown Timers
        private float _bumpTimer;
        private float _dashTimer;
        
        private void Update()
        {
            if (PlayerNumbering.SortedPlayers.Length <= playerIndex ||
                PlayerNumbering.SortedPlayers[playerIndex].ActorNumber != PhotonNetwork.LocalPlayer.ActorNumber)
            {
                return;
            }

            Cursor.lockState = CursorLockMode.Locked; 
            
            Forward = Input.GetAxisRaw("Vertical");
            Strafe = Input.GetAxisRaw("Horizontal");
            RotationOnX = Input.GetAxis("Mouse X");
            RotationOnY = Input.GetAxis("Mouse Y");

            //TODO : check if the rotation updates relative to previous rotation
            photonView.RPC("UpdateCameraRotation", RpcTarget.MasterClient, RotationOnX, RotationOnY);
            
            //Attack Intent
            if (Input.GetMouseButton(0) && CanShoot)
            {
                photonView.RPC("CanShootRPC", RpcTarget.MasterClient, false);
            }

            if (!CanShoot)
            {
                LoadShotValue += Time.deltaTime;
                if (Input.GetMouseButtonUp(0))
                {
                    photonView.RPC(LoadShotValue >= GameVariablesScript.Instance.ShootLoadTime ? "LaunchLoadedBulletRPC" : "LaunchBulletRPC",
                        RpcTarget.MasterClient);
                    LoadShotValue = 0.0f;
                }
            }
            
            //Bump intent
            if (Input.GetKeyDown(KeyCode.R) && CanBump)
            {
                photonView.RPC("CanBumpRPC", RpcTarget.MasterClient, false);
                photonView.RPC("UseBumpRPC", RpcTarget.MasterClient);
            }

            if (!CanBump)
            {
                _bumpTimer += Time.deltaTime;
                if (_bumpTimer <= GameVariablesScript.Instance.BumpCooldown)
                {
                    _bumpTimer = 0.0f;
                    photonView.RPC("CanBumpRPC", RpcTarget.MasterClient, true);
                }
            }

            //Cooldown dash
            if (!CanDash)
            {
                _dashTimer += Time.deltaTime;
                if (_dashTimer <= GameVariablesScript.Instance.DashCooldown)
                {
                    _dashTimer = 0.0f;
                    photonView.RPC("CanDashRPC", RpcTarget.MasterClient, true);
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
                photonView.RPC("MoveRPC", RpcTarget.MasterClient, false, 0.0f, 0.0f);
            }
            
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                Speed = GameVariablesScript.Instance.RunSpeed;
                photonView.RPC("RunRPC", RpcTarget.MasterClient, true);
            }

            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                Speed = GameVariablesScript.Instance.WalkSpeed;
                photonView.RPC("RunRPC", RpcTarget.MasterClient, false);
            }

            if (Input.GetKeyDown(KeyCode.LeftAlt) && CanDash)
            {
                Speed = GameVariablesScript.Instance.DashSpeed;
                photonView.RPC("CanDashRPC", RpcTarget.MasterClient, false);
                photonView.RPC("DashRPC", RpcTarget.MasterClient);
            }

//            if (Input.GetKeyUp(KeyCode.Space) && CanJump)
//            {
//                photonView.RPC("JumpRPC", RpcTarget.MasterClient);
//            }
            
            //Interaction Intent
            if (Input.GetKeyDown(KeyCode.E))
            {
                photonView.RPC("InteractRPC", RpcTarget.MasterClient);
            }
        }

        #region RPC

        [PunRPC]
        void UpdateCameraRotation(float rotationX, float rotationY)
        {
            if (!PhotonNetwork.IsMasterClient) return;
//            if (debug)
//            {
//                Debug.LogFormat("[IntentReceiver] I get the message : UpdateRotation on this avatar : {0}",
//                    playerIndex);
//                Debug.LogFormat("[IntentReceiver] X rotation : {0} and Y : {0}", 
//                    rotationX, rotationY);
//            }
            RotationOnX = rotationX;
            RotationOnY = rotationY;
        }
        
        [PunRPC]
        void UpdateCameraRotation(int rotationX, int rotationY)
        {
            if (!PhotonNetwork.IsMasterClient) return;
//            if (debug)
//            {
//                Debug.LogFormat("[IntentReceiver] I get the message : UpdateRotation on this avatar : {0}",
//                    playerIndex);
//                Debug.LogFormat("[IntentReceiver] X rotation : {0} and Y : {0}", 
//                    rotationX, rotationY);
//            }
            RotationOnX = rotationX;
            RotationOnY = rotationY;
        }

        [PunRPC]
        void MoveRpc(bool intent, int forwardInput, int strafeInput)
        {
            if (!PhotonNetwork.IsMasterClient) return;
            Speed = GameVariablesScript.Instance.WalkSpeed;
//            if (debug)
//            {
//                Debug.LogFormat("[IntentReceiver] I get the message : Move on this avatar : {0}", 
//                    playerIndex);
//                Debug.LogFormat("[IntentReceiver] Strafe value : {0}; Forward value : {1}", 
//                    strafeInput, forwardInput);
//            }
            Move = intent;
            Forward = forwardInput;
            Strafe = strafeInput;
        }
        
        [PunRPC]
        void MoveRpc(bool intent, float forwardInput, float strafeInput)
        {
            if (!PhotonNetwork.IsMasterClient) return;
            Speed = GameVariablesScript.Instance.WalkSpeed;
//            if (debug)
//            {
//                Debug.LogFormat("[IntentReceiver] I get the message : Move on this avatar : {0}",
//                    playerIndex);
//                Debug.LogFormat("[IntentReceiver] Strafe value : {0}; Forward value : {1}", 
//                    strafeInput, forwardInput);
//            }
            Move = intent;
            Forward = forwardInput;
            Strafe = strafeInput;
        }

        /*[PunRPC]
        void JumpRPC()
        {
            if (PhotonNetwork.IsMasterClient)
            {/*
                if (debug)
                {
                    Debug.Log("I get the message : Jump on this avatar : " + playerIndex);
                }
                CanJump = false;
                Jump = true;
            }
        }*/

        [PunRPC]
        void DashRpc()
        {
            if (!PhotonNetwork.IsMasterClient) return;
            if (Debug)
            {
                UnityEngine.Debug.LogFormat("[IntentReceiver] I get the message : Dash on this avatar : {0}",
                    playerIndex);
            }
            Dash = true;
        }

        [PunRPC]
        void RunRpc(bool intent)
        {
            if (!PhotonNetwork.IsMasterClient) return;
            
            if (Debug)
            {
                UnityEngine.Debug.LogFormat("[IntentReceiver] I get the message : Run on this avatar : {0}",
                    playerIndex);
            }
            Run = intent;
        }

        [PunRPC]
        void CanShootRpc(bool intent)
        {
            if (!PhotonNetwork.IsMasterClient) return;

            if (Debug)
            {
                UnityEngine.Debug.LogFormat("[IntentReceiver] I get the message : CanShoot on this avatar : {0} passed to {1}",
                    playerIndex, intent);
            }
            CanShoot = intent;
        }

        [PunRPC]
        void CanBumpRpc(bool intent)
        {
            if (!PhotonNetwork.IsMasterClient) return;

            if (Debug)
            {
                UnityEngine.Debug.LogFormat("[IntentReceiver] I get the message : CanBump on this avatar : {0} passed to {1}",
                    playerIndex, intent);
            }
            CanBump = intent;
        }

        [PunRPC]
        void CanDashRpc(bool intent)
        {
            if (!PhotonNetwork.IsMasterClient) return;

            if (Debug)
            {
                UnityEngine.Debug.LogFormat("[IntentReceiver] I get the message : CanDash on this avatar : {0} passed to {1}",
                    playerIndex, intent);
            }
            CanDash = intent;
        }
        
        [PunRPC]
        void LaunchBulletRpc()
        {
            if (!PhotonNetwork.IsMasterClient) return;
            
            if (Debug)
            {
                UnityEngine.Debug.LogFormat("[IntentReceiver] I get the message : Shoot on this avatar : {0}",
                    playerIndex);
            }
            Shoot = true;
        }

        [PunRPC]
        void LaunchLoadedBulletRpc()
        {
            if (!PhotonNetwork.IsMasterClient) return;
            
            if (Debug)
            {
                UnityEngine.Debug.LogFormat("[IntentReceiver] I get the message : Shoot Loaded on this avatar : {0}",
                    playerIndex);
            }
            ShootLoaded = true;
        }

        [PunRPC]
        void UseBumpRpc()
        {
            if (!PhotonNetwork.IsMasterClient) return;
            if (Debug)
            {
                UnityEngine.Debug.LogFormat("[IntentReceiver] I get the message : Bump on this avatar : {0}",
                    playerIndex);
            }
            Bump = true;
        }

        [PunRPC]
        void InteractRpc()
        {
            if (!PhotonNetwork.IsMasterClient) return;
            if (Debug)
            {
                UnityEngine.Debug.LogFormat("[IntentReceiver] I get the message : Interact on this avatar : {0}",
                    playerIndex);
            }
            Interact = true;
        }
        
        #endregion
    }
}
