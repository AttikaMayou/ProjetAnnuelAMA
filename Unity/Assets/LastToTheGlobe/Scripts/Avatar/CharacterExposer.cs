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
            if (!characterLocalPhotonView.IsMine && PhotonNetwork.IsConnected) return;
            Debug.Log("Awake of the CharacterExposer : " + gameObject.name);
            PlayerColliderDirectoryScript.Instance.AddExposer(this);
            AvatarsController.Instance.camInScene.playerExposer = this;
            AvatarsController.Instance.camInScene.InitializeCameraPosition();
            AvatarsController.Instance.camInScene.startFollowing = true;
            thirdPersonController.myCamera = AvatarsController.Instance.camInScene;
            
            //The prefab should not be destroyed when switching scene (Lobby to GameRoom for example)
            DontDestroyOnLoad(this.avatarRootGameObject);
        }
    }
}
