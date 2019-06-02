using UnityEngine;

//Auteur : Margot

namespace LastToTheGlobe.Scripts.Network
{
    public class AIntentReceiver : MonoBehaviour
    {
       public bool MoveBack { get; set; }
       public bool MoveForward { get; set; }
       public bool MoveLeft { get; set; }
       public bool MoveRight { get; set; }
       public bool Run { get; set; }
       public bool Jump { get; set; }
       public bool Dash { get; set; }
       public bool Shoot { get; set; }
       public bool TakeItem { get; set; }
       public bool ShootLoaded { get; set; }
       public bool Bump { get; set; }
       public bool Interact { get; set; }

       public float forward = 0.0f;
       public float strafe = 0.0f;
       public float speed = 5.0f;
       public float loadShotValue = 0.0f;
       public float rotationOnX = 0.0f;
       public float rotationOnY = 0.0f;
       public float rotationSpeed = 5.0f;

       public bool canDash = true;
       public bool canJump = true;
       public bool canShoot = true;
       protected const float walkSpeed = 5.0f;
       protected const float runSpeed = 8.0f;
       protected const float dashSpeed = 15.0f;
    }
}
