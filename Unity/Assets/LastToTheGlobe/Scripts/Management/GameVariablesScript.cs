using LastToTheGlobe.Scripts.Singleton;
using UnityEngine;

//Auteur : Attika

namespace Assets.LastToTheGlobe.Scripts.Management
{
    public class GameVariablesScript : MonoBehaviourSingleton<GameVariablesScript>
    {
        //TODO : initialize values there hen its definitely ok and add 'const' attribute 
        
        [Header("Movement Parameters")] 
        public int WalkSpeed;
        public int RunSpeed;
        public int DashSpeed;

        [Header("Shoot Parameters")] 
        public float ShootLoadTime;
        public int ShootDamage;
        public int ShootLoadedDamage;

        [Header("Hp Parameters")] 
        public int LifeInitial;
    }
}
