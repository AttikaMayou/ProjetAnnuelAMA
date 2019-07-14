using Assets.LastToTheGlobe.Scripts.Management;
using LastToTheGlobe.Scripts.Management;
using Photon.Pun;
using UnityEngine;

namespace LastToTheGlobe.Scripts.Environment.Planets
{
    public class BumpersPhotonViewSender : MonoBehaviour
    {
        public bool debug;
        
        #region RPC Callbacks

        [PunRPC]
        void AssignBumperRPC(int bumperId, int playerId)
        {
            if (debug) Debug.Log("[BumpersPhotonViewSender] AssignBumpRPC received");
            
            //Fin exposers from int parameters (IDs)
            var bumper = ColliderDirectoryScript.Instance.GetBumperExposer(bumperId);
            var player = ColliderDirectoryScript.Instance.GetCharacterExposer(playerId);

            if (!player || !bumper) return;

            if (debug)
            {
                Debug.LogFormat("[BumpersPhotonViewSender] Found the player {0} from this ID : {1}",player.name, playerId);
                Debug.LogFormat("[BumpersPhotonViewSender] Found the bumper {0} from this ID : {1}",bumper.name, bumperId);
            }
            
            //Set the bumper which is ACTUALLY near player
            player.bumper = bumper.BumpScript;
        }

        [PunRPC]
        void UnassignBumperRPC(int playerId)
        {
            if (debug) Debug.Log("[BumpersPhotonViewSender] UnassignBumperRPC received");
            
            //Find exposer from int parameter (ID)
            var player = ColliderDirectoryScript.Instance.GetCharacterExposer(playerId);
            
            if (!player) return;
            
            if (debug)
            {
                Debug.LogFormat("[BumpersPhotonViewSender] Found the player {0} from this ID : {1}",player.name, playerId);
            }

            //Set the bumper to null since the player isn't ACTUALLY near any bumper
            player.bumper = null;
        }

        #endregion
    }
}
