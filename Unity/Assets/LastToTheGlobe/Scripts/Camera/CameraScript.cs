using LastToTheGlobe.Scripts.Avatar;
using UnityEngine;


//Auteur : Abdallah et Attika

namespace LastToTheGlobe.Scripts.Camera
{
    public class CameraScript : MonoBehaviour {
        [SerializeField]private string playerTag = "Player";
        [SerializeField] private float cameraOffsetOriginalX = -47f;
        [SerializeField] private float cameraOffsetOriginalY = 124f;
        [SerializeField] private float cameraOffsetOriginalZ = 11f;
        private Vector3 _cameraOffsetOriginal;

        private Transform _myTransform;
        
        public CharacterExposer playerExposer;
        
        [SerializeField]
        private GameObject cameraRotatorX;

        private void Start ()
        {
            //Get this gameObject's transform
            _myTransform = this.gameObject.transform;
            //cameraOffset = Distance entre la caméra et le joueur
            var position = playerExposer.characterTransform.position;
            var y = position.y + 3.0f;
            var z = position.z - 9.0f;
            _cameraOffsetOriginal = position - new Vector3(position.x, y, z);
        }

        private void Update()
        {
              UpdatePosAndRot();
        }

        /// <summary>
        /// This function is used to update the position and the rotation of the camera
        /// </summary>
        private void UpdatePosAndRot()
        {
            //Récupération de la position du joueur à chaque frame
            var position = playerExposer.characterTransform.position;
            _myTransform.rotation = playerExposer.characterTransform.rotation * cameraRotatorX.transform.rotation;
            _myTransform.position = position - (_myTransform.rotation * _cameraOffsetOriginal); 
        }
    }
}
