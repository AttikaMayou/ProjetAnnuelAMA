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

        private void FixedUpdate()
        {
            if (PlayerNumbering.SortedPlayers.Length <= playerIndex ||
                PlayerNumbering.SortedPlayers[playerIndex].ActorNumber != PhotonNetwork.LocalPlayer.ActorNumber)
            {
                return;
            }
            
            forward = Input.GetAxis("Vertical");
            strafe = Input.GetAxis("Horizontal");
            
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
                photonView.RPC("RunRPC", RpcTarget.MasterClient, true);
            }

            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                photonView.RPC("RunRPC", RpcTarget.MasterClient, false);
            }

            if (Input.GetKeyDown(KeyCode.LeftAlt))
            {
                photonView.RPC("DashRPC", RpcTarget.MasterClient);
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
                Debug.Log("I get the message : Move Left");
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
                Debug.Log("I get the message : Move Back");
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
                Debug.Log("I get the message : Move Right");
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
                Debug.Log("I get the message : Move Froward");
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
                Jump = true;
            }
        }

        [PunRPC]
        void DashRPC()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                Dash = true;
            }
        }

        [PunRPC]
        void RunRPC(bool intent)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                Run = intent;
            }
        }

        [PunRPC]
        void LaunchBulletRPC()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                Shoot = true;
            }
        }

        [PunRPC]
        void UseBumpRPC()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                Bump = true;
            }
        }

        [PunRPC]
        void InteractRPC()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                Interact = true;
            }
        }
        
    }
}
