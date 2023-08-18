using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSpearmanController : UnitAIController
{
    public void AI_Warrior_Action(bool dontChangeAttackTarget)
    {

        unit.agent.obstacleAvoidanceType = UnityEngine.AI.ObstacleAvoidanceType.LowQualityObstacleAvoidance;

        //[FindEnemy]
        if(!dontChangeAttackTarget)
        FindAttackTarget();

        //[Stay]
        if (unit.attackTarget == null)
        {
            AI_GoToEnemyBase(unit.unitTeam);
            return;
        }

        Unit closestCav = AI_FindClosestTargetInList(UnitData.AI_State_FindTarget.findClosestCavalry, false);
        if (closestCav != null)
        {
            float dis_CavAlerm = Vector3.Distance(unit.transform.position, closestCav.transform.position);
            if (dis_CavAlerm < 4f)
            {
                unit.attackTarget = closestCav;
                AI_MoveToward(unit.attackTarget.gameObject.transform);
            }
        }

        float dis = Vector3.Distance(unit.transform.position, unit.attackTarget.transform.position);
        bool canAttack = false;

        if (dis < VP.radius_EyeRough * 4f)
        {
            canAttack = VP.CheckHitShpere(1);
        }

        //[Set Attack]
        if (unit.attackCD <= 0 && canAttack)
        {
            unit.SetChargeSpeed(0);
            AI_Stay(true);
        }


        //[Find Enemy]
        else
        {
            if (unit.data.current_AI_Tactic == UnitData.AI_State_Tactic.attack)
            {
                if (dis < unit.data.moveStopDis) AI_Stay(false);

                else AI_MoveToward(unit.attackTarget.gameObject.transform);
            }
        }
    }


    public void Update()
    {
        if (unit.attackCD > 0) return;

        if (unit.attackTarget != null)
        {
            float dis = Vector3.Distance(unit.transform.position, unit.attackTarget.transform.position);

            //Check can attack
            bool canAttack_InHitBox = VP.CheckHitShpere(1);


            if (dis < 3f && canAttack_InHitBox)
            {
                
                unit.attackCD = unit.data.attackCD + Random.Range(-1, 1);
                SetUpAttack(unit.data.damageMin, unit.data.damageMax, unit.data.weaponCauseAOE, unit.data.isAP);
            }
        }

    }

    public override void SetUpAttack(int damMin, int damMax, bool causeAOE, bool causeAP)
    {
        attackTrigger.InputData(this, damMin, damMax, causeAOE, causeAP);

        //if (VP.CheckSphere("Right"))
        //{
        //    AI_LookAt(VP.eyeR.transform);
        //    anim.SetTrigger("attack");
        //    return;
        //}

        //if (VP.CheckSphere("Left"))
        //{
        //    AI_LookAt(VP.eyeL.transform);
        //    anim.SetTrigger("attack");
        //    return;
        //}

        anim.SetTrigger("attack");
    }
}
