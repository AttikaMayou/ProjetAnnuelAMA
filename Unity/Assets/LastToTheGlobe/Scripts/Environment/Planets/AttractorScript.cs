using Assets.LastToTheGlobe.Scripts.Management;
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
        public Vector3 dirForce;

        [SerializeField] private PhotonView photonView;
        
        public void Attractor(int playerId, float gravity)
        {
            photonView.RPC("AttractObject", RpcTarget.MasterClient, playerId, gravity);
        }

        private void OnTriggerEnter(Collider coll)
        {
            Debug.Log("ON TRIGGER ENTER" + coll.gameObject.name);
            if (!coll.CompareTag("Player") && !coll.CompareTag("Bullet")) return;
            Debug.Log("ON TRIGGER ENTER POUET" + coll.gameObject.name);
            if (debug)
            {
                Debug.Log("there is a player or an orb who entered");
            }
            
            Debug.Log("ON TRIGGER ENTER PUOIT" + coll.gameObject.name);
            var exposer = PlayerColliderDirectoryScript.Instance.GetExposer(coll);
            if (!exposer)
            {
                Debug.Log("couldn't find player/orb in the Directory");
            }
            else
            {
                Debug.Log(exposer);
                exposer.attractor = this;
            }
        }

        private void OnTriggerExit(Collider coll)
        {
            if (coll.CompareTag("Player"))
            {
                var exposer = ColliderDirectoryScript.Instance.GetCharacterExposer(coll);
                exposer.attractor = null;
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
            var attractedRb = player.characterRb;
            var body = player.characterTr;
            //Give the direction of gravity
            var gravityUp = (body.position - transform.position).normalized;
            var bodyUp = body.up;

            attractedRb.AddForce(gravityUp * gravity);

            //Sync the vertical axe's player (up) with the gravity direction chosen before
            var rotation = body.rotation;
            var targetRotation = Quaternion.FromToRotation(bodyUp, gravityUp) * rotation;
            rotation = Quaternion.Slerp(rotation, targetRotation, speedRotation * Time.deltaTime);
            body.rotation = rotation;
            dirForce = gravityUp;
        }
    }
}
