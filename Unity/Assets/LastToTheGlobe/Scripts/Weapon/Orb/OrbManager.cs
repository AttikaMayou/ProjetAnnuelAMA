using LastToTheGlobe.Scripts.Environment.Planets;
using UnityEngine;

//Auteur: Abdallah

namespace LastToTheGlobe.Scripts.Weapon.Orb
{
    public class OrbManager : MonoBehaviour {

        [Header("Orb Parameters")]
        [SerializeField]private Transform selfPosition;
        [SerializeField]private float speed = 5f;
        private Vector3 _direction;
        private float _timeToDisable;
        [Header("Player and Attraction References")]
        [SerializeField]private Transform playerTransform;
        [SerializeField]private AttractedScript attractedScript;
        private Vector3 _centerPointAttractor;
        
        private void OnEnable()
        {
            _timeToDisable = Time.deltaTime;
            selfPosition.position = playerTransform.position + playerTransform.forward * 2f;
            _direction = playerTransform.right;
            _centerPointAttractor = attractedScript.attractor.selfTransform.position;

        }
        
        private void FixedUpdate () {
            transform.RotateAround(_centerPointAttractor,_direction,speed);

            _timeToDisable += Time.deltaTime;

            if (!(_timeToDisable >= 2f)) return;
            _timeToDisable = 0f;
            gameObject.SetActive(false);
        }
    }
}
