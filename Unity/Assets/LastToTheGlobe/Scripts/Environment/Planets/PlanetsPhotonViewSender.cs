using Assets.LastToTheGlobe.Scripts.Management;
using LastToTheGlobe.Scripts.Management;
using Photon.Pun;
using UnityEngine;

//Auteur : Attika

namespace LastToTheGlobe.Scripts.Environment.Planets
{
    public class PlanetsPhotonViewSender : MonoBehaviour
    {
        public bool debug;
        
        #region RPC Callbacks
        
        [PunRPC]
        public void DetectPlayerRPC(int planetId, int playerId)
        {
            if(debug) Debug.Log("[PlanetsPhotonViewSender] DetectPlayerRPC received");
            
            //Find exposers from int parameters (IDs)
            var planet = ColliderDirectoryScript.Instance.GetPlanetExposer(planetId);
            var player = ColliderDirectoryScript.Instance.GetCharacterExposer(playerId);

            if (!planet || !player) return;
            
            if (debug)
            {
                Debug.LogFormat("[PlanetsPhotonViewSender] Found the player {0} from this ID : {1}",player.name, playerId);
                Debug.LogFormat("[PlanetsPhotonViewSender] Found the planet {0} from this ID : {1}",planet.name, planetId);
            }
            
            player.DisableGravity();
            
            //Set the attractor script which ACTUALLY attract player
            player.Attractor = planet.attractorScript;
        }

        [PunRPC]
        public void RemoveAttractorPlayerRPC(int playerId)
        {
            if(debug) Debug.Log("[PlanetsPhotonViewSender] RemoveAttractorPlayerRPC received");
            
            //Find exposer from int parameter (ID)
            var player = ColliderDirectoryScript.Instance.GetCharacterExposer(playerId);

            if (!player) return;
            
            if (debug)
            {
                Debug.LogFormat("[PlanetsPhotonViewSender] Found the player {0} from this ID : {1}",player.name, playerId);
            }
            
            player.EnableGravity();
            
            //Set the attractor to null since the player isn't ACTUALLY attracted by anything
            player.Attractor = null;
        }
        
        #endregion
    }
}
