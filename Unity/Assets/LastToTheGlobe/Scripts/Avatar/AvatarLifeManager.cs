using UnityEngine;

//Auteur : Attika

namespace LastToTheGlobe.Scripts.Avatar
{
    public class AvatarLifeManager : MonoBehaviour
    {
        [Header("Balance Settings")] 
        public int lifeStartingPoint;
        public int attackDmg;
        
        public int myLife;
        public CharacterExposer myExposer;
        
        private void Awake()
        {
            myLife = lifeStartingPoint;
        }

        private void OnCollisionEnter(Collision other)
        {
            if (!other.gameObject.CompareTag("Bullet")) return;
            //if (other.gameObject == myExposer.thirdPersonController.orb) return;
            myLife -= attackDmg;
        }

        private void FixedUpdate()
        {
            if (myLife <= 0)
            {
                
            }
        }
    }
}
