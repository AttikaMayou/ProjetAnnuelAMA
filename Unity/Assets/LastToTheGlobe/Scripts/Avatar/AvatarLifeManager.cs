using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Pun.UtilityScripts;

//Auteur : Attika
//modification : Margot

namespace LastToTheGlobe.Scripts.Avatar
{
    public class AvatarLifeManager : MonoBehaviour
    {
        [SerializeField] private Canvas defeat;

        [Header("Balance Settings")] 
        public int lifeStartingPoint = 100;
        public int attackDmg;

        public bool inLife = true;
        
        public int myLife = 100;
        public CharacterExposer myExposer;

        [SerializeField]
        private Text textHealth;

        [SerializeField]
        private Text textHeathOther;

        private void Start()
        {
            defeat.enabled = false;
        }

        private void Awake()
        {
            myLife = lifeStartingPoint;
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.tag == "Bullet" && myExposer.characterLocalPhotonView.IsMine)
            {
                if(myLife <= 0)
                {
                    textHealth.text = "Health : 0";

                }
                else
                {
                    myLife -= attackDmg;
                    textHealth.text = "Health :" + myLife;
                }

                myExposer.characterLocalPhotonView.RPC("MajMine", RpcTarget.Others, myLife);
            }
            else
            {
                return;
            }
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.P))
            {
                if (myLife <= 0)
                {
                    textHealth.text = "Health : 0";
                }
                else
                {
                    myLife -= 10;
                    textHealth.text = "Health :" + myLife;
                }
            }

            if (myLife <= 0)
            {
                inLife = false;
                defeat.enabled = true;
            }
        }

        [PunRPC]
        void MajMine()
        {
            textHeathOther.text = "Health :" + myLife;
        }
    }
}
