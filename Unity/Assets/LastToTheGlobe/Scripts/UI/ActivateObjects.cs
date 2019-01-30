using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

//Auteur : Attika

namespace LastToTheGlobe.Scripts.UI
{
    public class ActivateObjects : MonoBehaviour
    {
        public List<GameObject> Objects;

        public void Activation()
        {
            foreach (var obj in Objects)
            {
                obj.SetActive(true);
            }
        }
        
        public void Deactivation()
        {
            foreach (var obj in Objects)
            {
                obj.SetActive(false);
            }
        }
        
    }
}
