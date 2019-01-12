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

    private float Forward;
    private float Strafe;
    private float Speed = 5;

    [SerializeField]
    [Tooltip("Raycast position")]
    private Transform raycastOrigin;

    public float ray;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void FixedUpdate()
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

        rb.velocity = new Vector3(Strafe * Speed, rb.velocity.y, Forward * Speed);

        //Saut
        if (Input.GetKey(KeyCode.Space) && IsGrounded())
        {
            rb.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
        }

    }

    //Check si le character est au sol
    private bool IsGrounded()
    {
        return Physics.Raycast(raycastOrigin.position, Vector3.down, ray);
    }





}



