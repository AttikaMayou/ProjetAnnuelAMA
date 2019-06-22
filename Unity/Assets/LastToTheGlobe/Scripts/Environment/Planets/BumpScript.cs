using Assets.LastToTheGlobe.Scripts.Management;
using Photon.Pun;
using UnityEngine;

namespace Assets.LastToTheGlobe.Scripts.Environment.Planets
{
    public class BumpScript : MonoBehaviour
    {
        public static bool debug = true;

        public BumperExposerScript Exposer;

        [SerializeField] private PhotonView photonView;

        public void BumpPlayer(int playerId)
        {
            
        }
        
        #region Collision Methods

        private void OnTriggerEnter(Collider other)
        {
            if(debug) Debug.LogFormat("[BumpScript] {0} get triggered by something : {1}",
                this.gameObject.name, other.gameObject.name);
            
            //Only the Master Client interact with collider and stuff like this
            if (!PhotonNetwork.IsMasterClient) return;

            var playerId = ColliderDirectoryScript.Instance.GetPlayerId(other);

            //if playerId is different from -1, that means this is a player which hit the bumper
            if (playerId != -1)
            {
                //Send to MasterClient a message to warn him with its own ID and playerId
                photonView.RPC("AssignBumperRPC", RpcTarget.MasterClient,
                    Exposer.Id, playerId);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if(debug) Debug.LogFormat("[BumpScript] {1} left {0}",
                this.gameObject.name, other.gameObject.name);
             
            //Only the Master Client interact with collider and stuff like this
            if (!PhotonNetwork.IsMasterClient) return;

            var playerId = ColliderDirectoryScript.Instance.GetPlayerId(other);

            //if playerId is different from -1, that means this is a player which left the bumper aera
            if (playerId != -1)
            {
                //Send to MasterClient a message to warn him with its own ID and playerId
                photonView.RPC("UnassignBumperRPC", RpcTarget.MasterClient,
                    playerId);
            }
        }

        #endregion
        
        #region RPC Callbacks

        [PunRPC]
        void AssignBumperRPC(int bumperId, int playerId)
        {
            if (debug) Debug.Log("[BumpScript] AssignBumpRPC received");
            
            //Fin exposers from int parameters (IDs)
            var bumper = ColliderDirectoryScript.Instance.GetBumperExposer(bumperId);
            var player = ColliderDirectoryScript.Instance.GetCharacterExposer(playerId);

            if (!player || !bumper) return;

            if (debug)
            {
                Debug.LogFormat("[BumpScript] Found the player {0} from this ID : {1}",player.name, playerId);
                Debug.LogFormat("[BumpScript] Found the bumper {0} from this ID : {1}",bumper.name, bumperId);
            }
            
            //Set the bumper which is ACTUALLY near player
            player.Bumper = bumper.BumpScript;
        }

        [PunRPC]
        void UnassignBumperRPC(int playerId)
        {
            if (debug) Debug.Log("[BumpScript] UnassignBumperRPC received");
            
            //Find exposer from int parameter (ID)
            var player = ColliderDirectoryScript.Instance.GetCharacterExposer(playerId);
            
            if (!player) return;
            
            if (debug)
            {
                Debug.LogFormat("[BumpScript] Found the player {0} from this ID : {1}",player.name, playerId);
            }

            //Set the bumper to null since the player isn't ACTUALLY near any bumper
            player.Bumper = null;
        }

        #endregion
    }
}
