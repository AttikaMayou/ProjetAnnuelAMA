using System.Collections;
using Assets.LastToTheGlobe.Scripts.Management;
using Assets.LastToTheGlobe.Scripts.Management.OLD;
using LastToTheGlobe.Scripts.Management;
using Photon.Pun;
using UnityEngine;

//Auteur : Abdallah
//Modification : Attika

namespace Assets.LastToTheGlobe.Scripts.Environment.Planets
{
    public class AttractorScript : MonoBehaviour
    {
        public bool debug = true;

        public float speedRotation = 10f;
        public Vector3 DirForce;

        [SerializeField] private PhotonView photonView;
        
        public void Attractor(int playerId, float gravity)
        {
            photonView.RPC("AttractObject", RpcTarget.MasterClient, playerId, gravity);
        }

        private void OnTriggerEnter(Collider coll)
        {
            if (!ColliderDirectoryScript.Instance.IsInitialized)
            {
                if (debug) Debug.Log("wait before trying to attract players - lobby situation");
                StartCoroutine(DelayOnTriggerEnter(2.0f));
            }
            if (debug) Debug.LogFormat("ON TRIGGER ENTER - {0}", coll.gameObject.name);
            
            if (!coll.CompareTag("Player") /*&& !coll.CompareTag("Bullet")*/) return;
            
            if (debug) Debug.Log("there is a player or an orb who entered");
            
            var exposer = ColliderDirectoryScript.Instance.GetCharacterExposer(coll);
            if (!exposer)
            {
                Debug.Log("Couldn't find player/orb in the Directory");
            }
            else
            {
                Debug.LogFormat("Find the exposer : {0}", exposer);
                exposer.Attractor = this;
            }
        }

        private IEnumerator DelayOnTriggerEnter(float time)
        {
            yield return new WaitForSeconds(time);
        }

        private void OnTriggerExit(Collider coll)
        {
            if (coll.CompareTag("Player"))
            {
                var exposer = ColliderDirectoryScript.Instance.GetCharacterExposer(coll);
                //exposer.attractor = null;
            }
            
            if (coll.CompareTag("Bullet"))
            {
                //TODO : add null to attractor of the bullet object
            }
        }

        [PunRPC]
        void AttractObject(int playerId, float gravity)
        {
            var player = ColliderDirectoryScript.Instance.CharacterExposers[playerId];
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
        void SetAttractor(int planetId, int playerId)
        {
            var player = ColliderDirectoryScript.Instance.CharacterExposers[playerId];
            var planet = ColliderDirectoryScript.Instance.PlanetExposers[planetId];
            if (debug)
            {
                Debug.Log("Find the player " + player.name + " from this ID : " + playerId);
                Debug.Log("Find the planet " + planet.name + " from this ID : " + planetId);
            }

            player.Attractor = planet.AttractorScript;
        }
    }
}
