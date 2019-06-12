using Assets.LastToTheGlobe.Scripts.Management;
using Photon.Pun;
using UnityEngine;

//Auteur: Abdallah
//Modification : Attika

namespace Assets.LastToTheGlobe.Scripts.Weapon.Orb
{
    public class OrbManager : global::Assets.LastToTheGlobe.Scripts.Avatar.Avatar
    {
        public bool debug = true;
        
        [Header("Orb Parameters")]
        [SerializeField] private Rigidbody orbRb;
        public Collider orbCd;
        [SerializeField] private float speed = 5.0f;
        private Vector3 _direction;
        private float _timeUsing;
        public float maxTimeUsing;
        private Vector3 _initialPos;
        public bool charged;
        
        [Header("Player and Attraction References")]
        public Transform playerTransform;
        private Vector3 _centerPointAttractor;

        private void OnEnable()
        {
            //timeUsing = Time.deltaTime;
            _timeUsing = 0.0f;
            maxTimeUsing = 3f;
            if (!playerTransform || !Attractor) return;
            _initialPos = playerTransform.position;
            transform.position = playerTransform.position + playerTransform.forward * 2f;
            _direction = playerTransform.right;
            _centerPointAttractor = Attractor.transform.position;
            if (!PhotonNetwork.IsMasterClient) return;
            ColliderDirectoryScript.Instance.AddOrbManager(this);
            if(debug) Debug.Log("add an orb to Directory");
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
            transform.position = _initialPos;
            this.gameObject.SetActive(false);
        }

        public void InitializeOrPosition()
        {
            transform.position = playerTransform.position + playerTransform.forward * 2f;
        }
    }
}
