using System.Collections;
using LastToTheGlobe.Scripts.Management;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Serialization;

namespace LastToTheGlobe.Scripts.Environment.Planets
{
    public class BumpScript : MonoBehaviour
    {
        public static bool Debug = false;

        [FormerlySerializedAs("Exposer")] public BumperExposerScript exposer;

        private int _i = 0;
        
        public void BumpPlayer(int bumperId, int playerId, float force)
        {
            if (!PhotonNetwork.IsMasterClient) return;

            if (bumperId != exposer.Id)
            {
                if(Debug) UnityEngine.Debug.LogWarningFormat("[BumpScript] Bumper {0} received order to bump player {1} but it was meant to {2}",
                    exposer.Id, playerId, bumperId);
                return;
            }
            
            var player = ColliderDirectoryScript.Instance.GetCharacterExposer(playerId);
            var bumpedRb = player.CharacterRb;
            //bumpedRb.AddForce(exposer.BumperTransform.up * force);
            player.CharacterRb.MovePosition(exposer.BumperTransform.up * force);
        }
        
        #region Collision Methods

        private void OnTriggerEnter(Collider other)
        {
            if (_i == 1)
            {
                return;
            }
            _i = 1;

            if(Debug) UnityEngine.Debug.LogFormat("[BumpScript] {0} get triggered by something : {1}",
                this.gameObject.name, other.gameObject.name);
            
            //Only the Master Client interact with collider and stuff like this
            if (!PhotonNetwork.IsMasterClient) return;

            var playerId = ColliderDirectoryScript.Instance.GetPlayerId(other);

            //if playerId is different from -1, that means this is a player which hit the bumper
            if (playerId != -1)
            {
                //Send to MasterClient a message to warn him with its own ID and playerId
                exposer.BumpersPhotonView.RPC("AssignBumperRPC", RpcTarget.MasterClient,
                    exposer.Id, playerId);
            }

            StartCoroutine(ResetTrigger());
        }

        private void OnTriggerExit(Collider other)
        {
            if (_i == 1)
            {
                return;
            }
            _i = 1;
            
            if(Debug) UnityEngine.Debug.LogFormat("[BumpScript] {1} left {0}",
                this.gameObject.name, other.gameObject.name);
             
            //Only the Master Client interact with collider and stuff like this
            if (!PhotonNetwork.IsMasterClient) return;

            var playerId = ColliderDirectoryScript.Instance.GetPlayerId(other);

            //if playerId is different from -1, that means this is a player which left the bumper aera
            if (playerId != -1)
            {
                //Send to MasterClient a message to warn him with its own ID and playerId
                exposer.BumpersPhotonView.RPC("UnassignBumperRPC", RpcTarget.MasterClient,
                    playerId);
            }
            
            StartCoroutine(ResetTrigger());
        }

        private IEnumerator ResetTrigger()
        {
            yield return new WaitForSeconds(0.2f);
            _i = 0;
        }
        
        #endregion
        
    }
}
