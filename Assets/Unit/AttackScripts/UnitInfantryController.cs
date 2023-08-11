using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitInfantryController : UnitAIController
{
    public bool use3DirVP;
    public void AI_Warrior_Action()
    {
        unit.agent.obstacleAvoidanceType = UnityEngine.AI.ObstacleAvoidanceType.LowQualityObstacleAvoidance;

        //[Stay]
        if (unit.attackTarget == null)
        {
            AI_Stay(true);
            return;
        }

        float dis = Vector3.Distance(unit.transform.position, unit.attackTarget.transform.position);
        bool canAttack = false;
        if (dis < VP.radius_EyeRough * 4f)
        {
            canAttack = VP.CheckHitShpere();
        }

        //[Set Attack]
        if (unit.attackCD <= 0 && canAttack)
        {
            unit.attackCD = unit.data.attackCD + Random.Range(-1, 1);
            SetUpAttack(unit.data.damageMin, unit.data.damageMax, unit.data.weaponCauseAOE);
            unit.SetChargeSpeed(0);
            AI_Stay(true);
        }

        //[wait]
        if (!CheckHoldStage()) { AI_Stay(true); return; }

        //[Find Enemy]
        else
        {
            if (unit.current_AI_Tactic == UnitData.AI_State_Tactic.attack)
            {
                AI_MoveToward(unit.attackTarget.gameObject.transform);
            }
        }
    }

    public override void SetUpAttack(int damMin, int damMax, bool causeAOE)
    {
        int ran = Random.Range(0, 2);
        if (ran == 0) anim.SetTrigger("attack");


        if (ran == 1)
        {
            anim.SetTrigger("attack2");
            if (unit.data_local.ID == UnitData.UnitListID.FootKnight)
            {
                AI_LookAt(unit.attackTarget.transform);
            }
        }
           
        attackTrigger.InputData(this, damMin, damMax, causeAOE);
    }
}
