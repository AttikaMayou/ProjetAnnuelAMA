using LastToTheGlobe.Scripts.Avatar;
using UnityEngine;
using UnityEngine.Serialization;

//Auteur : Abdallah
//Modification : Attika

namespace LastToTheGlobe.Scripts.Environment.Planets
{
    public class CharacterTrampolineScript : MonoBehaviour
    {
        [SerializeField] private CharacterExposer playerExposer;
        public KeyCode defensiveOrbInput;
        private bool _canHyperJump;
        private float _cooldownFinished = 0.0f;

        private void Update()
        {
            if (Input.GetKeyDown(defensiveOrbInput) && _canHyperJump && _cooldownFinished <= 0f)
            {
                _cooldownFinished = 10f;
                playerExposer.characterRb.AddForce(transform.up * 1300f);
            }

            if (_cooldownFinished >= 0f)
            {
                _cooldownFinished -= Time.deltaTime;
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if(collision.collider.CompareTag("Jumper"))
            {
                _canHyperJump = true;
            }
        }

        private void OnCollisionExit(Collision collision)
        {
            if (collision.collider.CompareTag("Jumper"))
            {
                _canHyperJump = false;
            }

        }
    }
}
