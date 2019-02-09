using UnityEngine;
using LastToTheGlobe.Scripts.Environment.Planets;

//Auteur : Abdallah

namespace LastToTheGlobe.Scripts.Environment.Planets
{
	public class AttractedScript : Avatar.Avatar {

		private Transform _myTransform;
		[SerializeField]
		private Rigidbody attractedRigidbody;
		[SerializeField]
		private float selfGravity = -10f;
		
		[HideInInspector]
		public bool firstStepOnGround;

		private void Start () {

			attractedRigidbody.constraints = RigidbodyConstraints.FreezeRotation;
			attractedRigidbody.useGravity = false;
			_myTransform = transform;
		}

		private void Update ()
		{
			if(firstStepOnGround && attractor)
			{
				attractor.Attractor(attractedRigidbody, _myTransform, -2600f);
			}
			else if (!firstStepOnGround && attractor)
			{
				attractor.Attractor(attractedRigidbody, _myTransform, selfGravity);
			}
        
		}
	}
}
