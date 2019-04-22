using LastToTheGlobe.Scripts.Environment.Planets;
//using NUnit.Framework.Constraints;
using UnityEngine;

//Auteur: Abdallah
//Modification : Attika

namespace LastToTheGlobe.Scripts.Weapon.Orb
{
    public class OrbManager : OrbExposerScript
    {

        public bool debug = true;
        
        [Header("Orb Parameters")]
        [SerializeField] private float speed = 5.0f;
        private Vector3 _direction;
        private float _timeUsing;
        public float maxTimeUsing;
        private Vector3 _initialPos;
        public bool charged;
        
        [Header("Player and Attraction References")]
        public Transform playerTransform;
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
            maxTimeUsing = 3f;
            if (!playerTransform) return;
            orbTransform.position = playerTransform.position + playerTransform.forward * 2f;
            _direction = playerTransform.right;
            _centerPointAttractor = attractedScript.attractor.planetTransform.position;
        }
        
        private void FixedUpdate () {

            if (charged)
            {
                transform.RotateAround(_centerPointAttractor,_direction,speed);
            }
            else
            {
                orbRb.MovePosition(transform.position + playerTransform.forward);
            }

            if (debug)
            {
                print(_timeUsing);
                print(maxTimeUsing);
            }
            _timeUsing += Time.deltaTime;

            if (!(_timeUsing >= maxTimeUsing)) return;
            _timeUsing = 0.0f;
            ResetOrb();
            charged = false;
        }

        private void ResetOrb()
        {
            if(debug) Debug.Log("reset pos of the offensive orb");
            orbTransform.position = _initialPos;
            this.gameObject.SetActive(false);
        }
    }
}
