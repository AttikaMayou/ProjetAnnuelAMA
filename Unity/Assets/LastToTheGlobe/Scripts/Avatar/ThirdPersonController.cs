using LastToTheGlobe.Scripts.Camera;
using LastToTheGlobe.Scripts.Environment.Planets;
using UnityEngine;

//Auteur : Abdallah
//Modification : Attika, Margot

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
        [SerializeField]
        [Tooltip("Rigidbody du player")]
        public Rigidbody rb;
        [SerializeField]
        [Tooltip("Vitesse de la marche")]
        private float speed;
        [SerializeField]
        [Tooltip("Vitesse de la course")]
        private float runSpeed;
        [SerializeField]
        [Tooltip("Vitesse du saut")]
        private float jumpSpeed;
        [SerializeField]
        [Tooltip("Vitesse du dash")]
        private float dashSpeed;
        private bool _isJumping = false;
        private float _forward;
        private float _strafe;
        private Quaternion _rotation;
        private Vector3 _jumpDir;
        private int _jumpMax = 1;
        private bool _dashAsked = false;

        [Header("Orb Objects")]
        public GameObject orb;
        public GameObject orbSpawned;

            private void FixedUpdate () 
        {
            // Récupération des floats vertical et horizontal de l'animator au script
            _forward = Input.GetAxis("Vertical");
            _strafe = Input.GetAxis("Horizontal");
            Vector3 _moveDir = new Vector3(_strafe, 0.0f, _forward);

            rb.MovePosition(rb.position + transform.TransformDirection(_moveDir) * speed * Time.deltaTime);
            Running();
            //Dash
            if (_dashAsked)
            {
                rb.MovePosition(rb.position + transform.TransformDirection(_moveDir) * dashSpeed * Time.deltaTime);
                _dashAsked = false;
            }
            //Déplacement
            else
            {
                rb.MovePosition(rb.position + transform.TransformDirection(_moveDir) * speed * Time.deltaTime);
            }

            //Rotate the character so the camera can follow
            transform.Rotate(new Vector3(0,
                Input.GetAxis("Mouse X") * rotationSpeed,
                0));
        
        
            //Rotate cameraRotatorX to give the camera the right vertical axe
            playerExposer.cameraRotatorX.transform.Rotate(new Vector3(-(Input.GetAxis("Mouse Y") * rotationSpeed),
                0,
                0), Space.Self);
        
        
            //Prevent the camera from going too high or too low
            //Les valeurs qui était mise sont des valeurs qui peuvent être pris par la variable cameraRotatorX.transform.rotation.x (-1 - 1)
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
        
            //Apply a stronger force to jump
            if (!Input.GetKey(KeyCode.Space) || _isJumping) return;
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

        private void Running()
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                speed = runSpeed;
            }
            else
            {
                speed = speed;
            }
        }

        private void Dash()
        {
            if (Input.GetKeyDown(KeyCode.LeftAlt))
            {
                _dashAsked = true;
            }
        }

        private void Jump()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (_jumpMax > 0)
                {
                    rb.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
                    _jumpMax--;
                }
            }
            if (IsGrounded())
            {
                jumpMax = 1;
            }
        }
    }
}
