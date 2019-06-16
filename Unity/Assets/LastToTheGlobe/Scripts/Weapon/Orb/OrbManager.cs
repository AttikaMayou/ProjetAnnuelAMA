using Assets.LastToTheGlobe.Scripts.Management;
using Photon.Pun;
using UnityEngine;

//Auteur: Abdallah
//Modification : Attika

//TODO : Refacto this script (Attika)

namespace Assets.LastToTheGlobe.Scripts.Weapon.Orb
{
    public class OrbManager : global::Assets.LastToTheGlobe.Scripts.Avatar.Avatar
    {
        public static bool debug = true;

        public int Id;
        
        [Header("Orb Parameters")]
        [SerializeField] private Rigidbody _orbRb;
        public Collider OrbCd;
        [SerializeField] private float speed = 5.0f;
        private Vector3 _direction;
        private float _timeUsing;
        public float MaxTimeUsing;
        private Vector3 _initialPos;
        public bool Loaded;
        
        [Header("Player and Attraction References")]
        public Transform PlayerTransform;
        private Vector3 _centerPointAttractor;

        private void OnEnable()
        {
            if (!PhotonNetwork.IsMasterClient) return;
            //timeUsing = Time.deltaTime;
            _timeUsing = 0.0f;
            MaxTimeUsing = 3f;
            if (!PlayerTransform || !Attractor) return;
            _initialPos = PlayerTransform.position;
            transform.position = PlayerTransform.position + PlayerTransform.forward * 2f;
            _direction = PlayerTransform.right;
            _centerPointAttractor = Attractor.transform.position;
            ColliderDirectoryScript.Instance.AddOrbManager(this, out Id);
            if(debug) Debug.Log("add an orb to Directory");
        }
        
        private void FixedUpdate ()
        {
            if (!PhotonNetwork.IsMasterClient) return;
            
            if (Loaded)
            {
                transform.RotateAround(_centerPointAttractor,_direction,speed);
            }
            else
            {
                _orbRb.MovePosition(transform.position + PlayerTransform.forward);
            }

//            if (debug)
//            {
//                print(_timeUsing);
//                print(MaxTimeUsing);
//            }

            _timeUsing += Time.deltaTime;

            if (!(_timeUsing >= MaxTimeUsing)) return;
            _timeUsing = 0.0f;
            ResetOrb();
            Loaded = false;
        }

        private void ResetOrb()
        {
            if (!PhotonNetwork.IsMasterClient) return;
            
            if(debug) Debug.Log("reset pos of the pooled orb");
            
            transform.position = _initialPos;
            this.gameObject.SetActive(false);
        }

        public void InitializeOrPosition()
        {
            if (!PhotonNetwork.IsMasterClient) return;
            transform.position = PlayerTransform.position + PlayerTransform.forward * 2f;
        }

        //Dereference itself to the ColliderDirectory
        private void OnDisable()
        {
            //only the Master Client remove the orb to the directory and reset his ID
            if (!PhotonNetwork.IsMasterClient) return;
            ColliderDirectoryScript.Instance.RemoveOrbManager(this);
        }
    }
}
