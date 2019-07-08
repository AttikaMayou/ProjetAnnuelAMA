﻿using UnityEngine;

//Auteur : Margot

namespace Assets.LastToTheGlobe.Scripts.Network
{
    public class AIntentReceiver : MonoBehaviour
    {
       public bool Move { get; set; }
       public bool Run { get; set; }
       //public bool Jump { get; set; }
       public bool Dash { get; set; }
       public bool Shoot { get; set; }
       public bool TakeItem { get; set; }
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
    }
}
