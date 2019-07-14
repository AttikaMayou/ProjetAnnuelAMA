using System.Collections.Generic;
using LastToTheGlobe.Scripts.Management;
using Photon.Pun;
using UnityEngine;

namespace LastToTheGlobe.Scripts.Chest
{
    public class ChestsPhotonViewSender : MonoBehaviour
    {
        // Start is called before the first frame update

        public bool debug;
        //public List<GameObject> itemSlot;
    
        #region RPC Callbacks

        

        [PunRPC]
        void AssignChestRPC(int chestId, int playerId)
        {
            

            if (debug) Debug.Log("[ChestsPhotonViewSender] AssignChestRPC received");
            
            //Fin exposers from int parameters (IDs)
            var chest = ColliderDirectoryScript.Instance.GetChestExposer(chestId);
            var player = ColliderDirectoryScript.Instance.GetCharacterExposer(playerId);

           
            
            if (!player || !chest) return;

            if (debug)
            {
                Debug.LogFormat("[ChestsPhotonViewSender] Found the player {0} from this ID : {1}",player.name, playerId);
                Debug.LogFormat("[ChestsPhotonViewSender] Found the chest {0} from this ID : {1}",chest.name, chestId);
            }
            
            var _chestSeed = Random.Range(0,255);            
            //Set the chest which is ACTUALLY near player
            player.chest = chest.ChestScript;
            chest.seedChest = _chestSeed;
            player.interaction.enabled = true;
            
            if (debug) Debug.LogFormat("Ce joueur : {0} possède la seed suivante : {1}", playerId, _chestSeed);
            
        }

        [PunRPC]
        void UnassignChestRPC(int playerId)
        {
            if (debug) Debug.Log("[ChestsPhotonViewSender] UnassignChestRPC received");
            
            //Find exposer from int parameter (ID)
            var player = ColliderDirectoryScript.Instance.GetCharacterExposer(playerId);
            
            if (!player) return;
            
            if (debug)
            {
                Debug.LogFormat("[ChestsPhotonViewSender] Found the player {0} from this ID : {1}",player.name, playerId);
            }

            //Set the chest to null since the player isn't ACTUALLY near any chest
            player.chest = null;
            player.interaction.enabled = false;

        }
    
    

        #endregion
    }
}
