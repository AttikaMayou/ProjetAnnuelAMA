using UnityEngine;
using LastToTheGlobe.Scripts.Management;
//Auteur : Attika

namespace LastToTheGlobe.Scripts.Environment
{
    public class SunController : MonoBehaviour
    {
        public float t;

        private void Awake()
        {
            gameObject.transform.position = GameVariablesScript.Instance.startPos;
        }
        
        private void Update()
        {
            t += Time.deltaTime / GameVariablesScript.Instance.timeToReachTarget;
            transform.position = Vector3.Lerp(GameVariablesScript.Instance.startPos, GameVariablesScript.Instance.target, t);
        }


        //TODO : add methods for sun control (UI feedback for players + movement of the object + lighting setup)
    }
}
