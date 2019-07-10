using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Auteur : Abdallah

namespace LastToTheGlobe.Scripts.UI.UIConnexion
{
    public class ButtonScript : MonoBehaviour
    {
        public Button myButton;
        public List<GameObject> toEnable = new List<GameObject>();
        public List<GameObject> toDisable = new List<GameObject>();

        void Start () {
            myButton.onClick.AddListener(TaskOnClick);
        }
        
        protected void TaskOnClick()
        {
            if (toEnable != null)
            {
                for (int i = 0; i < toEnable.Count; i++)
                {
                    toEnable[i].SetActive(true);
                }
            }

            if (toDisable != null)
            {
                for (int i = 0; i < toDisable.Count; i++)
                {
                    toDisable[i].SetActive(false);
                }
            }

        }
    }
}