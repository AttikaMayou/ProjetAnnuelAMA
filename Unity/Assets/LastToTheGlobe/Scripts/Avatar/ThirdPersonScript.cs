using LastToTheGlobe.Scripts.Camera;
using UnityEngine;


//Auteur : Attika
//Modifications : Abdallah

namespace LastToTheGlobe.Scripts.Avatar
{
    public class ThirdPersonScript : MonoBehaviour
    {
        [SerializeField] private CharacterExposerScript playerExposer;
        [Header("Camera Parameters")]
        public CameraScript myCamera;
        [SerializeField]
        private float rotationSpeed = 5.0f;
        private Quaternion _rotation;

        private void FixedUpdate()
        {
            transform.Rotate(new Vector3(0,
                Input.GetAxis("Mouse X") * rotationSpeed,
                0));
        
        
            //Rotate cameraRotatorX to give the camera the right vertical axe
            playerExposer.cameraRotatorX.transform.Rotate(new Vector3(-(Input.GetAxis("Mouse Y") * rotationSpeed),
                0,
                0), Space.Self);
        
        
            //Prevent the camera from going too high or too low
            //Les valeurs qui étaient mises sont des valeurs qui peuvent être pris par la variable cameraRotatorX.transform.rotation.x (-1 - 1)
            if (playerExposer.cameraRotatorX.transform.rotation.x >= 0.42f)
            {
                _rotation = 
                    new Quaternion(0.42f, _rotation.y, 
                        _rotation.z, _rotation.w);
                playerExposer.cameraRotatorX.transform.rotation = _rotation;
            }
            
            if (playerExposer.cameraRotatorX.transform.rotation.x <= -0.2f)
            {
                _rotation = 
                    new Quaternion(-0.2f, _rotation.y, 
                        _rotation.z, _rotation.w);
                playerExposer.cameraRotatorX.transform.rotation = _rotation;
            }
        }
    }
}
