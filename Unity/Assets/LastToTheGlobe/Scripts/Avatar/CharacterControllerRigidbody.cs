using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;

//Auteur : Margot

namespace LastToTheGlobe.Scripts.Dev
{
    public class CharacterControllerRigidbody : MonoBehaviour
    {
        [SerializeField]
        private Camera PlayerCam;

        [SerializeField]
        [Tooltip("Character")]
        private Rigidbody rb;

        [SerializeField]
        [Tooltip("Vitesse de la course")]
        private float runSpeed;

        [SerializeField]
        [Tooltip("Vitesse de la marche")]
        private float walkSpeed;

        [SerializeField]
        [Tooltip("Vitesse du saut")]
        private float jumpSpeed;

        [SerializeField]
        [Tooltip("Vitesse du dash")]
        private float DashSpeed;

        [SerializeField]
        [Tooltip("Raycast position")]
        private Transform raycastOrigin;

        private float forward;
        private float strafe;
        private float speed = 5;
        private float ray = 0.4f;
        private bool isGrounded = false;
        private int jumpMax = 1;
        private bool dashAsked = false;

        void Awake()
        {
        }

        void Start()
        {
            rb = GetComponent<Rigidbody>();
        }


        void Update()
        {
            // Récupération des floats vertical et horizontal de l'animator au script
            forward = Input.GetAxis("Vertical");
            strafe = Input.GetAxis("Horizontal");
            Vector3 movement = new Vector3(strafe, 0.0f, forward);

            //Course
            Running();

            //Dash
            Dash();

            //Saut et double saut
            Jump();
        }


        void FixedUpdate()
        {
            //Dash
            if (dashAsked)
            {
                rb.velocity = new Vector3(strafe * DashSpeed, rb.velocity.y, forward * DashSpeed);
                dashAsked = false;
            }
            //Déplacement
            else
            {
                rb.velocity = new Vector3(strafe * speed, rb.velocity.y, forward * speed);
            }
        }

        //Check si le character est au sol
        private bool IsGrounded()
        {
            return Physics.Raycast(raycastOrigin.position, Vector3.down, ray);
        }

        //Course
        private void Running()
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                speed = runSpeed;
            }
            else
            {
                speed = walkSpeed;
            }
        }

        //Dash
        private void Dash()
        {
            if (Input.GetKeyDown(KeyCode.LeftAlt))
            {
                dashAsked = true;
            }
        }

        //Saut et double saut
        private void Jump()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (jumpMax > 0)
                {
                    rb.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
                    jumpMax--;
                }
            }
            if (IsGrounded())
            {
                jumpMax = 1;
            }
        }


/*

        [PunRPC]
        private void ActivateAvatarRPC(int avatarId)
        {
            avatars[avatarId].AvatarRootGameObject.SetActive(true);
        }

        [PunRPC]
        private void DeactivateAvatarRPC(int avatarId)
        {
            avatars[avatarId].AvatarRootGameObject.SetActive(false);
        }
        */
    }
}



