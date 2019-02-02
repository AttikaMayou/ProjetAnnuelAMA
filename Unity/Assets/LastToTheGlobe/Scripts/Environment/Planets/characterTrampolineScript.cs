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

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space) && canHyperJump)
            {
                playerRigibody.AddForce(attractor.dirForce * 1300f);
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
