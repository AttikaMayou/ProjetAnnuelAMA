using LastToTheGlobe.Scripts.Avatar;
using UnityEngine;
using UnityEngine.Serialization;

//Auteur : Abdallah
//Modification : Attika

namespace LastToTheGlobe.Scripts.Environment.Planets
{
    public class CharacterTrampolineScript : Avatar.Avatar
    {
        [SerializeField] private CharacterExposer playerExposer;
        public KeyCode defensiveOrbInput;
        private bool canHyperJump;
        private float cooldownFinished = 0.0f;

        private void Update()
        {
            if (Input.GetKeyDown(defensiveOrbInput) && canHyperJump && cooldownFinished <= 0f)
            {
                cooldownFinished = 10f;
                playerExposer.playerRb.AddForce(transform.up * 1300f);
            }

            if (cooldownFinished >= 0f)
            {
                cooldownFinished -= Time.deltaTime;
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if(collision.collider.CompareTag("jumper"))
            {
                canHyperJump = true;
            }
        }

        private void OnCollisionExit(Collision collision)
        {
            if (collision.collider.CompareTag("jumper"))
            {
                canHyperJump = false;
            }

        }
    }
}
