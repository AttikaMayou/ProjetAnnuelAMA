using UnityEngine;
using UnityEngine.Serialization;

//Auteur : Abdallah
//Modification : Attika

namespace LastToTheGlobe.Scripts.Environment.Planets
{
	public class AttractedScript : Assets.LastToTheGlobe.Scripts.Avatar.Avatar 
	{
		[SerializeField]
		private Rigidbody attractedRb;
		public float selfGravity = -10.0f;
		
		//Bool parameter that indicate if the player is on ground or not
		[HideInInspector]
		public bool isGrounded;

		private void Awake ()
		{
			if (!attractedRb) return;
			attractedRb.constraints = RigidbodyConstraints.FreezeRotation;
			attractedRb.useGravity = false;
		}

		private void Update ()
		{
			//print(attractor.planetTransform.position);
//			if(isGrounded && attractor && attractedRb)
//			{
//				attractor.Attractor(attractedRb, this.transform, -2600f);
//			}
//			else if (!isGrounded && attractor && attractedRb)
//			{
//				attractor.Attractor(attractedRb, this.transform, selfGravity);
//			}
		}
	}
}
