using LastToTheGlobe.Scripts.Camera;
using LastToTheGlobe.Scripts.Environment.Planets;
using UnityEngine;

//Auteur : Abdallah
//Modification : Attika

namespace LastToTheGlobe.Scripts.Avatar
{
    public class ThirdPersonController : Avatar
    {
        [SerializeField] private AttractedScript attractedScript;

        [Header("Character Exposer")] 
        public CharacterExposer playerExposer;
        
        [Header("Camera Parameters")]
        public CameraScript myCamera;
        [SerializeField]
        private float rotationSpeed = 5.0f;

        [Header("Movement Parameters")] 
        private Quaternion _rotation;
        private Vector3 _moveDir;
        private Vector3 _jumpDir;
        public float speed;
        private bool _isJumping = false;
        public Rigidbody rb;
   
        [Header("Orb References")]
        public GameObject orb;
        public GameObject orbSpawned;
        private bool _canThrowSpell;

        [Header("Inputs")] 
        public KeyCode offensiveOrbInput;
        public KeyCode jumpInput;

        private void FixedUpdate () 
        {
            //Get inputs from the player
            _moveDir = new Vector3(Input.GetAxisRaw("Horizontal"),
                0,
                Input.GetAxisRaw("Vertical")).normalized;
        
        
            rb.MovePosition(rb.position + transform.TransformDirection(_moveDir) * speed * Time.deltaTime);
        
            //Rotate the character so the camera can follow
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

            //Detects the input to throw an offensiveOrb
            if (Input.GetKeyDown(offensiveOrbInput) && _canThrowSpell)
            {
                _canThrowSpell = false;
                orb.SetActive(true);
            }

            //Detects the current status of the offensive orb to update if necessary
            if (!orb.activeSelf)
            {
                _canThrowSpell = true;
            }
            
            //Apply a stronger force to jump
            if (!Input.GetKey(jumpInput) || _isJumping) return;
            _jumpDir = attractor.dirForce;
            rb.AddForce(_jumpDir * 250);

            //To avoid double jump
            _isJumping = true;
        }
        
        //Get jump again
        private void OnCollisionEnter(Collision hit)
        {
            if (!hit.gameObject.CompareTag("Planet")) return;
            _isJumping = false;
            attractedScript.isGrounded = false;
        }
    }
}
