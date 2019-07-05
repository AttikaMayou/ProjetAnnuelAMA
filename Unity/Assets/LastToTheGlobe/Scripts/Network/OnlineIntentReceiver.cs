using Assets.LastToTheGlobe.Scripts.Management;
using LastToTheGlobe.Scripts.Management;
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
        public static bool Debug = true;

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

            if(lockCursor) Cursor.lockState = CursorLockMode.Locked; 
            
            forward = Input.GetAxisRaw("Vertical");
            strafe = Input.GetAxisRaw("Horizontal");
            rotationOnX = Input.GetAxisRaw("Mouse X");
            rotationOnY = Input.GetAxisRaw("Mouse Y");

            //TODO : check if the rotation updates relative to previous rotation
            photonView.RPC("UpdateCameraRotation", RpcTarget.MasterClient, rotationOnX, rotationOnY);
            
            //Attack Intent
            if (Input.GetMouseButton(0) && canShoot)
            {
                photonView.RPC("CanShootRpc", RpcTarget.MasterClient, false);
            }

            if (!canShoot)
            {
                loadShotValue += Time.deltaTime;
                if (Input.GetMouseButtonUp(0))
                {
                    photonView.RPC(loadShotValue >= GameVariablesScript.Instance.shootLoadTime ? "LaunchLoadedBulletRpc" : "LaunchBulletRpc",
                        RpcTarget.MasterClient);
                    loadShotValue = 0.0f;
                }
            }
            
            //Bump intent
            if (Input.GetKeyDown(KeyCode.R) && canBump)
            {
                photonView.RPC("CanBumpRpc", RpcTarget.MasterClient, false);
                photonView.RPC("UseBumpRpc", RpcTarget.MasterClient);
            }

            if (!canBump)
            {
                _bumpTimer += Time.deltaTime;
                if (_bumpTimer <= GameVariablesScript.Instance.bumpCooldown)
                {
                    _bumpTimer = 0.0f;
                    photonView.RPC("CanBumpRpc", RpcTarget.MasterClient, true);
                }
            }

            //Cooldown dash
            if (!canDash)
            {
                _dashTimer += Time.deltaTime;
                if (_dashTimer <= GameVariablesScript.Instance.dashCooldown)
                {
                    _dashTimer = 0.0f;
                    photonView.RPC("CanDashRpc", RpcTarget.MasterClient, true);
                }
            }
            
            //Movement Intent
            if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.S) 
                                            || Input.GetKeyDown(KeyCode.Q) 
                                            || Input.GetKeyDown(KeyCode.D))
            {
                photonView.RPC("MoveRpc", RpcTarget.MasterClient, true, forward, strafe);
            }
            
            if (Input.GetKeyUp(KeyCode.Z) || Input.GetKeyUp(KeyCode.S) 
                                          || Input.GetKeyUp(KeyCode.Q) 
                                          || Input.GetKeyUp(KeyCode.D))
            {
                photonView.RPC("MoveRpc", RpcTarget.MasterClient, false, 0.0f, 0.0f);
            }
            
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                speed = GameVariablesScript.Instance.runSpeed;
                photonView.RPC("RunRpc", RpcTarget.MasterClient, true);
            }

            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                speed = GameVariablesScript.Instance.walkSpeed;
                photonView.RPC("RunRpc", RpcTarget.MasterClient, false);
            }

            if (Input.GetKeyDown(KeyCode.LeftAlt) && canDash)
            {
                speed = GameVariablesScript.Instance.dashSpeed;
                photonView.RPC("CanDashRpc", RpcTarget.MasterClient, false);
                photonView.RPC("DashRpc", RpcTarget.MasterClient);
            }

//            if (Input.GetKeyUp(KeyCode.Space) && CanJump)
//            {
//                photonView.RPC("JumpRPC", RpcTarget.MasterClient);
//            }
            
            //Interaction Intent
            if (Input.GetKeyDown(KeyCode.E))
            {
                photonView.RPC("InteractRpc", RpcTarget.MasterClient);
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
            rotationOnX = rotationX;
            rotationOnY = rotationY;
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
            rotationOnX = rotationX;
            rotationOnY = rotationY;
        }

        [PunRPC]
        void MoveRpc(bool intent, int forwardInput, int strafeInput)
        {
            if (!PhotonNetwork.IsMasterClient) return;
            speed = GameVariablesScript.Instance.walkSpeed;
//            if (debug)
//            {
//                Debug.LogFormat("[IntentReceiver] I get the message : Move on this avatar : {0}", 
//                    playerIndex);
//                Debug.LogFormat("[IntentReceiver] Strafe value : {0}; Forward value : {1}", 
//                    strafeInput, forwardInput);
//            }
            Move = intent;
            forward = forwardInput;
            strafe = strafeInput;
        }
        
        [PunRPC]
        void MoveRpc(bool intent, float forwardInput, float strafeInput)
        {
            if (!PhotonNetwork.IsMasterClient) return;
            speed = GameVariablesScript.Instance.walkSpeed;
//            if (debug)
//            {
//                Debug.LogFormat("[IntentReceiver] I get the message : Move on this avatar : {0}",
//                    playerIndex);
//                Debug.LogFormat("[IntentReceiver] Strafe value : {0}; Forward value : {1}", 
//                    strafeInput, forwardInput);
//            }
            Move = intent;
            forward = forwardInput;
            strafe = strafeInput;
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
            canShoot = intent;
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
            canBump = intent;
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
            canDash = intent;
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
