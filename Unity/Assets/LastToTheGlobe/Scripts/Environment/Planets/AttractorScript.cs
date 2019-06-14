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
        
        public void Attractor(int playerId, float gravity)
        {
            photonView.RPC("AttractObject", RpcTarget.MasterClient,
                playerId, gravity);
        }

        /*private void OnTriggerEnter(Collider coll)
        {
            //Only the MasterClient interact with collider and stuff like this
            if (!PhotonNetwork.IsMasterClient) return;

            //Get the ID's of the player and planet with their respective collider
            var playerId = ColliderDirectoryScript.Instance.GetPlayerId(coll);
            var planetId = ColliderDirectoryScript.Instance.GetPlanetId(Exposer.PlanetCollider);
            
            //If the ID are different from -1 (means that the exposers are enabled),
            //we call the function 'SetAttractor'
            if(planetId != -1 && playerId != -1)
                photonView.RPC("SetAttractor", RpcTarget.MasterClient,
                    playerId, planetId);
        }*/
        
        #region Private Methods

        //The planet detects a collider entered its attraction field 
        private void OnTriggerEnter(Collider other)
        {
            if(debug) Debug.LogFormat("[AttractorScript] {0} get triggered by something : {1}",
                this.gameObject.name , other.gameObject.name);
            if(!PhotonNetwork.IsMasterClient) return;
            var playerId = ColliderDirectoryScript.Instance.GetPlayerId(other);
            //if playerId is different from -1, that means this is a player which hit the planet
            if (playerId != -1) 
            {
                //Send the MasterClient a message to warn him with its own ID and the playerId
                photonView.RPC("DetectPlayerRPC", RpcTarget.MasterClient,
                    Exposer.Id, playerId);
            }
        }
        
        #endregion

        /*private void OnTriggerExit(Collider coll)
        {
            //Only the MasterClient interact with collider and stuff like this
            if (!PhotonNetwork.IsMasterClient) return;
            
            //Get ID of the player with his collider
            var playerId = ColliderDirectoryScript.Instance.GetPlayerId(coll);
            
            //If ID is different from -1 (means that the exposer is enabled),
            //we call the function 'RemoveAttractor'
            if(playerId != -1)
                photonView.RPC("RemoveAttractor", RpcTarget.MasterClient,
                    playerId);
        }*/
        
        #region RPC Callbacks
        
        [PunRPC]
        void DetectPlayerRPC(int planetId, int playerId)
        {
            if(debug) Debug.Log("[AttractorScript] DetectPlayerRPC received");
            
            //Find the exposers from the int parameters (IDs)
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

        /*[PunRPC]
        void AttractObject(int playerId, float gravity)
        {
            var player = ColliderDirectoryScript.Instance.GetCharacterExposer(playerId);
            var attractedRb = player.CharacterRb;
            var body = player.CharacterTr;
            //Give the direction of gravity
            var gravityUp = (body.position - transform.position).normalized;
            var bodyUp = body.up;
          
            attractedRb.AddForce(gravityUp * gravity);
          
            //Sync the vertical axe's player (up) with the gravity direction chosen before
            var rotation = body.rotation;
            var targetRotation = Quaternion.FromToRotation(bodyUp, gravityUp) * rotation;
            rotation = Quaternion.Slerp(rotation, targetRotation, speedRotation * Time.deltaTime);
            body.rotation = rotation;
            DirForce = gravityUp;
        }
        
        [PunRPC]
        void SetAttractor(int playerId, int planetId)
        {
            //Find the exposers from the int parameters (IDs)
            var player = ColliderDirectoryScript.Instance.GetCharacterExposer(playerId);
            var planet = ColliderDirectoryScript.Instance.GetPlanetExposer(planetId);
            
            if (debug)
            {
                Debug.Log("Find the player " + player.name + " from this ID : " + playerId);
                Debug.Log("Find the planet " + planet.name + " from this ID : " + planetId);
            }

            //Set the attractor script which ACTUALLY attract player
            player.Attractor = planet.AttractorScript;
        }

        [PunRPC]
        void RemoveAttractor(int playerId)
        {
            //Find the exposer from the int parameter (ID)
            var player = ColliderDirectoryScript.Instance.GetCharacterExposer(playerId);
            
            if (debug)
            {
                Debug.Log("Find the player " + player.name + " from this ID : " + playerId);
            }

            //Set the attractor to null since the player isn't ACTUALLY attracted by anything
            player.Attractor = null;
        }*/
        
        #endregion
    }
}
