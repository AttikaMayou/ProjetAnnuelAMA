using Assets.LastToTheGlobe.Scripts.Environment.Planets;
using LastToTheGlobe.Scripts.Environment.Planets;
using UnityEngine;

//Auteur : Abdallah

namespace Assets.LastToTheGlobe.Scripts.Avatar
{
    public class Avatar : MonoBehaviour
    {
        [HideInInspector]
        public AttractorScript Attractor;

        private float _selfGravity = -10.0f;

        // Set the gravity (float)
        public void SetGravity(float gravity)
        {
            _selfGravity = gravity;
        }

        // Return the value of the gravity (float)
        public float GetGravity()
        {
            return _selfGravity;
        }
    }
}
