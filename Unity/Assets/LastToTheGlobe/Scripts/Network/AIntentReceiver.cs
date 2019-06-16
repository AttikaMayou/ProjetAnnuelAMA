using UnityEngine;

//Auteur : Margot

namespace LastToTheGlobe.Scripts.Network
{
    public class AIntentReceiver : MonoBehaviour
    {
       public bool Move { get; set; }
       public bool Run { get; set; }
       //public bool Jump { get; set; }
       public bool Dash { get; set; }
       public bool Shoot { get; set; }
       
       public bool ShootLoaded { get; set; }
       public bool Bump { get; set; }
       public bool Interact { get; set; }

       public float Forward = 0.0f;
       public float Strafe = 0.0f;
       public float Speed = 5.0f;
       public float LoadShotValue = 0.0f;
       public float RotationOnX = 0.0f;
       public float RotationOnY = 0.0f;
       public float RotationSpeed = 5.0f;

       public bool CanDash = true;
       //public bool CanJump = true;
       public bool CanShoot = true;
       protected const float WalkSpeed = 5.0f;
       protected const float RunSpeed = 8.0f;
       protected const float DashSpeed = 15.0f;
       protected const float ShootLoadTime = 1.5f;
       protected const float ShootDamage = 10.0f;
       protected const float ShootLoadedDamage = 50.0f;
    }
}
