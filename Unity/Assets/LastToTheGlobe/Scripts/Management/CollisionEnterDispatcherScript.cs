using System;
using UnityEngine;

//Auteur : Attika

namespace Assets.LastToTheGlobe.Scripts.Management
{
    public class CollisionEnterDispatcherScript : MonoBehaviour
    {
        public event Action<CollisionEnterDispatcherScript, Collider> CollisionEvent;

        public void OnCollisionEnter(Collision other)
        {
            CollisionEvent?.Invoke(this, other.collider);
        }
    }
}
