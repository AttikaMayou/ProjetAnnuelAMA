using LastToTheGlobe.Scripts.Environment.Planets;
using UnityEngine;

//Auteur: Abdallah
//Modification : Attika

namespace LastToTheGlobe.Scripts.Weapon.Orb
{
    public class OrbManager : OrbExposerScript {

        [Header("Orb Parameters")]
        [SerializeField] private float speed = 5.0f;
        private Vector3 _direction;
        private float _timeUsing;
        public float maxTimeUsing;
        private Vector3 _initialPos;
        
        [Header("Player and Attraction References")]
        [SerializeField] private Transform playerTransform;
        [SerializeField] private AttractedScript attractedScript;
        private Vector3 _centerPointAttractor;

        private void Awake()
        {
            _initialPos = orbTransform.position;
        }

        private void OnEnable()
        {
            //timeUsing = Time.deltaTime;
            _timeUsing = 0.0f;
            orbTransform.position = playerTransform.position + playerTransform.forward * 2f;
            _direction = playerTransform.right;
            _centerPointAttractor = attractedScript.attractor.planetTransform.position;
        }
        
        private void FixedUpdate () {
            transform.RotateAround(_centerPointAttractor,_direction,speed);

            _timeUsing += Time.deltaTime;

            if (!(_timeUsing >= maxTimeUsing)) return;
            _timeUsing = 0.0f;
            ResetOrb();
        }

        private void ResetOrb()
        {
            orbTransform.position = _initialPos;
            this.gameObject.SetActive(false);
        }
    }
}
