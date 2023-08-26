using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitInfantryController : UnitAIController
{
    public bool use3DirVP;
    public void AI_Warrior_Action(bool dontChangeAttackTarget)
    {
        unit.agent.obstacleAvoidanceType = UnityEngine.AI.ObstacleAvoidanceType.LowQualityObstacleAvoidance;

        //[FindTarget]
        if(!dontChangeAttackTarget)
        FindAttackTarget();

        //[Stay]
        if (unit.attackTarget == null)
        {
            //AI_GoToConquerLand(-1.1f);
            AI_GoToEnemyBase(unit.unitTeam);
            return;
        }

        float dis = Vector3.Distance(unit.transform.position, unit.attackTarget.transform.position);
        bool canAttack = false;

        //if (!VP.CheckHitShpere(3))
        //{
        //    AI_GoToConquerLand(-1.1f);
        //    return;
        //}

        if (unit.attackTarget.data.unitType == UnitData.UnitType.cavalry)
        {
            if (dis > VP.radius_EyeRough * 3f)
            {
                //AI_GoToConquerLand(-1.1f);
                AI_GoToEnemyBase(unit.unitTeam);
                return;
            }
        }

        if (dis < VP.radius_EyeRough * 4f)
        {
            canAttack = VP.CheckHitShpere(1);
        }

        //[Set Attack]
        if (unit.attackCD <= 0 && canAttack)
        {
            unit.attackCD = unit.data.attackCD + Random.Range(-1, 1);
            SetUpAttack(unit.data.damageMin, unit.data.damageMax, unit.data.weaponCauseAOE, unit.data.isAP);
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

    public override void SetUpAttack(int damMin, int damMax, bool causeAOE, bool causeAP)
    {
        attackTrigger.InputData(this, damMin, damMax, causeAOE, causeAP);

        if (use3DirVP)
        {
            if (unit.data_local.ID == UnitData.UnitListID.SpearKnight)
            {
                if (VP.CheckSphere("Right") != 0)
                {
                    Debug.Log("!!R");
                    this.unit.agent.enabled = false;
                    this.unit.transform.eulerAngles += new Vector3(0, -30, 0);
                    this.unit.agent.enabled = true;
                    anim.SetTrigger("attack");
                    return;
                }

                if (VP.CheckSphere("Left") != 0)
                {
                    Debug.Log("!!L");
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
