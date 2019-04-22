using UnityEngine;

//Auteur : Attika

namespace LastToTheGlobe.Scripts.Environment
{
    public class SunController : MonoBehaviour
    {
        [SerializeField] private Vector3 startPos;
        
        private void Awake()
        {
            gameObject.transform.position = startPos;
        }
        
        //TODO : add methods for sun control (UI feedback for players + movement of the object + lighting setup)
    }
}
