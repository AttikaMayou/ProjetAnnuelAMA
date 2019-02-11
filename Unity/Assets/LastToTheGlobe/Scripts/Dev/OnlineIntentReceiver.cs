using Photon.Pun;
using Photon.Pun.UtilityScripts;
using UnityEngine;
using UnityEngine.Serialization;

//Auteur : Margot
//Modification : Attika

namespace LastToTheGlobe.Scripts.Dev
{
    public class OnlineIntentReceiver : AIntentReceiver
    {
        [FormerlySerializedAs("PlayerIndex")]
        [FormerlySerializedAs("PlayerActorId")]
        [SerializeField]
        private int playerIndex;

        [SerializeField]
        private PhotonView photonView;

        public void Update()
        {
//            if (PlayerNumbering.SortedPlayers.Length <= PlayerIndex ||
//             PlayerNumbering.SortedPlayers[PlayerIndex].ActorNumber != PhotonNetwork.LocalPlayer.ActorNumber)
//            {
//                return;
//            }

            //Movement Intent
            if (Input.GetKeyDown(KeyCode.Z))
            {
                photonView.RPC("MoveForwardRPC", RpcTarget.MasterClient, true);
            }

            if (Input.GetKeyDown(KeyCode.Z))
            {
                photonView.RPC("MoveForwardRPC", RpcTarget.MasterClient, false);
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                photonView.RPC("MoveBackRPC", RpcTarget.MasterClient, true);
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                photonView.RPC("MoveBackRPC", RpcTarget.MasterClient, false);
            }

            if (Input.GetKeyUp(KeyCode.Q))
            {
                photonView.RPC("MoveLeftRPC", RpcTarget.MasterClient, true);
            }

            if (Input.GetKeyUp(KeyCode.Q))
            {
                photonView.RPC("MoveLeftRPC", RpcTarget.MasterClient, false);
            }

            if (Input.GetKeyUp(KeyCode.D))
            {
                photonView.RPC("MoveRightRPC", RpcTarget.MasterClient, true);
            }

            if (Input.GetKeyUp(KeyCode.D))
            {
                photonView.RPC("MoveRightRPC", RpcTarget.MasterClient, false);
            }

            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                photonView.RPC("RunRPC", RpcTarget.MasterClient, true);
            }

            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                photonView.RPC("RunRPC", RpcTarget.MasterClient, false);
            }

            if (Input.GetKeyUp(KeyCode.LeftAlt))
            {
                photonView.RPC("DashRPC", RpcTarget.MasterClient, true);
            }

            if (Input.GetKeyUp(KeyCode.LeftAlt))
            {
                photonView.RPC("DashRPC", RpcTarget.MasterClient, false);
            }

            if (Input.GetKeyUp(KeyCode.Space))
            {
                photonView.RPC("JumpRPC", RpcTarget.MasterClient, true);
            }

            //Ajout double jump
        }

        [PunRPC]
        void MoveLeftRPC(bool intent)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                MoveLeft = intent;
            }
        }

        [PunRPC]
        void MoveBackRPC(bool intent)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                MoveBack = intent;
            }
        }

        [PunRPC]
        void MoveRightRPC(bool intent)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                MoveRight = intent;
            }
        }

        [PunRPC]
        void MoveForwardRPC(bool intent)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                MoveForward = intent;
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


    }
}
