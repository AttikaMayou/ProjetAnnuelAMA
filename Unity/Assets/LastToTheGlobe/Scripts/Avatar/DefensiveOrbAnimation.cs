//Auteur : Abdallah
//Modification : Attika

//TODO : refacto this in another script 

using Assets.LastToTheGlobe.Scripts.Weapon.Orb;
using LastToTheGlobe.Scripts.Weapon.Orb;

namespace Assets.LastToTheGlobe.Scripts.Avatar
{
    public class DefensiveOrbAnimation : OrbExposerScript
    {
        /*[SerializeField] private CharacterExposer playerExposer;
        [SerializeField] private float speed = 1f;

        private float _actualTime = 0f;
        
        private void Update()
        {
            _actualTime += Time.deltaTime;
            _actualTime %= (Mathf.PI * 2);
            orbTransform.RotateAround(playerExposer.characterTransform.position,
                                        playerExposer.characterTransform.up, speed);
            orbTransform.Translate(new Vector3(0,Mathf.Cos(_actualTime) * 
                                                 Time.deltaTime/2,0),Space.Self);
        }*/
    }
}
