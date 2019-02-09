using UnityEngine;

//Auteur : Abdallah

namespace LastToTheGlobe.Scripts.Environment.Planets
{
    public class characterTrampolineScript : Avatar.Avatar
    {
    
        public AttractorScript attractor;

        [SerializeField]
        private Rigidbody playerRigibody;

        private bool canHyperJump;

        private float cooldownFinished = 0f;
        

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            
            if (Input.GetKeyDown(KeyCode.Space) && canHyperJump && cooldownFinished <= 0f)
            {
                cooldownFinished = 10f;
                playerRigibody.AddForce(transform.up * 1300f);
                
            }

            if (cooldownFinished >= 0f)
            {
                cooldownFinished -= Time.deltaTime;
            }
        }

        void OnCollisionEnter(Collision collision)
        {
            if(collision.collider.tag == "jumper")
            {
                canHyperJump = true;
            }
        }

        void OnCollisionExit(Collision collision)
        {
            if (collision.collider.tag == "jumper")
            {
                canHyperJump = false;
            }

        }
    }
}
