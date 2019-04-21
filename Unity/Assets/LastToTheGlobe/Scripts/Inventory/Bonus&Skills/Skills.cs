using System.Collections.Generic;
using LastToTheGlobe.Scripts.Avatar;
using UnityEngine;

//Auteur : Attika
//Modif : Abdallah

namespace LastToTheGlobe.Scripts.Inventory
{
    public class Skills : ObjectScript
    {
        public delegate void SkillsMethod();

        public Dictionary<string, SkillsMethod> skillsMethods = new Dictionary<string, SkillsMethod>();
        
        //Getting CharacterExposer
        public CharacterExposer characterExposer;
        
        //Setting Class Parameters
        private Rigidbody rb;
        
        
        private void Start()
        {
            rb = characterExposer.characterRb;
            skillsMethods.Add("Dash", Dash);
        }

        private void Dash()
        {
            rb.MovePosition(rb.position + transform.TransformDirection(characterExposer._movedir) * characterExposer.dashSpeed * Time.deltaTime);
        }
    }
}
