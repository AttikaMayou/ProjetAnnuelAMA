using LastToTheGlobe.Scripts.Singleton;
using UnityEngine;
using UnityEngine.Serialization;

//Auteur : Attika

namespace LastToTheGlobe.Scripts.Management
{
    public class GameVariablesScript : MonoBehaviourSingleton<GameVariablesScript>
    {
        //TODO : initialize values there hen its definitely ok and add 'const' attribute 
        
        [FormerlySerializedAs("WalkSpeed")] [Header("Movement Parameters")] 
        public int walkSpeed;
        [FormerlySerializedAs("RunSpeed")] public int runSpeed;
        [FormerlySerializedAs("DashSpeed")] public int dashSpeed;
        public bool lockCursor;

        [FormerlySerializedAs("ShootLoadTime")] [Header("Shoot Parameters")] 
        public float shootLoadTime;
        [FormerlySerializedAs("ShootDamage")] public int shootDamage;
        [FormerlySerializedAs("ShootLoadedDamage")] public int shootLoadedDamage;

        [FormerlySerializedAs("DashCooldown")] [Header("Cooldown Values")] 
        public float dashCooldown;
        [FormerlySerializedAs("BumpCooldown")] public float bumpCooldown;

        [FormerlySerializedAs("LifeInitial")] [Header("Hp Parameters")] 
        public int lifeInitial;
    }
}
