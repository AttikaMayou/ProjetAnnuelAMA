using LastToTheGlobe.Scripts.Avatar;
using UnityEngine;
using UnityEngine.Serialization;


//Auteur : Abdallah
//Modification : Attika

namespace LastToTheGlobe.Scripts.Environment.Planets
{
    public class AttractorScript : PlanetExposerScript {

        public float speedRotation = 10f;
        public Vector3 dirForce;
        private AvatarExposerScript currentAvatar;
        [SerializeField] private PlayerColliderDirectoryScript playerColliderDirectoryScript;

        public void Attractor(Rigidbody attractedRb, Transform body, float gravity)
        {
            //Donne la direction de la gravité
            var gravityUp = (body.position - transform.position).normalized;
            var bodyUp = body.up;

            attractedRb.AddForce(gravityUp * gravity);

            //Synchronise l'axe vertical du joueur (up) avec l'axe gravité choisi
            var rotation = body.rotation;
            Quaternion targetRotation = Quaternion.FromToRotation(bodyUp, gravityUp) * rotation;
            rotation = Quaternion.Slerp(rotation, targetRotation, speedRotation * Time.deltaTime);
            body.rotation = rotation;
            dirForce = gravityUp;
        }

        private void OnTriggerEnter(Collider coll)
        {
            if (!coll.CompareTag("Player")) return;
            
            var exposer = playerColliderDirectoryScript.GetExposer(coll);

            exposer.thirdPersonController.attractor = this;
            exposer.characterTrampolineScript.attractor = this;
            exposer.selfPlayerAttractedScript.attractor = this;
            exposer.selfOrbAttractedScript.attractor = this;
        }

        private void OnTriggerExit(Collider coll)
        {
            if (!coll.CompareTag("Player")) return;
            
            var exposer = playerColliderDirectoryScript.GetExposer(coll);

            exposer.thirdPersonController.attractor = null;
            exposer.characterTrampolineScript.attractor = null;
            exposer.selfPlayerAttractedScript.attractor = null;
            exposer.selfOrbAttractedScript.attractor = null;
        }
    }
}
