using Assets.LastToTheGlobe.Scripts.Avatar;
using LastToTheGlobe.Scripts.Avatar;
using LastToTheGlobe.Scripts.Singleton;
using UnityEngine;

//Auteur : Abdallah
//Modification : Attika

namespace LastToTheGlobe.Scripts.Camera
{
    public class CameraScript : MonoBehaviour {
        
//        [SerializeField] private string playerTag = "Player";
//        [SerializeField] private float cameraOffsetOriginalX = -47f;
//        [SerializeField] private float cameraOffsetOriginalY = 124f;
//        [SerializeField] private float cameraOffsetOriginalZ = 11f;
        private Vector3 _cameraOffsetOriginal;

        private Transform _myTransform;
        
        [Header("Local Player References")]
        public CharacterExposerScript playerExposer;
        public GameObject targetPlayer;

        public bool startFollowing;

        private void Awake ()
        {
            //Get this gameObject's transform
            _myTransform = this.gameObject.transform;
        }

        public void InitializeCameraPosition()
        {
            //cameraOffset = Distance between camera and player
            if (!targetPlayer || !playerExposer)
            {
                Debug.LogError("No targetPlayer to follow !");
                return;
            }
            
            //Initialize position
            var position = targetPlayer.transform.position;
            Debug.Log(position);
            //var x = position.x + 3.0f;
            var y = position.y + 1.0f;
            var z = position.z - 5.3f;
            _cameraOffsetOriginal = position - new Vector3(position.x, y, z);
            
            //Initializing rotation
            _myTransform.rotation = targetPlayer.transform.rotation * playerExposer.cameraRotatorX.transform.rotation;
        }

        private void FixedUpdate()
        {
            if(startFollowing)
                UpdatePosAndRot();
        }
        
        /// <summary>
        /// This function is used to update the position and the rotation of the camera according to the player's one
        /// </summary>
        private void UpdatePosAndRot()
        {
            if (!targetPlayer || !playerExposer) return;
            //Update the player's position each frame
            var position = targetPlayer.transform.position;
            _myTransform.position = position;
            _myTransform.rotation = targetPlayer.transform.rotation * playerExposer.cameraRotatorX.transform.rotation;
            _myTransform.position -= _myTransform.rotation * _cameraOffsetOriginal; 
        }

        //Dunno what this function will serve to... But here it is
//        private void ResetCamPosition()
//        {
//            var originPos = new Vector3(cameraOffsetOriginalX, 
//                                        cameraOffsetOriginalY,
//                                        cameraOffsetOriginalZ);
//            _myTransform.position = originPos;
//        }
    }
}
