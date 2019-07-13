using LastToTheGlobe.Scripts.Management;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Serialization;

//Auteur: Abdallah

namespace LastToTheGlobe.Scripts.Weapon.Orb
{
    public class OrbManager : global::LastToTheGlobe.Scripts.Avatar.Avatar
    {
        public bool debug = true;
        
        [Header("Orb Parameters")]
        public OrbExposerScript exposer;
        private Vector3 _playerPosition;
        private Vector3 _direction;
        private float _timeUsing;
        private Vector3 _initialPos;
        public bool loaded;
        
        private Vector3 _centerPointAttractor;

        private void FixedUpdate () {

            if (loaded)
            {
                transform.RotateAround(_centerPointAttractor,_direction, GameVariablesScript.Instance.orbOffensiveSpeed);
            }
            else
            {
                exposer.orbRb.MovePosition(transform.position + exposer.playerExposer.CharacterTr.forward);
            }
            
            _timeUsing += Time.deltaTime;

            if (!(_timeUsing >= GameVariablesScript.Instance.lifeTimeOrb)) return;
            _timeUsing = 0.0f;
            loaded = false;
            ResetOrb();
        }

        private void ResetOrb()
        {
            if(debug) Debug.LogFormat("[OrbManager] Reset position of the offensive orb {0}", exposer.id);
            exposer.orbRb.isKinematic = true;
            transform.position = _initialPos;
            this.gameObject.SetActive(false);
        }

        public void InitializeOrPosition()
        {  
            _timeUsing = 0.0f;
            if (!Attractor || !exposer.playerExposer) return;
            _playerPosition = exposer.playerExposer.CharacterTr.position;
            _initialPos = _playerPosition;
            _direction = exposer.playerExposer.CharacterTr.right;
            _centerPointAttractor = Attractor.transform.position;
            transform.position = _playerPosition + exposer.playerExposer.CharacterTr.forward * 2f;
            exposer.orbRb.isKinematic = false;
        }
    }
}
