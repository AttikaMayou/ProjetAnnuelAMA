using LastToTheGlobe.Scripts.Camera;
using LastToTheGlobe.Scripts.Dev;
using LastToTheGlobe.Scripts.Environment.Planets;
using LastToTheGlobe.Scripts.Management;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

//Auteur: Margot, Abdallah et Attika

namespace LastToTheGlobe.Scripts.Avatar
{
    public class CharacterExposer : MonoBehaviour
    {
        //Parameters used to control character
        public Rigidbody characterRb;
        public PhotonView characterLocalPhotonView;
        public PhotonTransformView characterTrView;
        public Transform characterTransform;
        public GameObject avatarRootGameObject;
        public GameObject cameraRotatorX;
        public ThirdPersonController thirdPersonController;
        public CharacterTrampolineScript characterTrampolineScript;
        public AttractedScript selfPlayerAttractedScript;
        public AttractedScript selfOrbAttractedScript;
        public Collider characterCollider;

        //Reference itself to the ColliderDirectory and CameraScript when it spawns 
        private void Awake()
        {
            Debug.Log("Awake of the CharacterExposer : " + gameObject.name);
            PlayerColliderDirectoryScript.Instance.AddExposer(this);
            //if (!characterLocalPhotonView.IsMine) return;
            AvatarsController.Instance.camInScene.playerExposer = this;
            AvatarsController.Instance.camInScene.InitializeCameraPosition();
            AvatarsController.Instance.camInScene.startFollowing = true;
            thirdPersonController.myCamera = AvatarsController.Instance.camInScene;

            if (characterLocalPhotonView.IsMine)
            {
                AvatarsController.LocalPlayerInstance = this.gameObject;
            }
            DontDestroyOnLoad(this.avatarRootGameObject);
        }
    }
}
