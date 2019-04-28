using LastToTheGlobe.Scripts.Avatar;
using LastToTheGlobe.Scripts.Management;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Serialization;


//Auteur : Abdallah
//Modification : Attika

namespace LastToTheGlobe.Scripts.Environment.Planets
{
    public class AttractorScript : MonoBehaviour
    {
        public bool debug = true;
        
        public float speedRotation = 10f;
        public Vector3 dirForce;
        
        public void Attractor(Rigidbody attractedRb, Transform body, float gravity)
        {
            if (!PhotonNetwork.IsMasterClient) return;
            
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

        private void OnTriggerEnter(Collider coll)
        {
            if (debug)
            {
                Debug.Log("there is a player or an orb who entered");
            }

            if (coll.CompareTag("Player"))
            {
                var exposer = ColliderDirectoryScript.Instance.GetCharacterExposer(coll);
                exposer.attractor = this;
            }

            if (coll.CompareTag("Bullet"))
            {
                //TODO : add attractor to bullet object
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
    }
}
