using System.Collections;
using Assets.LastToTheGlobe.Scripts.Management;
using Assets.LastToTheGlobe.Scripts.Management.OLD;
using LastToTheGlobe.Scripts.Management;
using Photon.Pun;
using UnityEngine;

//Auteur : Attika

namespace Assets.LastToTheGlobe.Scripts.Environment.Planets
{
    public class AttractorScript : MonoBehaviour
    {
        public bool debug = true;

        public PlanetExposerScript Exposer;

        public float speedRotation = 10f;
        public Vector3 DirForce;

        [SerializeField] private PhotonView photonView;
        
        public void AttractPlayer(int playerId, float gravity)
        {
            //Only the MasterClient interact with collider and stuff like this
            if (!PhotonNetwork.IsMasterClient) return;
            
            var player = ColliderDirectoryScript.Instance.GetCharacterExposer(playerId);
            var attractedRb = player.CharacterRb;
            var body = player.CharacterTr;
            
            //Give the direction of gravity
            var gravityUp = (body.position - Exposer.PlanetTransform.position).normalized;
            var bodyUp = body.up;
          
            attractedRb.AddForce(gravityUp * gravity);
          
            //Sync the vertical axe's player (up) with the gravity direction chosen before
            var rotation = body.rotation;
            var targetRotation = Quaternion.FromToRotation(bodyUp, gravityUp) * rotation;
            rotation = Quaternion.Slerp(rotation, targetRotation, speedRotation * Time.deltaTime);
            body.rotation = rotation;
            DirForce = gravityUp;
        }
        

        #region Private Methods

        //The planet detects a collider entered its attraction field 
        private void OnTriggerEnter(Collider other)
        {
            if(debug) Debug.LogFormat("[AttractorScript] {0} get triggered by something : {1}",
                this.gameObject.name , other.gameObject.name);
            
            //Only the MasterClient interact with collider and stuff like this
            if(!PhotonNetwork.IsMasterClient) return;
            
            var playerId = ColliderDirectoryScript.Instance.GetPlayerId(other);
            
            //if playerId is different from -1, that means this is a player which hit the planet
            if (playerId != -1) 
            {
                //Send the MasterClient a message to warn him with its own ID and playerId
                photonView.RPC("DetectPlayerRPC", RpcTarget.MasterClient,
                    Exposer.Id, playerId);
            }
        }
        
        //The planet detects a collider exit its attraction field 
        private void OnTriggerExit(Collider other)
        {
            if(debug) Debug.LogFormat("[AttractorScript] {1} left {0}",
                this.gameObject.name , other.gameObject.name);
            
            //Only the MasterClient interact with collider and stuff like this
            if (!PhotonNetwork.IsMasterClient) return;

            var playerId = ColliderDirectoryScript.Instance.GetPlayerId(other);
            
            //if playerId is different from -1, that means this is a player which left the planet
            if (playerId != -1) 
            {
                //Send the MasterClient a message to warn him with playerId
                photonView.RPC("RemoveAttractorPlayerRPC", RpcTarget.MasterClient, 
                    playerId);
            }
        }
        
        #endregion

        #region RPC Callbacks
        
        [PunRPC]
        void DetectPlayerRPC(int planetId, int playerId)
        {
            if(debug) Debug.Log("[AttractorScript] DetectPlayerRPC received");
            
            //Find exposers from int parameters (IDs)
            var planet = ColliderDirectoryScript.Instance.GetPlanetExposer(planetId);
            var player = ColliderDirectoryScript.Instance.GetCharacterExposer(playerId);

            if (!planet || !player) return;
            
            if (debug)
            {
                Debug.Log("Found the player " + player.name + " from this ID : " + playerId);
                Debug.Log("Found the planet " + planet.name + " from this ID : " + planetId);
            }
            
            //Set the attractor script which ACTUALLY attract player
            player.Attractor = planet.AttractorScript;
        }

        [PunRPC]
        void RemoveAttractorPlayerRPC(int playerId)
        {
            if(debug) Debug.Log("[AttractorScript] RemoveAttractorPlayerRPC received");
            
            //Find exposer from int parameter (ID)
            var player = ColliderDirectoryScript.Instance.GetCharacterExposer(playerId);

            if (!player) return;
            
            if (debug)
            {
                Debug.Log("Found the player " + player.name + " from this ID : " + playerId);
            }

            //Set the attractor to null since the player isn't ACTUALLY attracted by anything
            player.Attractor = null;
        }
        
        #endregion
    }
}
