using LastToTheGlobe.Scripts.Avatar;
using LastToTheGlobe.Scripts.Singleton;
using UnityEngine;

//Auteur : Abdallah
//Modification : Attika

namespace LastToTheGlobe.Scripts.Camera
{
    public class CameraScript : MonoBehaviourSingleton<CameraScript> {
        
        [SerializeField] private string playerTag = "Player";
        [SerializeField] private float cameraOffsetOriginalX = -47f;
        [SerializeField] private float cameraOffsetOriginalY = 124f;
        [SerializeField] private float cameraOffsetOriginalZ = 11f;
        private Vector3 _cameraOffsetOriginal;

        private Transform _myTransform;
        
        [Header("Local Player References")]
        public CharacterExposer playerExposer;

        public bool startFollowing;

        private void Awake ()
        {
            //Get this gameObject's transform
            _myTransform = this.gameObject.transform;
        }

        public void InitializeCameraPosition()
        {
            //cameraOffset = Distance between camera and player
            if (!playerExposer) return;
            var position = playerExposer.characterTransform.position;
            var y = position.y + 3.0f;
            var z = position.z - 9.0f;
            _cameraOffsetOriginal = position - new Vector3(position.x, y, z);
        }

        private void Update()
        {
            if(startFollowing)
                UpdatePosAndRot();
        }
        
        /// <summary>
        /// This function is used to update the position and the rotation of the camera according to the player's one
        /// </summary>
        private void UpdatePosAndRot()
        {
            if (!playerExposer) return;
            //Update the player's position each frame
            var position = playerExposer.characterTransform.position;
            _myTransform.rotation = playerExposer.characterTransform.rotation * playerExposer.cameraRotatorX.transform.rotation;
            _myTransform.position = position - (_myTransform.rotation * _cameraOffsetOriginal); 
        }

        //Dunno what this function will serve to... But here it is
        private void ResetCamPosition()
        {
            var originPos = new Vector3(cameraOffsetOriginalX, 
                                        cameraOffsetOriginalY,
                                        cameraOffsetOriginalZ);
            _myTransform.position = originPos;
        }
    }
}
