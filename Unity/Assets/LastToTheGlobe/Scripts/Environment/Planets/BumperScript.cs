using LastToTheGlobe.Scripts.Avatar;
using TMPro;
using UnityEngine;

//Auteur : Abdallah
//Modification : Attika

namespace LastToTheGlobe.Scripts.Environment.Planets
{
    public class BumperScript : MonoBehaviour
    {
        [SerializeField] private CharacterExposer playerExposer;
        public KeyCode defensiveInput;
        private bool _canHyperJump;
        private float _cooldownFinished = 0.0f;

        private void Update()
        {
            if (Input.GetKeyDown(defensiveInput) && _canHyperJump && _cooldownFinished <= 0.0f)
            {
                _cooldownFinished = 10.0f;
                playerExposer.characterRb.AddForce(transform.up * 1300.0f);
            }

            if (_cooldownFinished >= 0.0f)
            {
                _cooldownFinished -= Time.deltaTime;
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.collider.CompareTag("Jumper"))
            {
                _canHyperJump = true;
            }
        }

        private void OnCollisionExit(Collision other)
        {
            if (other.collider.CompareTag("Jumper"))
            {
                _canHyperJump = false;
            }
        }
    }
}
