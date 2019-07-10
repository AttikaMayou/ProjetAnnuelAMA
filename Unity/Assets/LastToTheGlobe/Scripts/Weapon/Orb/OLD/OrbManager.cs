using Photon.Pun;
using UnityEngine;

//Auteur: Abdallah

namespace LastToTheGlobe.Scripts.Weapon.Orb.OLD
{
    public class OrbManager : global::LastToTheGlobe.Scripts.Avatar.Avatar
    {
        /*public bool debug = true;
        //TODO : refacto this (and remove OrbsPhotonViewSender cause we dont need it)
        
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
            //timeUsing = Time.deltaTime;
            _timeUsing = 0.0f;
            MaxTimeUsing = 3f;
            if (!PlayerTransform || !Attractor) return;
            _initialPos = PlayerTransform.position;
            transform.position = PlayerTransform.position + PlayerTransform.forward * 2f;
            _direction = PlayerTransform.right;
            _centerPointAttractor = Attractor.transform.position;
            if (!PhotonNetwork.IsMasterClient) return;
            //ColliderDirectoryScript.Instance.AddOrbManager(this, out Id);
            if(debug) Debug.Log("add an orb to Directory");
        }
        
        private void FixedUpdate () {

            if (Loaded)
            {
                transform.RotateAround(_centerPointAttractor,_direction,speed);
            }
            else
            {
                _orbRb.MovePosition(transform.position + PlayerTransform.forward);
            }

            if (debug)
            {
                print(_timeUsing);
                print(MaxTimeUsing);
            }
            _timeUsing += Time.deltaTime;

            if (!(_timeUsing >= MaxTimeUsing)) return;
            _timeUsing = 0.0f;
            ResetOrb();
            Loaded = false;
        }

        private void ResetOrb()
        {
            if(debug) Debug.Log("reset pos of the offensive orb");
            transform.position = _initialPos;
            this.gameObject.SetActive(false);
        }

        public void InitializeOrPosition()
        {
            transform.position = PlayerTransform.position + PlayerTransform.forward * 2f;
        }

        //Dereference itself to the ColliderDirectory
        private void OnDisable()
        {
            //only the Master Client remove the orb to the directory and reset his ID
            if (!PhotonNetwork.IsMasterClient) return;
            //ColliderDirectoryScript.Instance.RemoveOrbManager(this);
        }*/
    }
}
