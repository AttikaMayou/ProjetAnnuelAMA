using LastToTheGlobe.Scripts.Avatar;
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
        private Vector3 _playerPosition;
        private Vector3 _direction;
        private float _timeUsing;
        private Vector3 _initialPos;
        public bool loaded;
        private Vector3 _centerPointAttractor;
        private bool _getUsed;
        public CharacterExposerScript otherExposer;
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
                exposer.orbRb.MovePosition(transform.position + exposer.playerExposer.CharacterTr.forward);
            }
            
            _timeUsing += Time.deltaTime;

            if (!(_timeUsing >= GameVariablesScript.Instance.lifeTimeOrb)) return;
            _timeUsing = 0.0f;
            loaded = false;
            ResetOrb();
        }

        private void OnCollisionEnter(Collision other)
        {
            //Dont forget to uncomment this for final version
            otherExposer = ColliderDirectoryScript.Instance.GetCharacterExposer(other.collider);
            
            if (other.gameObject.CompareTag("Player"))
            {    
                if (otherExposer != exposer.playerExposer)
                {
                    print(exposer);
                    print("The Player i want to hurt " +otherExposer.Id);
                    exposer.orbsPhotonView.RPC("InflictDamage",RpcTarget.MasterClient, exposer.playerExposer.Id, otherExposer.Id);
                    ResetOrb();
                }
            }
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
            //if (!exposer.Attractor || !exposer.playerExposer) return;
            _playerPosition = exposer.playerExposer.CharacterTr.position;
            _initialPos = _playerPosition;
            _direction = exposer.playerExposer.CharacterTr.right;
            //_centerPointAttractor = exposer.Attractor.transform.position;
            transform.position = _initialPos + exposer.playerExposer.CharacterTr.forward * 2f;
            exposer.orbRb.isKinematic = false;
            _getUsed = true;
        }
    }
}
