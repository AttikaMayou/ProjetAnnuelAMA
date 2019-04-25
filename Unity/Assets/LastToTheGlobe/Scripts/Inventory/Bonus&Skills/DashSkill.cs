using System.Collections;
using System.Collections.Generic;
using LastToTheGlobe.Scripts.Inventory;
using UnityEngine;
using UnityEngine.UI;

public class DashSkill : Skills
{
    public Image icon;
    
    public override void SkillAction()
    {
        base.SkillAction();
        rb.MovePosition(rb.position + rb.transform.TransformDirection(characterExposer._movedir) * characterExposer.dashSpeed * Time.deltaTime);
    }
}
