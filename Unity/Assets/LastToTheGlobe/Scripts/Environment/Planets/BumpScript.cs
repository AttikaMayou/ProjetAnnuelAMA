﻿using Assets.LastToTheGlobe.Scripts.Management;
using Photon.Pun;
using UnityEngine;

namespace Assets.LastToTheGlobe.Scripts.Environment.Planets
{
    public class BumpScript : MonoBehaviour
    {
        public static bool Debug = true;

        public BumperExposerScript Exposer;

        public void BumpPlayer(int bumperId, int playerId, float force)
        {
            if (!PhotonNetwork.IsMasterClient) return;

            if (bumperId != Exposer.Id)
            {
                if(Debug) UnityEngine.Debug.LogWarningFormat("[BumpScript] Bumper {0} received order to bump player {1} but it was meant to {2}",
                    Exposer.Id, playerId, bumperId);
                return;
            }
            
            var player = ColliderDirectoryScript.Instance.GetCharacterExposer(playerId);
            var bumpedRb = player.CharacterRb;
            
            bumpedRb.AddForce(Exposer.BumperTransform.up * force);
        }
        
        #region Collision Methods

        private void OnTriggerEnter(Collider other)
        {
            if(Debug) UnityEngine.Debug.LogFormat("[BumpScript] {0} get triggered by something : {1}",
                this.gameObject.name, other.gameObject.name);
            
            //Only the Master Client interact with collider and stuff like this
            if (!PhotonNetwork.IsMasterClient) return;

            var playerId = ColliderDirectoryScript.Instance.GetPlayerId(other);

            //if playerId is different from -1, that means this is a player which hit the bumper
            if (playerId != -1)
            {
                //Send to MasterClient a message to warn him with its own ID and playerId
                Exposer.BumpersPhotonView.RPC("AssignBumperRPC", RpcTarget.MasterClient,
                    Exposer.Id, playerId);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if(Debug) UnityEngine.Debug.LogFormat("[BumpScript] {1} left {0}",
                this.gameObject.name, other.gameObject.name);
             
            //Only the Master Client interact with collider and stuff like this
            if (!PhotonNetwork.IsMasterClient) return;

            var playerId = ColliderDirectoryScript.Instance.GetPlayerId(other);

            //if playerId is different from -1, that means this is a player which left the bumper aera
            if (playerId != -1)
            {
                //Send to MasterClient a message to warn him with its own ID and playerId
                Exposer.BumpersPhotonView.RPC("UnassignBumperRPC", RpcTarget.MasterClient,
                    playerId);
            }
        }

        #endregion
        
    }
}
