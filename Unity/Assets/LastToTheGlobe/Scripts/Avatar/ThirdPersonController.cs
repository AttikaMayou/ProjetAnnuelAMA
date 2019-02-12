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
   
        [Header("Orb Objects")]
        public GameObject orb;
        public GameObject orbSpawned;

        private void FixedUpdate () 
        {
            //Gestion des déplacements autour de la planète
            _moveDir = new Vector3(Input.GetAxisRaw("Horizontal"),
                0,
                Input.GetAxisRaw("Vertical")).normalized;
        
        
            rb.MovePosition(rb.position + transform.TransformDirection(_moveDir) * speed * Time.deltaTime);
        
        
            //Permet de tourner le personnage pour donner la direction à la caméra sur l'axe horizontale
            transform.Rotate(new Vector3(0,
                Input.GetAxis("Mouse X") * rotationSpeed,
                0));
        
        
            //Permet de tourner le gameobject qui donne la direction à la caméra sur l'axe vertical
            playerExposer.cameraRotatorX.transform.Rotate(new Vector3(-(Input.GetAxis("Mouse Y") * rotationSpeed),
                0,
                0), Space.Self);
        
        
            //Les deux conditions juste en dessous permettent à la caméra de ne pas aller trop haut ni trop bas
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
        
            //Gestion du saut en appliquant force plus èlevé que la gravité.
            if(Input.GetKey(KeyCode.Space) && !_isJumping)
            {

                _jumpDir = attractor.dirForce;
                rb.AddForce(_jumpDir * 250);

                //Interdiction du saut
                _isJumping = true;
            }
        }
        
        //Autorisation du saut
        private void OnCollisionEnter(Collision hit)
        {
            if (!hit.gameObject.CompareTag("planet")) return;
            _isJumping = false;
            attractedScript.isGrounded = false;
        }
    }
}
