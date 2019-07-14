using System;
using System.Collections;
using LastToTheGlobe.Scripts.Management;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Serialization;

//Auteur : Attika

namespace LastToTheGlobe.Scripts.Environment.Planets
{
    public class AttractorScript : MonoBehaviour
    {
        public static bool Debug = false;

        public PlanetExposerScript exposer;

        private int _i = 0;

        public void AttractPlayer(int planetId, int playerId, float gravity)
        {
            //Only the MasterClient interact with collider and stuff like this
            if (!PhotonNetwork.IsMasterClient) return;

            if (planetId != exposer.id)
            {
                if(Debug) UnityEngine.Debug.LogWarningFormat("[AttractorScript] Planet {0} received order to attract player {1} but it was meant to planet {2}",
                    exposer.id, playerId, planetId);
                return;
            }
            
            var player = ColliderDirectoryScript.Instance.GetCharacterExposer(playerId);
            var attractedRb = player.CharacterRb;
            var body = player.CharacterTr;

            var planet = ColliderDirectoryScript.Instance.GetPlanetExposer(planetId);
            
            //Give the direction of gravity
            var gravityUp = (body.position - planet.planetTransform.position).normalized;
            var bodyUp = body.up;
          
            attractedRb.AddForce(gravityUp * gravity);
          
            //Sync the vertical axe's player (up) with the gravity direction chosen before
            var rotation = body.rotation;
            var targetRotation = Quaternion.FromToRotation(bodyUp, gravityUp) * rotation;
            rotation = Quaternion.Slerp(rotation, targetRotation, GameVariablesScript.Instance.speedPlanetRotation * Time.deltaTime);
            body.rotation = rotation;
        }
        
        #region Collision Methods

        //The planet detects a collider entered its attraction field 
        private void OnTriggerEnter(Collider other)
        {
            if (_i == 1)
            {
                return;
            }
            _i = 1;
            
            if(Debug) UnityEngine.Debug.LogFormat("[AttractorScript] {0} get triggered by something : {1}",
                this.gameObject.name , other.gameObject.name);
            
            //Only the MasterClient interact with collider and stuff like this
            if(!PhotonNetwork.IsMasterClient) return;
            
            var playerId = ColliderDirectoryScript.Instance.GetPlayerId(other);

            //if playerId is different from -1, that means this is a player which hit the planet
            if (playerId != -1) 
            {
                //Send to MasterClient a message to warn him with its own ID and playerId
                exposer.planetsPhotonView.RPC("DetectPlayerRPC", RpcTarget.MasterClient,
                    exposer.id, playerId);
            }

            StartCoroutine(ResetTrigger());
        }
        
        //The planet detects a collider exit its attraction field 
        private void OnTriggerExit(Collider other)
        {
            if (_i == 1)
            {
                return;
            }
            _i = 1;
            
            if(Debug) UnityEngine.Debug.LogFormat("[AttractorScript] {1} left {0}",
                this.gameObject.name , other.gameObject.name);
            
            //Only the MasterClient interact with collider and stuff like this
            if (!PhotonNetwork.IsMasterClient) return;

            var playerId = ColliderDirectoryScript.Instance.GetPlayerId(other);
            
            //if playerId is different from -1, that means this is a player which left the planet
            if (playerId != -1) 
            {
                //Send to MasterClient a message to warn him with playerId
                exposer.planetsPhotonView.RPC("RemoveAttractorPlayerRPC", RpcTarget.MasterClient, 
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
