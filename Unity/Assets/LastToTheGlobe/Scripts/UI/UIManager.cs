using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

//Auteur: Margot

namespace LastToTheGlobe.Scripts.UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private Canvas playerInventory;
        [SerializeField] private Canvas tutorial;
        [SerializeField] private Text nbPlayerText;
        [SerializeField] private int life;
        private bool canOpenInventory = false;


        void Start()
        {
            playerInventory.enabled = false;
            tutorial.enabled = false;
        }

        void Update()
        {
            //UI Inventory
            if (Input.GetKeyDown(KeyCode.I))
            {
                if(!canOpenInventory)
                {
                    playerInventory.enabled = true;
                    canOpenInventory = true;
                }
                else if(canOpenInventory)
                {
                    playerInventory.enabled = false;
                    canOpenInventory = false;
                }
            }
            //UI Tutorial
            if (Input.GetKey(KeyCode.F1))
            {
                tutorial.enabled = true;
            }
            else
            {
                tutorial.enabled = false;
            }
        }

        //Player currently in the game
        public void updateNbPlayer()
        {
            int nbPlayers = PhotonNetwork.PlayerList.Length;
            nbPlayerText.text = "Joueurs : " + nbPlayers.ToString();
        }
    }
}
