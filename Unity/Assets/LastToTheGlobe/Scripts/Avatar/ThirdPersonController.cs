using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Avatar = LastToTheGlobe.Scripts.Avatar.Avatar;

public class ThirdPersonController : Avatar {

    [SerializeField]
    private GameObject cameraRotatorX;

    private Vector3 moveDir;
    private Vector3 jumpDir;
    public float speed;

    public Rigidbody rb;

    [SerializeField]
    private float rotationSpeed = 5.0f;

    private bool isJumping = false;

    public GameObject orb;
    public GameObject orbSpawned;
    

    void Start ()
    {
        //cameraRotatorX.transform.rotation = transform.rotation;
    }
   
    void FixedUpdate () 
    {
        //Gestion des déplacements autour de la planète
        moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        rb.MovePosition(rb.position + transform.TransformDirection(moveDir) * speed * Time.deltaTime);
        //Permet de tourner le personnage pour donner la direction à la caméra sur l'axe horizontale
        transform.Rotate(new Vector3(0, Input.GetAxis("Mouse X") * rotationSpeed, 0));
        //Permet de tourner le gameobject qui donne la direction à la caméra sur l'axe vertical
        cameraRotatorX.transform.Rotate(new Vector3(-(Input.GetAxis("Mouse Y") * rotationSpeed), 0, 0), Space.Self);
        //Les deux conditions juste en dessous permettent à la caméra de ne pas aller trop haut ni trop bas
        //Les valeurs qui était mise sont des valeurs qui peuvent être pris par la variable cameraRotatorX.transform.rotation.x (-1 - 1)
        if (cameraRotatorX.transform.rotation.x >= 0.42f)
        {
            cameraRotatorX.transform.rotation = new Quaternion(0.42f, cameraRotatorX.transform.rotation.y, cameraRotatorX.transform.rotation.z, cameraRotatorX.transform.rotation.w);
        }
        if (cameraRotatorX.transform.rotation.x <= -0.2f)
        {
            cameraRotatorX.transform.rotation = new Quaternion(-0.2f, cameraRotatorX.transform.rotation.y, cameraRotatorX.transform.rotation.z, cameraRotatorX.transform.rotation.w);
        }
        //Gestion du saut en appliquant force plus èlevé que la gravité.
        if(Input.GetKey(KeyCode.Space) && !isJumping)
        {

            jumpDir = attractor.dirForce;
            rb.AddForce(jumpDir * 250);

        //Interdiction du saut
            isJumping = true;
        }
    }
    //Autorisation du saut
    void OnCollisionEnter(Collision hit)
    {
        if(hit.gameObject.tag == "planet")
        {
            isJumping = false;
            gameObject.GetComponent<AttractedScript>().firstStepOnGround = false;
        }
    }
}
