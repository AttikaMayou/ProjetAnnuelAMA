using Assets.LastToTheGlobe.Scripts.Avatar;
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
        [SerializeField] private float _yAdd;
        [SerializeField] private float _zAdd;
        
        [Header("Local Player References")] 
        public CharacterExposerScript PlayerExposer;

        public bool StartFollowing;

        private void Awake()
        {
            _myTr = this.gameObject.transform;
        }

        public void InitializeCameraPosition()
        {
            if(debug) Debug.Log("initialization of the camera");
            //cameraOffset = Distance between the camera and player
            if (!PlayerExposer)
            {
                Debug.LogError("No target player to follow");
                return;
            }
            
            var position = PlayerExposer.CharacterTr.position;
            var y = position.y + _yAdd;
            var z = position.z + _zAdd;
            _cameraOffsetOriginal = position - new Vector3(position.x, y, z);

            _myTr.rotation = PlayerExposer.CameraRotatorX.transform.rotation;
        }

        private void FixedUpdate()
        {
            if (StartFollowing)
            {
                UpdatePosAndRot();
            }
        }
        
        private void UpdatePosAndRot()
        {
            if (!PlayerExposer) return;

            var position = PlayerExposer.CharacterTr.position;
            //transform.forward = playerExposer.characterCollider.transform.forward;
            
            _myTr.rotation = PlayerExposer.CameraRotatorX.transform.rotation;
            position -= _myTr.rotation * _cameraOffsetOriginal;
            _myTr.position = position;
        }
    }
}
