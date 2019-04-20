using LastToTheGlobe.Scripts.Camera;
using LastToTheGlobe.Scripts.Dev;
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
        [SerializeField]
        [Tooltip("Rigidbody du player")]
        public Rigidbody rb;
        [SerializeField]
        [Tooltip("Vitesse de la marche")]
        private float speed;
        [SerializeField]
        [Tooltip("Vitesse de la course")]
        private float runSpeed = 8;
        [SerializeField]
        [Tooltip("Vitesse du saut")]
        private float jumpSpeed = 5;
        [SerializeField]
        [Tooltip("Vitesse du dash")]
        private float dashSpeed = 30;
        private bool _isJumping = false;
        private float _forward;
        private float _strafe;
        private Quaternion _rotation;
        private Vector3 _jumpDir;
        private int _jumpMax = 1;
        private bool _dashAsked = false;
        private bool _runAsked = false;

        [Header("Orb References")]
        public GameObject orb;
        public GameObject orbSpawned;
        private bool _canThrowSpell;

        [Header("Inputs")] 
        public KeyCode offensiveOrbInput;
        public KeyCode jumpInput;
        public KeyCode runInput;
        public KeyCode dashInput;

        private void FixedUpdate () 
        {
            // Récupération des floats vertical et horizontal de l'animator au script
            _forward = Input.GetAxis("Vertical");
            _strafe = Input.GetAxis("Horizontal");
            Vector3 _moveDir = new Vector3(_strafe, 0.0f, _forward);

            Running();
            if (_runAsked == true)
            {
                speed = runSpeed;
                _runAsked = false;
            }
            else
            {
                rb.MovePosition(rb.position + transform.TransformDirection(_moveDir) * speed * Time.deltaTime);
            }

            //Dash
            if (_dashAsked)
            {
                rb.MovePosition(rb.position + transform.TransformDirection(_moveDir) * dashSpeed * Time.deltaTime);
                _dashAsked = false;
                Debug.Log("DashAsked");
            }
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
            //Les valeurs qui étaient mises sont des valeurs qui peuvent être pris par la variable cameraRotatorX.transform.rotation.x (-1 - 1)
            /*if (playerExposer.cameraRotatorX.transform.rotation.x >= 0.42f)
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
            }*/

            //Detects the input to throw an offensiveOrb
            if (Input.GetKeyDown(offensiveOrbInput) && _canThrowSpell && playerExposer.characterLocalPhotonView.IsMine)
            {
                _canThrowSpell = false;
                //orb.SetActive(true);
                //AvatarsController.Instance.LaunchBullet(playerExposer);
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
            if (!hit.gameObject.CompareTag("Planet"))
            {
                _isJumping = false;
                attractedScript.isGrounded = false;
            }
            if (hit.gameObject.CompareTag("Item"))
            {
                print("hello");
            }
            
        }

        private void Running()
        {
            if (Input.GetKey(runInput))
            {
                _runAsked = true;
            }
        }

        private void Dash()
        {
            if (Input.GetKeyDown(dashInput))
            {
                _dashAsked = true;
            }
        }

/*
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
        }*/
    }
}
