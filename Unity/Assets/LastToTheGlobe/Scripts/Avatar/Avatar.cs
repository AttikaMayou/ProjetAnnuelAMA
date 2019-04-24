using LastToTheGlobe.Scripts.Environment.Planets;
using UnityEngine;

//Auteur : Abdallah

namespace LastToTheGlobe.Scripts.Avatar
{
    public class Avatar : MonoBehaviour
    {
        [HideInInspector]
        public AttractorScript attractor;

        private float _selfGravity = -10.0f;

        public void SetGravity(float gravity)
        {
            _selfGravity = gravity;
        }

        public float GetGravity()
        {
            return _selfGravity;
        }
    }
}
