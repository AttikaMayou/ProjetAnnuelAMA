using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


//Auteur: Margot

namespace LastToTheGlobe.Scripts.Dev
{
    public class CharacterExposer : MonoBehaviour
    {
        //Parameters used to control character
        public Rigidbody rb;
        public PhotonRigidbodyView CharacterRbView;
        //public Transform CharacterTransform;
        public GameObject AvatarRootGameObject;
        public ThirdPersonController ThirdPersonController;
        public characterTrampolineScript CharacterTrampolineScript;
        public AttractedScript SelfPlayerAttractedScript;
        public AttractedScript SelfOrbAttractedScript;
        public Collider Collider;
    }
}
