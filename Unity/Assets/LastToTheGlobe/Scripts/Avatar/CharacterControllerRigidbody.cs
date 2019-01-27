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
    private float ray = 0.4f;
    private bool isGrounded = false;
    private int JumpMax= 1;
    private bool DashAsked = false;


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
            DashAsked = true;
        }

        //Saut et double saut
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (JumpMax > 0)
            {
                rb.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
                JumpMax--;
            }
        }
        if (IsGrounded())
        {
            JumpMax = 1;
        }
    }

    
    void FixedUpdate()
    {
        //Dash
        if (DashAsked)
        {
            rb.velocity = new Vector3(Strafe * DashSpeed, rb.velocity.y, Forward * DashSpeed);
            DashAsked = false;
        }
        //Déplacement
        else
        {
            rb.velocity = new Vector3(Strafe * Speed, rb.velocity.y, Forward * Speed);
        }
    }

    //Check si le character est au sol
    private bool IsGrounded()
    { 
        return Physics.Raycast(raycastOrigin.position, Vector3.down, ray);
    }





}



