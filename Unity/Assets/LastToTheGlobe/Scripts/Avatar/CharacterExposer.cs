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
        public Rigidbody rb;
        [FormerlySerializedAs("CharacterRbView")] public PhotonRigidbodyView characterRbView;
        [FormerlySerializedAs("CharacterTransform")] public Transform characterTransform;
        [FormerlySerializedAs("AvatarRootGameObject")] public GameObject avatarRootGameObject;
        [FormerlySerializedAs("ThirdPersonController")] public ThirdPersonController thirdPersonController;
        [FormerlySerializedAs("CharacterTrampolineScript")] public characterTrampolineScript characterTrampolineScript;
        [FormerlySerializedAs("SelfPlayerAttractedScript")] public AttractedScript selfPlayerAttractedScript;
        [FormerlySerializedAs("SelfOrbAttractedScript")] public AttractedScript selfOrbAttractedScript;
        public Collider Collider;
    }
}
