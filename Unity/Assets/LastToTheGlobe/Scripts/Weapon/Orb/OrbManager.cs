using LastToTheGlobe.Scripts.Environment.Planets;
using UnityEngine;

//Auteur: Abdallah
//Modification : Attika

namespace LastToTheGlobe.Scripts.Weapon.Orb
{
    public class OrbManager : OrbExposerScript {

        [Header("Orb Parameters")]
        [SerializeField]private float speed = 5f;
        private Vector3 _direction;
        private float timeUsing;
        public float maxTimeUsing;
        private Vector3 initialPos;
        
        [Header("Player and Attraction References")]
        [SerializeField]private Transform playerTransform;
        [SerializeField]private AttractedScript attractedScript;
        private Vector3 _centerPointAttractor;

        private void Awake()
        {
            initialPos = orbTransform.position;
        }

        private void OnEnable()
        {
            //timeUsing = Time.deltaTime;
            timeUsing = 0.0f;
            orbTransform.position = playerTransform.position + playerTransform.forward * 2f;
            _direction = playerTransform.right;
            _centerPointAttractor = attractedScript.attractor.planetTransform.position;
        }
        
        private void FixedUpdate () {
            transform.RotateAround(_centerPointAttractor,_direction,speed);

            timeUsing += Time.deltaTime;

            if (!(timeUsing >= maxTimeUsing)) return;
            timeUsing = 0.0f;
            ResetOrb();
        }

        private void ResetOrb()
        {
            orbTransform.position = initialPos;
            this.gameObject.SetActive(false);
        }
    }
}
