using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Infantry : UnitAIController
{
    public bool use3DirVP;
    public void AI_Warrior_Action(bool remainAttackTarget)
    {
        unit.agent.obstacleAvoidanceType = UnityEngine.AI.ObstacleAvoidanceType.LowQualityObstacleAvoidance;

        //[FindTarget]
        if(!remainAttackTarget)
        FindAttackTarget();

        //[Stay]
        if (unit.attackTarget == null)
        {
            AI_GoToEnemyBase(unit.unitTeam);
            return;
        }

        //[Distance]
        float dis = Vector3.Distance(unit.transform.position, unit.attackTarget.transform.position);
        bool canAttack = false;

        if (unit.attackTarget.data.unitType == UnitData.UnitType.cavalry)
        {
            if (dis > viewPoint.radius_EyeRough * 3f)
            {
                //AI_GoToConquerLand(-1.1f);
                AI_GoToEnemyBase(unit.unitTeam);
                return;
            }
        }
        if (dis < viewPoint.radius_EyeRough * 4f)
        {
            canAttack = viewPoint.CheckHitShpere(1);
        }


        //[Set Attack]
        if (unit.attackCD <= 0 && canAttack)
        {
            unit.attackCD = unit.data.attackCD + Random.Range(-1, 1);
            SetUpAttack(unit.data.damage, unit.data.isAP);
            unit.SetChargeSpeed(0);
            AI_Stay(true);
        }

        //[Find Enemy]
        else
        {
            if (unit.data.current_AI_Tactic == UnitData.AI_State_Tactic.attack)
            {
                if (dis < unit.data.moveStopDis) AI_Stay(false);

                else if(dis < viewPoint.radius_EyeRough * 4f)
                {

                    AI_MoveToward_WithAI(unit.attackTarget.gameObject.transform);
                }

                else AI_MoveToward(unit.attackTarget.gameObject.transform);

            }
        }
    }

    public override void SetUpAttack(Vector2Int damage,  bool causeAP)
    {
        attackTrigger.InputData(this, damage, causeAP);

        if (use3DirVP)
        {
            if (unit.data_local.ID == UnitData.UnitListID.SpearKnight)
            {
                if (viewPoint.CheckSphere("Right") != 0)
                {
                    this.unit.agent.enabled = false;
                    this.unit.transform.eulerAngles += new Vector3(0, -30, 0);
                    this.unit.agent.enabled = true;
                    anim.SetTrigger("attack");
                    return;
                }

                if (viewPoint.CheckSphere("Left") != 0)
                {
                    this.unit.agent.enabled = false;
                    this.unit.transform.eulerAngles += new Vector3(0, 30, 0);
                    this.unit.agent.enabled = true;
                    anim.SetTrigger("attack");
                    return;
                }

                else anim.SetTrigger("attack");
            }
        }

        else
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
        }
    }
}
