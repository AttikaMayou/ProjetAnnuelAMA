using LastToTheGlobe.Scripts.Environment.Planets;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Serialization;

//Auteur: Margot, Abdallah et Attika

namespace LastToTheGlobe.Scripts.Avatar
{
    public class CharacterExposer : MonoBehaviour
    {
        //Parameters used to control character
        public Rigidbody playerRb;
        public PhotonRigidbodyView characterRbView;
        public Transform characterTransform;
        public GameObject avatarRootGameObject;
        public ThirdPersonController thirdPersonController;
        public CharacterTrampolineScript characterTrampolineScript;
        public AttractedScript selfPlayerAttractedScript;
        public AttractedScript selfOrbAttractedScript;
        public Collider playerCollider;
    }
}
