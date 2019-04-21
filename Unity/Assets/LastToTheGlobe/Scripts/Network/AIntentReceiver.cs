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
       public bool Bump { get; set; }
       public bool Interact { get; set; }

       public float forward;
       public float strafe;
       public float speed = 5.0f;
       public const float walkSpeed = 5.0f;
       public const float runSpeed = 8.0f;
       public const float dashSpeed = 15.0f;
    }
}
