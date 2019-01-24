using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Auteur : Margot

public class CharacterControllerRigidbody : MonoBehaviour
{

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

    private float Forward;
    private float Strafe;
    private float Speed = 5;
    private bool CanDoubleJump = true;
    private float ray;



    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }


    void Update()
    {
        // Récupération des floats vertical et horizontal de l'animator au script
        Forward = Input.GetAxis("Vertical");
        Strafe = Input.GetAxis("Horizontal");

        Vector3 movement = new Vector3(Strafe, 0.0f, Forward);
    }

    
    void FixedUpdate()
    {
        //Course
        if (Input.GetKey(KeyCode.LeftShift))
        {
            Speed = runSpeed;
        }
        else
        {
            Speed = walkSpeed;
        }

        //Dash
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            Speed = DashSpeed;
        }

        rb.velocity = new Vector3(Strafe * Speed, rb.velocity.y, Forward * Speed);


        //Saut
        if (Input.GetKey(KeyCode.Space))
        {
            if(IsGrounded())
            {
                rb.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
                CanDoubleJump = true;
                //rb.velocity = new Vector3(Strafe * Speed, 0, Forward * Speed);
            }
            //Double saut
            else
            {
                if(CanDoubleJump == true)
                {
                    //rb.velocity = new Vector3(Strafe * Speed, 0, Forward * Speed);
                    rb.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
                    CanDoubleJump = false;
                }
            }
        }

    }

    //Check si le character est au sol
    private bool IsGrounded()
    {
        return Physics.Raycast(raycastOrigin.position, Vector3.down, ray);
    }





}



