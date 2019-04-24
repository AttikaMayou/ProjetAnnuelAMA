using System.Collections;
using LastToTheGlobe.Scripts.Camera;
using LastToTheGlobe.Scripts.Dev;
using LastToTheGlobe.Scripts.Environment.Planets;
using LastToTheGlobe.Scripts.Inventory;
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
        //public CharacterTrampolineScript characterTrampolineScript;
        public Trampoline bumperScript;
        public AttractedScript selfPlayerAttractedScript;
        public AttractedScript selfOrbAttractedScript;
        public Collider characterCollider;
        public InventoryScript inventoryScript;
        
        //Character Parameters
        public Vector3 _movedir;
        public float dashSpeed = 30;
        
        //Reference itself to the ColliderDirectory and CameraScript when it spawns 
        private void Awake()
        {
            //Only the master add the players to directory
//            if (PhotonNetwork.IsMasterClient)
//            {
                Debug.Log("Awake of the CharacterExposer : " + gameObject.name);
                //PlayerColliderDirectoryScript.Instance.AddExposer(this);
                //StartCoroutine(AvatarsController.Instance.WaitBeforeSyncData(this));
//            }

            if (!characterLocalPhotonView.IsMine && PhotonNetwork.IsConnected) return;
            //Wait for the camera to have the player reference before initializing follow behaviour
            //StartCoroutine(InitializeCameraReferences());
            
            //The prefab should not be destroyed when switching scene (Lobby to GameRoom for example)
            DontDestroyOnLoad(this.avatarRootGameObject);
        }

//        private IEnumerator InitializeCameraReferences()
//        {
//            yield return new WaitForSeconds(1.0f);
//            AvatarsController.Instance.camInScene.targetPlayer = this.gameObject;
//            AvatarsController.Instance.camInScene.playerExposer = this;
//            AvatarsController.Instance.camInScene.InitializeCameraPosition();
//            AvatarsController.Instance.camInScene.startFollowing = true;
//           //thirdPersonController.myCamera = AvatarsController.Instance.camInScene;
//        }
    }
}
