using System.Collections.Generic;
using UnityEngine;

//Auteur : Attika

namespace LastToTheGlobe.Scripts.UI
{
    public class ActivateObjects : MonoBehaviour
    {
        public List<GameObject> objects;

        public void Activation()
        {
            foreach (var obj in objects)
            {
                obj.SetActive(true);
            }
        }
        
        public void Deactivation()
        {
            foreach (var obj in objects)
            {
                obj.SetActive(false);
            }
        }
    }
}
