using Assets.LastToTheGlobe.Scripts.Avatar;
using LastToTheGlobe.Scripts.Avatar;
using UnityEngine;

//Auteur : Abdallah
//Modification : Attika

namespace Assets.LastToTheGlobe.Scripts.Camera
{
    public class CameraControllerScript : MonoBehaviour
    {
        public bool debug = true;
        
        private Vector3 _cameraOffsetOriginal;

        private Transform _myTr;

        [Header("Balance Parameters")] 
        [SerializeField] private float yAdd;
        [SerializeField] private float zAdd;
        
        [Header("Local Player References")] 
        public CharacterExposerScript playerExposer;

        public bool startFollowing;

        private void Awake()
        {
            _myTr = this.gameObject.transform;
        }

        public void InitializeCameraPosition()
        {
            if(debug) Debug.Log("initialization of the camera");
            //cameraOffset = Distance between the camera and player
            if (!playerExposer)
            {
                Debug.LogError("No target player to follow");
                return;
            }
            
            var position = playerExposer.characterTr.position;
            var y = position.y + yAdd;
            var z = position.z + zAdd;
            _cameraOffsetOriginal = position - new Vector3(position.x, y, z);

            _myTr.rotation = playerExposer.cameraRotatorX.transform.rotation;
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

            var position = playerExposer.characterTr.position;
            //transform.forward = playerExposer.characterCollider.transform.forward;
            
            _myTr.rotation = playerExposer.cameraRotatorX.transform.rotation;
            position -= _myTr.rotation * _cameraOffsetOriginal;
            _myTr.position = position;
        }
    }
}
