using LastToTheGlobe.Scripts.Management;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Serialization;

//Auteur: Abdallah

namespace LastToTheGlobe.Scripts.Weapon.Orb
{
    public class OrbManager : MonoBehaviour
    {
        public bool debug = true;
        
        [Header("Orb Parameters")]
        public OrbExposerScript exposer;
        [SerializeField] private Vector3 _playerPosition;
        [SerializeField] private Vector3 _direction;
        private float _timeUsing;
        [SerializeField] private Vector3 _initialPos;
        public bool loaded;
        [SerializeField] private Vector3 _centerPointAttractor;
        private bool _getUsed;

        private void Awake()
        {
            _getUsed = false;
        }
        
        private void FixedUpdate ()
        {
            if (!_getUsed) return;
            if (loaded)
            {
                transform.RotateAround(_centerPointAttractor,_direction, GameVariablesScript.Instance.orbOffensiveSpeed);
            }
            else
            {
                exposer.orbRb.MovePosition(transform.position + exposer.playerExposer.characterTr.forward);
            }
            
            _timeUsing += Time.deltaTime;

            if (!(_timeUsing >= GameVariablesScript.Instance.lifeTimeOrb)) return;
            _timeUsing = 0.0f;
            loaded = false;
            ResetOrb();
        }

        private void ResetOrb()
        {
            _getUsed = false;
            if(debug) Debug.LogFormat("[OrbManager] Reset position of the offensive orb {0}", exposer.id);
            exposer.orbRb.isKinematic = true;
            transform.position = _initialPos;
            this.gameObject.SetActive(false);
        }

        public void InitializeOrPosition()
        {
            _timeUsing = 0.0f;
            if (!exposer.Attractor || !exposer.playerExposer) return;
            _playerPosition = exposer.playerExposer.characterTr.position;
            if(debug) Debug.LogFormat("[OrbManager] Player position : {0}", _playerPosition);
            _initialPos = _playerPosition;
            _direction = exposer.playerExposer.characterTr.right;
            if(debug) Debug.LogFormat("[OrbManager] Direction : {0}", _direction);
            _centerPointAttractor = exposer.Attractor.transform.position;
            if(debug) Debug.LogFormat("[OrbManager] Center point attractor : {0}", _centerPointAttractor);
            transform.position = _playerPosition + exposer.playerExposer.characterTr.forward * 2f;
            exposer.orbRb.isKinematic = false;
            _getUsed = true;
        }
    }
}
