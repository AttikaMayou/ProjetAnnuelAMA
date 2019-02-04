using UnityEngine;

//Auteur : Abdallah

namespace LastToTheGlobe.Scripts.Environment.Planets
{
    public class characterTrampolineScript : MonoBehaviour
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
            print(cooldownFinished);
            if (Input.GetKeyDown(KeyCode.Space) && canHyperJump && cooldownFinished <= 0f)
            {
                cooldownFinished = 10f;
                playerRigibody.AddForce(attractor.dirForce * 1300f);
                
            }

            while (cooldownFinished >= 0f)
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
