using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonMonster : UnitAIController
{
    public AttackTrigger attackTrigger_Hand2;
    public AttackTrigger attackTrigger_Body;
    public bool isAttacking;
    public override void SetUp()
    {
        this.transform.localPosition = new Vector3(0, 0.1f, -0.278f);
        this.transform.GetChild(0).localPosition = new Vector3(0, 0, 0);
        this.transform.localEulerAngles = new Vector3(90, -90, 0);
        anim = this.GetComponent<Animator>();
        unit = this.transform.parent.GetComponent<Unit>();
    }


    public void AI_Monster_Action(bool dontChangeAttackTarget)
    {
        if (isAttacking)
        {
            AI_MoveForward();
            return;
        } 

        if (unit.currentAgentSpeed > 1)
        {
            anim.SetBool("move", true);
        }
        else anim.SetBool("move", false);

        unit.agent.obstacleAvoidanceType = UnityEngine.AI.ObstacleAvoidanceType.LowQualityObstacleAvoidance;

        //[FindTarget]
        if(!dontChangeAttackTarget)
        FindAttackTarget();

        //[Stay]
        if (unit.attackTarget == null)
        {
            AI_GoToEnemyBase(unit.unitTeam);
            return;
        }

        bool canAttack = viewPoint.CheckHitShpere(1);

        if (unit.attackCD <= 0 && canAttack)
        {
            isAttacking = true;
            attackTrigger.InputData(this, unit.data.damageMin, unit.data.damageMax, unit.data.weaponAOENum, unit.data.isAP);
            attackTrigger_Hand2.InputData(this, unit.data.damageMin, unit.data.damageMax, unit.data.weaponAOENum, unit.data.isAP);
            attackTrigger_Body.InputData(this, unit.data.damageMin, unit.data.damageMax, unit.data.weaponAOENum, unit.data.isAP);


            unit.attackCD = unit.data.attackCD + Random.Range(-2, 2);

            int ran = Random.Range(0, 3);
            if (ran == 0) anim.SetTrigger("attack");

            if (ran == 1)
            {
                anim.SetTrigger("attack2");
                AI_LookAt(unit.attackTarget.transform);
            }

            if (ran == 2)
            {
                anim.SetTrigger("attackN");
                AI_LookAt(unit.attackTarget.transform);
            }
        }


        //[Find Enemy]
        else
        {
            if (unit.data.current_AI_Tactic == UnitData.AI_State_Tactic.attack)
            {
                AI_MoveToward(unit.attackTarget.gameObject.transform);
            }
        }
    }

    public void Anim_ChargeAttack(int force)
    {
        unit.agent.enabled = false;
        unit.rb.velocity = Vector3.zero;
        unit.knockBackTimer = 0.23f;
        unit.rb.freezeRotation = true;
        unit.GetComponent<CapsuleCollider>().isTrigger = true;
        unit.rb.AddForce(force * unit.transform.forward, ForceMode.Impulse);
    }

    public void Anim_LookAtTarget()
    {
        Unit.UnitTeam targetTeam = Unit.UnitTeam.teamB;
        if (unit.unitTeam == Unit.UnitTeam.teamB) targetTeam = Unit.UnitTeam.teamA;
        unit.attackTarget = AIFunctions.AI_Find_ClosestUnit(targetTeam, unit, false, true);

        if (unit.attackTarget != null)
        {
            AI_MoveToward(unit.attackTarget.transform);
        }
    }

    public void Anim_AttackingEnd()
    {
        isAttacking = false;
    }
}
   

