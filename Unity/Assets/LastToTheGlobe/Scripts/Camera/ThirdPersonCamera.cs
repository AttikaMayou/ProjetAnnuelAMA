using UnityEngine;
using UnityEngine.Serialization;

//Auteur : Abdallah

namespace LastToTheGlobe.Scripts.Camera
{
    public class ThirdPersonCamera : MonoBehaviour {

        public Transform playerTransform;

        private Vector3 _cameraOffset;

        [FormerlySerializedAs("SmoothFactor")] [Range(0.01f, 1.0f)]
        public float smoothFactor = 0.5f;

        public float distanceToPlayer = 5.0f;

        [FormerlySerializedAs("RotationSpeed")] public float rotationSpeed = 5.0f;

        private Vector3 _newPos;

        private void Start()
        {
            _cameraOffset = transform.position;
        }

        private void LateUpdate()
        {
            var camTurnAngleX = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * rotationSpeed, playerTransform.up);
            _cameraOffset = camTurnAngleX * _cameraOffset;
            transform.position = Vector3.Slerp(transform.position, _cameraOffset, smoothFactor);
            transform.LookAt(playerTransform);


        }

    }
}
