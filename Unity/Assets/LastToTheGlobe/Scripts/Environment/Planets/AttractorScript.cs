using LastToTheGlobe.Scripts.Avatar;
using LastToTheGlobe.Scripts.Management;
using UnityEngine;
using UnityEngine.Serialization;


//Auteur : Abdallah
//Modification : Attika

namespace LastToTheGlobe.Scripts.Environment.Planets
{
    public class AttractorScript : PlanetExposerScript
    {
        public bool debug = true;
        
        public float speedRotation = 10f;
        public Vector3 dirForce;
        private AvatarExposerScript _currentAvatar;
        
        public void Attractor(Rigidbody attractedRb, Transform body, float gravity)
        {
            //Give the direction of gravity
            var gravityUp = (body.position - transform.position).normalized;
            var bodyUp = body.up;

            attractedRb.AddForce(gravityUp * gravity);

            //Sync the vertical axe's player (up) with the gravity direction chosen before=
            var rotation = body.rotation;
            var targetRotation = Quaternion.FromToRotation(bodyUp, gravityUp) * rotation;
            rotation = Quaternion.Slerp(rotation, targetRotation, speedRotation * Time.deltaTime);
            body.rotation = rotation;
            dirForce = gravityUp;
        }

        private void OnTriggerEnter(Collider coll)
        {
            if (!coll.CompareTag("Player")) return;
            if(debug) Debug.Log("there is a player who entered");
            
            var exposer = PlayerColliderDirectoryScript.Instance.GetExposer(coll);
            if (!exposer) return;

            //exposer.thirdPersonController.attractor = this;
            exposer.selfPlayerAttractedScript.attractor = this;
            exposer.selfOrbAttractedScript.attractor = this;
        }

        private void OnTriggerExit(Collider coll)
        {
            if (!coll.CompareTag("Player")) return;
            
            var exposer = PlayerColliderDirectoryScript.Instance.GetExposer(coll);
            if (!exposer) return;
            
            //exposer.thirdPersonController.attractor = null;
            exposer.selfPlayerAttractedScript.attractor = null;
            exposer.selfOrbAttractedScript.attractor = null;
        }
    }
}
