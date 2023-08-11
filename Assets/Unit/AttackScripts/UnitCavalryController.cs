using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCavalryController : UnitAIController
{
    private bool update_canAttack;
 

    public override void SetUp()
    {
        this.transform.localPosition = new Vector3(0, 0.1f, -0.278f);
        this.transform.GetChild(0).localPosition = new Vector3(0, 0, 0);
        this.transform.localEulerAngles = new Vector3(90, -90, 0);
        anim = this.GetComponent<Animator>();
        unit = this.transform.parent.GetComponent<Unit>();
    }

    public void Update()
    {
        unit.AddChargeSpeed();

        if (unit.currentAgentSpeed > 1)
        {
            anim.SetBool("move", true);
        }
        else anim.SetBool("move", false);

        if (update_canAttack)
        {
            CheckAttackDirection();
            return;
        }

        if (unit.attackCD > 0) return;

        //Check can attack
        bool canAttack_InHitBox = VP.CheckHitShpere();
        if (!canAttack_InHitBox) return;

        //[Set Attack]
        if (canAttack_InHitBox)
        {
            update_canAttack = true;
            AI_Cav_Attack();
        }
    }

    public void AI_Cavalry_Action()
    {
        //[Stay]
        if (unit.attackTarget == null)
        {
            AI_Stay(true);
            return;
        }

        if (!CheckHoldStage()) { AI_Stay(true); return; }

        //[Find Enemy]
        if (unit.current_AI_Tactic == UnitData.AI_State_Tactic.attack)
        {
            //[Freeze after attack]
            if (unit.attackCD > unit.data.attackCD * 0.3f)
            {
                unit.agent.obstacleAvoidanceType = UnityEngine.AI.ObstacleAvoidanceType.NoObstacleAvoidance;

                Vector3 toPos = unit.transform.position + unit.transform.forward * 5f;
                unit.agent.SetDestination(toPos);

 
                bool canunitgo = unit.agent.CalculatePath(toPos, unit.agent.path);
                if(!canunitgo) unit.agent.SetDestination(unit.transform.position + unit.transform.forward * -10f);

            }

            else
            {
                float dis = Vector3.Distance(unit.transform.position, unit.attackTarget.transform.position);
                if (unit.agent.enabled)
                {
                    if (dis > 10f)
                    {
                        unit.agent.obstacleAvoidanceType = UnityEngine.AI.ObstacleAvoidanceType.MedQualityObstacleAvoidance;
                        unit.agent.SetDestination(unit.attackTarget.transform.position);
                    }

                    else if (dis > 4f)
                    {
                        unit.agent.obstacleAvoidanceType = UnityEngine.AI.ObstacleAvoidanceType.LowQualityObstacleAvoidance;
                        unit.agent.SetDestination(unit.attackTarget.transform.position);
                    }

                    else
                    {
                        unit.agent.obstacleAvoidanceType = UnityEngine.AI.ObstacleAvoidanceType.NoObstacleAvoidance;
                        unit.agent.SetDestination(unit.transform.position + unit.transform.forward * 6f);
        
                    }
                }
            }
        }
    }




    public void AI_Cav_Attack()
    {
        unit.agent.obstacleAvoidanceType = UnityEngine.AI.ObstacleAvoidanceType.NoObstacleAvoidance;
        unit.attackCD = unit.data.attackCD + Random.Range(-2, 2);

        if (unit.data.weaponCauseAOE)
           SetUpAttack(unit.data.damageMin, unit.data.damageMax, true);

        else
           SetUpAttack(unit.data.damageMin, unit.data.damageMax, false);
    }
    public override void SetUpAttack(int damMin, int damMax, bool causeAOE)
    {
        attackTrigger.InputData(this, damMin, damMax, causeAOE);
    }
    public void TriggerAttack(string myanim)
    {
        anim.SetTrigger(myanim);
        update_canAttack = false;
    }
    public void CheckAttackDirection()
    {
        if (update_canAttack != true) return;

        if (VP.CheckSphere("Middle"))
        {
            TriggerAttack("attack");
            return;
        }

        if (VP.CheckSphere("Right"))
        {
            TriggerAttack("attackR");
            return;
        }

        if (VP.CheckSphere("Left"))
        {
            TriggerAttack("attackL");
            return;
        }
    }

   
}
