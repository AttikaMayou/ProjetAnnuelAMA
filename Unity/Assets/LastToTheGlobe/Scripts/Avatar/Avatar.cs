using Assets.LastToTheGlobe.Scripts.Environment.Planets;
using UnityEngine;

//Auteur : Abdallah

namespace Assets.LastToTheGlobe.Scripts.Avatar
{
    public class Avatar : MonoBehaviour
    {
        [HideInInspector]
        public AttractorScript Attractor;

        private float _selfGravity = -10.0f;

        /// <summary>
        /// Set the gravity (float)
        /// </summary>
        /// <param name="gravity"></param>
        public void SetGravity(float gravity)
        {
            _selfGravity = gravity;
        }

        /// <summary>
        /// Return the value of the gravity (float)
        /// </summary>
        /// <returns></returns>
        public float GetGravity()
        {
            return _selfGravity;
        }
    }
}
