using Assets.LastToTheGlobe.Scripts.Management;
using Photon.Pun;
using UnityEngine;

//Auteur : Attika

namespace Assets.LastToTheGlobe.Scripts.Environment.Planets
{
    public class AttractorScript : MonoBehaviour
    {
        public static bool Debug = true;

        public PlanetExposerScript Exposer;

        public float speedRotation = 10f;
        public Vector3 DirForce;
        
        public void AttractPlayer(int planetId, int playerId, float gravity)
        {
            //Only the MasterClient interact with collider and stuff like this
            if (!PhotonNetwork.IsMasterClient) return;

            if (planetId != Exposer.Id)
            {
                if(Debug) UnityEngine.Debug.LogWarningFormat("[AttractorScript] Planet {0} received order to attract player {1} but it was meant to {2}",
                    Exposer.Id, playerId, planetId);
                return;
            }
            
            var player = ColliderDirectoryScript.Instance.GetCharacterExposer(playerId);
            var attractedRb = player.CharacterRb;
            var body = player.CharacterTr;

            var planet = ColliderDirectoryScript.Instance.GetPlanetExposer(planetId);
            
            //Give the direction of gravity
            var gravityUp = (body.position - planet.PlanetTransform.position).normalized;
            var bodyUp = body.up;
          
            attractedRb.AddForce(gravityUp * gravity);
          
            //Sync the vertical axe's player (up) with the gravity direction chosen before
            var rotation = body.rotation;
            var targetRotation = Quaternion.FromToRotation(bodyUp, gravityUp) * rotation;
            rotation = Quaternion.Slerp(rotation, targetRotation, speedRotation * Time.deltaTime);
            body.rotation = rotation;
            DirForce = gravityUp;
        }
        
        #region Collision Methods

        //The planet detects a collider entered its attraction field 
        private void OnTriggerEnter(Collider other)
        {
            if(Debug) UnityEngine.Debug.LogFormat("[AttractorScript] {0} get triggered by something : {1}",
                this.gameObject.name , other.gameObject.name);
            
            //Only the MasterClient interact with collider and stuff like this
            if(!PhotonNetwork.IsMasterClient) return;
            
            var playerId = ColliderDirectoryScript.Instance.GetPlayerId(other);
            
            //if playerId is different from -1, that means this is a player which hit the planet
            if (playerId != -1) 
            {
                //Send to MasterClient a message to warn him with its own ID and playerId
                Exposer.PlanetsPhotonView.RPC("DetectPlayerRPC", RpcTarget.MasterClient,
                    Exposer.Id, playerId);
            }
        }
        
        //The planet detects a collider exit its attraction field 
        private void OnTriggerExit(Collider other)
        {
            if(Debug) UnityEngine.Debug.LogFormat("[AttractorScript] {1} left {0}",
                this.gameObject.name , other.gameObject.name);
            
            //Only the MasterClient interact with collider and stuff like this
            if (!PhotonNetwork.IsMasterClient) return;

            var playerId = ColliderDirectoryScript.Instance.GetPlayerId(other);
            
            //if playerId is different from -1, that means this is a player which left the planet
            if (playerId != -1) 
            {
                //Send to MasterClient a message to warn him with playerId
                Exposer.PlanetsPhotonView.RPC("RemoveAttractorPlayerRPC", RpcTarget.MasterClient, 
                    playerId);
            }
        }
        
        #endregion
    }
}
