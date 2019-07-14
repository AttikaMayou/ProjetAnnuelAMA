using UnityEngine;
using UnityEngine.Serialization;

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

       

       [FormerlySerializedAs("Forward")] public float forward = 0.0f;
       [FormerlySerializedAs("Strafe")] public float strafe = 0.0f;
       [FormerlySerializedAs("Speed")] public float speed = 5.0f;
       [FormerlySerializedAs("LoadShotValue")] public float loadShotValue = 0.0f;
       [FormerlySerializedAs("RotationOnX")] public float rotationOnX = 0.0f;
       [FormerlySerializedAs("RotationOnY")] public float rotationOnY = 0.0f;
       public bool lockCursor;
       public bool inChest;
       [FormerlySerializedAs("RotationSpeed")] public float rotationSpeed = 5.0f;

       [FormerlySerializedAs("CanDash")] public bool canDash = true;
       //public bool CanJump = true;
       [FormerlySerializedAs("CanShoot")] public bool canShoot = true;
       [FormerlySerializedAs("CanBump")] public bool canBump = true;
    }
}
