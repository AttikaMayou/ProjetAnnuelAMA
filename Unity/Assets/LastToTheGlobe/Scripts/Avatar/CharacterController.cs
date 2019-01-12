using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Auteur : Margot


namespace LastToTheGlobe.Scripts.Dev
{
    public class CharacterController: MonoBehaviour
    {
        private CharacterController controller;

        [SerializeField]
        [Tooltip("Vitesse de la course")]
        private float runSpeed;

        [SerializeField]
        [Tooltip("Vitesse de la marche")]
        private float WalkSpeed;

        [SerializeField]
        [Tooltip("Gravité")]
        public float gravity = 20.0F;

        [SerializeField]
        [Tooltip("Vitesse du saut")]
        public float JumpSpeed;

        private Animator animator;
        // Deux floats pour faire avancer : Horizontal/Forward = walk devant derriere / Vertical/Strafe = walk gauche droite
        private float Forward;
        private float Strafe;
        private Vector3 movement = Vector3.zero;
        private float Speed= 1;

        //CAMERA
        [SerializeField]
        private GameObject cameraRotatorX;

        [SerializeField]
        [Tooltip("Vitesse de la camera")]
        private float rotationSpeed = 3;

        void Start()
        {

            controller = GetComponent<CharacterController>();
            animator = GetComponentInChildren<Animator>();

            // character au sol
            gameObject.transform.position = new Vector3(0, 0, 0);
        }

        void Update()
        {   
            //CAMERA
            transform.Rotate(new Vector3(0, Input.GetAxis("Mouse X") * rotationSpeed, 0));
            cameraRotatorX.transform.Rotate(new Vector3(-(Input.GetAxis("Mouse Y") * rotationSpeed), 0, 0), Space.Self);

            // Récupération des floats vertical et horizontal de l'animator au script
            Forward = Input.GetAxis("Vertical");
            Strafe = Input.GetAxis("Horizontal");

            if (controller.isGrounded)
            {
                //Set movement
                movement = new Vector3(Strafe, 0.0f, Forward);
                movement = transform.TransformDirection(movement);

                //Course
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    Speed = runSpeed; 

                }
                else
                {
                    Speed = WalkSpeed;
                }                    
                
                //Saut
                if (Input.GetKey(KeyCode.Space))
                {
                    movement.y = JumpSpeed;
                }
                movement = movement * Speed;
               
            }       
            //Ajout de la gravité
            movement.y = movement.y - (gravity * Time.deltaTime);
            //Move player with DeltaTime
            controller.Move(movement * Time.deltaTime);
        }
    }
}
