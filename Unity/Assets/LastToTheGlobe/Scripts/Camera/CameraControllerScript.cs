using LastToTheGlobe.Scripts.Avatar;
using UnityEngine;
using UnityEngine.Serialization;

//Auteur : Abdallah
//Modification : Attika

namespace LastToTheGlobe.Scripts.Camera
{
    public class CameraControllerScript : MonoBehaviour
    {
        public bool debug = true;
        
        private Vector3 _cameraOffsetOriginal;

        private Transform _myTr;

        [FormerlySerializedAs("_yAdd")]
        [Header("Balance Parameters")] 
        [SerializeField] private float yAdd;
        //Suggested value : 1
        [FormerlySerializedAs("_zAdd")] [SerializeField] private float zAdd;
        //Suggested value : -5.3
        
        [FormerlySerializedAs("PlayerExposer")] [Header("Local Player References")] 
        public CharacterExposerScript playerExposer;

        [FormerlySerializedAs("StartFollowing")] public bool startFollowing;

        private void Awake()
        {
            _myTr = this.gameObject.transform;
        }

        public void InitializeCameraPosition()
        {
            if(debug) Debug.Log("[CameraControllerScript] Initialization of the camera");
            //cameraOffset = Distance between the camera and player
            if (!playerExposer)
            {
                Debug.LogError("[CameraControllerScript] No target player to follow");
                return;
            }
            
            var position = playerExposer.CharacterTr.position;
            var y = position.y + yAdd;
            var z = position.z + zAdd;
            _cameraOffsetOriginal = position - new Vector3(position.x, y, z);

            _myTr.rotation = playerExposer.CameraRotatorX.transform.rotation;
        }

        private void FixedUpdate()
        {
            if (startFollowing)
            {
                UpdatePosAndRot();
            }
        }
        
        private void UpdatePosAndRot()
        {
            if (!playerExposer) return;

            var position = playerExposer.CharacterTr.position;
            //transform.forward = playerExposer.characterCollider.transform.forward;
            
            _myTr.rotation = playerExposer.CameraRotatorX.transform.rotation;
            position -= _myTr.rotation * _cameraOffsetOriginal;
            _myTr.position = position;
        }
    }
}
