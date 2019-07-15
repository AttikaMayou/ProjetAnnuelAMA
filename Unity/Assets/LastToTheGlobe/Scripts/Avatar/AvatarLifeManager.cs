using Assets.LastToTheGlobe.Scripts.Avatar;
using LastToTheGlobe.Scripts.Management;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using LastToTheGlobe.Scripts.UI;

//Auteur : Attika
//Modifications : Margot

namespace LastToTheGlobe.Scripts.Avatar
{
    public class AvatarLifeManager : MonoBehaviour
    {
        [SerializeField] private ActivateObjects defeat;
        [SerializeField] private ActivateObjects lifeUI;

        [Header("Balance Settings")] 
        public int lifeStartingPoint = 100;
        public int attackDmg;
        public bool inLife = true;
        public int myLife = 100;
        [SerializeField] private CharacterExposerScript myExposer;

        [SerializeField]
        private Text textHealth;

        [SerializeField]
        private Text textHeathOther;

        private Collider selfCollider;
        private void Awake()
        {
            myLife = lifeStartingPoint;
            //defeat.Deactivation();
            //myExposer.lifeUI.Activation();
        }

        public void InflictDamage()
        {
                if(myLife <= 0)
                {
                    textHealth.text = "Health : 0";
                    defeat.Activation();
                }
                else
                {
                    myLife -= attackDmg;
                    textHealth.text = "Health :" + myLife;
                }
                
                Debug.LogFormat("The player with id {0} has updated HP : {1}", myExposer.Id, myLife);

                myExposer.CharacterPhotonView.RPC("MajMine", RpcTarget.Others, myLife);
            
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.P))
            {
                if (myLife <= 0)
                {
                    //textHealth.text = "Health : 0";
                }
                else
                {
                    myLife -= 10;
                    //textHealth.text = "Health : " + myLife;
                }
            }

            if (myLife <= 0)
            {
                inLife = false;
                //myExposer.defeatUI.Activation();
            }
        }

        [PunRPC]
        void MajMine()
        {
            //textHeathOther.text = "Health :" + myLife;
        }
    }
}
