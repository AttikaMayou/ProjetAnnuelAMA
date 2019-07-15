using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Auteur : Abdallah

namespace LastToTheGlobe.Scripts.UI.UIConnexion
{
    public class ButtonScript : MonoBehaviour
    {
        public bool shopOpen;
        public Button shopButton;
        public Button returnButton;
        public List<GameObject> shop = new List<GameObject>();
        public List<GameObject> menu = new List<GameObject>();

        void Start () {
            shopButton.onClick.AddListener(toShop);
            returnButton.onClick.AddListener(toMenu);
        }
        
        public void toMenu()
        {
            if (menu != null)
            {
                for (int i = 0; i < menu.Count; i++)
                {
                    print("Enabled");
                    menu[i].SetActive(true);
                }
            }

            if (shop != null)
            {
                for (int i = 0; i < shop.Count; i++)
                {
                    print("Disabled");
                    shop[i].SetActive(false);
                }
            }

        }

        public void toShop()
        {
            shopOpen = true;
            if (shop != null)
            {
                for (int i = 0; i < shop.Count; i++)
                {
                    print("Enabled");
                    shop[i].SetActive(true);
                }
            }

            if (menu != null)
            {
                for (int i = 0; i < menu.Count; i++)
                {
                    print("Disabled");
                    menu[i].SetActive(false);
                }
            }
        }
    }
}