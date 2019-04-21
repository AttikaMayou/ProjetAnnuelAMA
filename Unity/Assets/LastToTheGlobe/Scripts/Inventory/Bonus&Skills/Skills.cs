using UnityEngine;

//Auteur : Attika

namespace LastToTheGlobe.Scripts.Inventory
{
    public class Skills : ObjectScript
    {
        public bool Dash(Rigidbody rb, Vector3 _moveDir, float dashSpeed, bool _dashAsked)
        {
            rb.MovePosition(rb.position + transform.TransformDirection(_moveDir) * dashSpeed * Time.deltaTime);
            _dashAsked = false;
            Debug.Log("DashAsked");
            return _dashAsked;
        }
    }
}
