using Assets.LastToTheGlobe.Scripts.Management;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using UnityEngine;

//Auteur : Margot
//Modification : Attika

namespace Assets.LastToTheGlobe.Scripts.Network
{
    public class OnlineIntentReceiver : AIntentReceiver
    {
        public static bool debug = true;
        
        [SerializeField] private int _playerIndex;

        [SerializeField] private PhotonView _photonView;

        private float _dashTime = 1.0f; 
        
        private void Update()
        {
            if (PlayerNumbering.SortedPlayers.Length <= _playerIndex ||
                PlayerNumbering.SortedPlayers[_playerIndex].ActorNumber != PhotonNetwork.LocalPlayer.ActorNumber)
            {
                return;
            }

            Cursor.lockState = CursorLockMode.Locked; 
            
            Forward = Input.GetAxisRaw("Vertical");
            Strafe = Input.GetAxisRaw("Horizontal");
            RotationOnX = Input.GetAxis("Mouse X");
            RotationOnY = Input.GetAxis("Mouse Y");

            //TODO : check if the rotation updates relative to previous rotation
            _photonView.RPC("UpdateCameraRotation", RpcTarget.MasterClient, RotationOnX, RotationOnY);
            
            //Attack Intent
            if (Input.GetMouseButton(0) && CanShoot)
            {
                _photonView.RPC("CanShootRPC", RpcTarget.MasterClient, false);
            }

            if (!CanShoot)
            {
                LoadShotValue += Time.deltaTime;
                if (Input.GetMouseButtonUp(0))
                {
                    _photonView.RPC(LoadShotValue >= GameVariablesScript.Instance.ShootLoadTime ? "LaunchLoadedBulletRPC" : "LaunchBulletRPC",
                        RpcTarget.MasterClient);
                    LoadShotValue = 0.0f;
                }
            }
            
            //Bump intent
            if (Input.GetKeyDown(KeyCode.R) && CanBump)
            {
                _photonView.RPC("CanBumpRPC", RpcTarget.MasterClient, false);
                _photonView.RPC("UseBumpRPC", RpcTarget.MasterClient);
            }

            if (!CanBump)
            {
                //TODO : deal with cooldown here   
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
                _photonView.RPC("MoveRPC", RpcTarget.MasterClient, true, Forward, Strafe);
            }
            
            if (Input.GetKeyUp(KeyCode.Z) || Input.GetKeyUp(KeyCode.S) 
                                          || Input.GetKeyUp(KeyCode.Q) 
                                          || Input.GetKeyUp(KeyCode.D))
            {
                _photonView.RPC("MoveRPC", RpcTarget.MasterClient, false, 0, 0);
            }
            
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                Speed = GameVariablesScript.Instance.RunSpeed;
                _photonView.RPC("RunRPC", RpcTarget.MasterClient, true);
            }

            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                Speed = GameVariablesScript.Instance.WalkSpeed;
                _photonView.RPC("RunRPC", RpcTarget.MasterClient, false);
            }

            if (Input.GetKeyDown(KeyCode.LeftAlt) && CanDash)
            {
                Speed = GameVariablesScript.Instance.DashSpeed;
                _photonView.RPC("DashRPC", RpcTarget.MasterClient);
                //canDash = false;
            }

//            if (Input.GetKeyUp(KeyCode.Space) && CanJump)
//            {
//                photonView.RPC("JumpRPC", RpcTarget.MasterClient);
//            }
            
            //Interaction Intent
            if (Input.GetKeyDown(KeyCode.E))
            {
                _photonView.RPC("InteractRPC", RpcTarget.MasterClient);
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
        void MoveRPC(bool intent, int forwardInput, int strafeInput)
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
        void MoveRPC(bool intent, float forwardInput, float strafeInput)
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
        void DashRPC()
        {
            if (!PhotonNetwork.IsMasterClient) return;
            if (debug)
            {
                Debug.LogFormat("[IntentReceiver] I get the message : Dash on this avatar : {0}",
                    _playerIndex);
            }
            Dash = true;
        }

        [PunRPC]
        void RunRPC(bool intent)
        {
            if (!PhotonNetwork.IsMasterClient) return;
            
            if (debug)
            {
                Debug.LogFormat("[IntentReceiver] I get the message : Run on this avatar : {0}",
                    _playerIndex);
            }
            Run = intent;
        }

        [PunRPC]
        void CanShootRPC(bool intent)
        {
            if (!PhotonNetwork.IsMasterClient) return;

            if (debug)
            {
                Debug.LogFormat("[IntentReceiver] I get the message : CanShoot on this avatar : {0} passed to {1}",
                    _playerIndex, intent);
            }
            CanShoot = intent;
        }

        [PunRPC]
        void CanBumpRPC(bool intent)
        {
            if (!PhotonNetwork.IsMasterClient) return;

            if (debug)
            {
                Debug.LogFormat("[IntentReceiver] I get the message : CanBump on this avatar : {0} passed to {1}",
                    _playerIndex, intent);
            }
            CanBump = intent;
        }
        
        [PunRPC]
        void LaunchBulletRPC()
        {
            if (!PhotonNetwork.IsMasterClient) return;
            
            if (debug)
            {
                Debug.LogFormat("[IntentReceiver] I get the message : Shoot on this avatar : {0}",
                    _playerIndex);
            }
            Shoot = true;
        }

        [PunRPC]
        void LaunchLoadedBulletRPC()
        {
            if (!PhotonNetwork.IsMasterClient) return;
            
            if (debug)
            {
                Debug.LogFormat("[IntentReceiver] I get the message : Shoot Loaded on this avatar : {0}",
                    _playerIndex);
            }
            ShootLoaded = true;
        }

        [PunRPC]
        void UseBumpRPC()
        {
            if (!PhotonNetwork.IsMasterClient) return;
            if (debug)
            {
                Debug.LogFormat("[IntentReceiver] I get the message : Bump on this avatar : {0}",
                    _playerIndex);
            }
            Bump = true;
        }

        [PunRPC]
        void InteractRPC()
        {
            if (!PhotonNetwork.IsMasterClient) return;
            if (debug)
            {
                Debug.LogFormat("[IntentReceiver] I get the message : Interact on this avatar : {0}",
                    _playerIndex);
            }
            Interact = true;
        }
        
        #endregion
    }
}
