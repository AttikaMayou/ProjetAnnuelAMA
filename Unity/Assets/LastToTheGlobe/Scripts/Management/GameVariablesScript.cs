using LastToTheGlobe.Scripts.Singleton;
using UnityEngine;
using UnityEngine.Serialization;

//Auteur : Attika
//Modifications : Margot

namespace LastToTheGlobe.Scripts.Management
{
    public class GameVariablesScript : MonoBehaviourSingleton<GameVariablesScript>
    {
        //TODO : initialize values there hen its definitely ok and add 'const' attribute 
        
        [Header("Movement Parameters")] 
        public int walkSpeed;
        public int runSpeed;
        public int dashSpeed;
        public bool lockCursor;

        [Header("Gravity Parameters")] 
        public float planetsGravity;
        public float speedPlanetRotation;
        public float bumpersForce;

        [Header("Shoot Parameters")] 
        public float shootCooldown;
        public float shootLoadTime;
        public int shootDamage;
        public int shootLoadedDamage;
        public float orbOffensiveSpeed;
        public float lifeTimeOrb;

        [Header("Orb Defensive Parameters")] 
        public float orbDefensiveSpeed;

        [Header("Cooldown Values")] 
        public float dashCooldown;
        public float bumpCooldown;

        [Header("Hp Parameters")] 
        public int lifeInitial;

        [Header("Environment Parameters")]
        public int nbreOfTremplin;
        public float timeToReachTarget;
        public Vector3 target;
        public Vector3 startPos = new Vector3(0, -1000, 0);
    }
}
