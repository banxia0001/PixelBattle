using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class UnitAIController : MonoBehaviour
{
    public bool isSpineSkeleton = false;

    public AttackTrigger attackTrigger;
    [HideInInspector] public ViewPoint viewPoint;
    [HideInInspector] public Animator anim;
    [HideInInspector] public Unit unit;

    public virtual void Start()
    {
        unit = this.transform.parent.GetComponent<Unit>();
        viewPoint = this.transform.parent.GetComponent<ViewPoint>();
        anim = this.GetComponent<Animator>();
        unit = this.transform.parent.GetComponent<Unit>();
    }

    public virtual void LateUpdate()
    {
        this.transform.localEulerAngles = new Vector3(90, 0, 90);
        unit.canvas.transform.eulerAngles = new Vector3(0, 0, 0);
    }
    public virtual void SetUp()
    {
        unit = this.transform.parent.GetComponent<Unit>();
        this.transform.localPosition = new Vector3(0, 0.1f, -0.278f);
        this.transform.localEulerAngles = new Vector3(90, -90, 0);

        if (isSpineSkeleton) return;
        this.transform.GetChild(0).localPosition = new Vector3(0, 0, 0);
        anim = this.GetComponent<Animator>();
    }

    public virtual void SetUpAttack(Vector2Int damage, bool causeAP)
    {
        return;
    }

    public virtual void AI_DecideAttackTarget(Unit.UnitTeam targetTeam, bool targetFrontline)
    {
        int choice = Random.Range(0, 20);
        if (choice < 12) unit.attackTarget = AI_FindClosestTargetInList(unit.data.current_AI_Target, targetFrontline);
        else unit.attackTarget = AI_FindClosestTargetInList(unit.data.current_AI_Target_Secondly, targetFrontline);

        if (unit.attackTarget == null)
        {
            if (choice < 12) unit.attackTarget = AI_FindClosestTargetInList(unit.data.current_AI_Target_Secondly, targetFrontline);
            else unit.attackTarget = AI_FindClosestTargetInList(unit.data.current_AI_Target, targetFrontline);
        }

        if (unit.attackTarget == null)
        {
            unit.attackTarget = AIFunctions.AI_Find_ClosestUnit(targetTeam, unit, false, targetFrontline);
        }
    }

    public Unit AI_FindClosestTargetInList(UnitData.AI_State_FindTarget current_AI_Target, bool targrtFrontLine)
    {
        Unit.UnitTeam targetTeam = Unit.UnitTeam.teamB;
        if (unit.unitTeam == Unit.UnitTeam.teamB) targetTeam = Unit.UnitTeam.teamA;

        if (current_AI_Target == UnitData.AI_State_FindTarget.findValueableTarget_InDistance)
        {
            return AIFunctions.AI_Find_ValueableUnit(targetTeam, unit, false);
        }

        if (current_AI_Target == UnitData.AI_State_FindTarget.findValueableTarget_InFrontline)
        {
            return AIFunctions.AI_Find_ValueableUnit(targetTeam, unit, true);
        }

        if (current_AI_Target == UnitData.AI_State_FindTarget.findClosest)
        {
            return AIFunctions.AI_Find_ClosestUnit(targetTeam, unit, true, true);
        }

        if (current_AI_Target == UnitData.AI_State_FindTarget.findClosestWarrior || current_AI_Target == UnitData.AI_State_FindTarget.findClosestTarget_InFrontline)
        {
            return AIFunctions.AI_Find_ClosestUnit_2(targetTeam, current_AI_Target, unit, true);
        }

        else
        {
            return AIFunctions.AI_Find_ClosestUnit_2(targetTeam, current_AI_Target, unit, false);
        }
    }

    public virtual void FindAttackTarget()
    {
        Unit.UnitTeam targetTeam = Unit.UnitTeam.teamB;
        if (unit.unitTeam == Unit.UnitTeam.teamB) targetTeam = Unit.UnitTeam.teamA;

        bool focusFrontline = false;

        if (unit.data.unitType == UnitData.UnitType.archer ||
            unit.data.unitType == UnitData.UnitType.artillery ||
            unit.data.unitType == UnitData.UnitType.cavalry) focusFrontline = false;

        else focusFrontline = true;

        //[If target empty]
        if (unit.attackTarget == null)
        {
            AI_DecideAttackTarget(targetTeam, focusFrontline);
        }
        //[See if there are new unit that are closer]
        else
        {
            float dis = Vector3.Distance(this.transform.position, unit.attackTarget.transform.position);
            if (unit.data.unitType == UnitData.UnitType.infantry)
            {
                if (dis > 2.5f) AI_DecideAttackTarget(targetTeam, focusFrontline);
            }

            else if (unit.data.unitType == UnitData.UnitType.archer || unit.data.unitType == UnitData.UnitType.artillery)
            {
                if (dis > unit.data.shootDis * 1.2f) AI_DecideAttackTarget(targetTeam, focusFrontline);
            }

            else if (unit.data.unitType == UnitData.UnitType.cavalry)
            {
                if (dis > 5f) AI_DecideAttackTarget(targetTeam, focusFrontline);
            }

            else
            {
                if (dis > 3f) unit.attackTarget = AI_FindClosestTargetInList(unit.data.current_AI_Target, false);
            }
        }
    }
   

    public virtual void AI_Stay(bool minorMove)
    {
        //Debug.Log("Stay");
        if (unit.agent.enabled)
        {
            if (minorMove)
                unit.agent.SetDestination(unit.transform.position + new Vector3(Random.Range(-0.13f, 0.13f), 0, Random.Range(-0.13f, 0.13f)));

            else
                unit.agent.SetDestination(unit.transform.position);
        }
    }

    public virtual void AI_GoToEnemyBase(Unit.UnitTeam team)
    {
        //Debug.Log("Stay");
        if (unit.agent.enabled)
        {
            if(team == Unit.UnitTeam.teamA)
                unit.agent.SetDestination(unit.transform.position + new Vector3(10,0,0));

            else
                unit.agent.SetDestination(unit.transform.position + new Vector3(-10, 0, 0));
        }
    }

    public virtual void AI_GoToBase(Unit.UnitTeam team)
    {
        //Debug.Log("Stay");
        if (unit.agent.enabled)
        {
            if (team == Unit.UnitTeam.teamA)
                unit.agent.SetDestination(unit.transform.position + new Vector3(-10, 0, 0));

            else
                unit.agent.SetDestination(unit.transform.position + new Vector3(10, 0, 0));
        }
    }
    public virtual void AI_MoveToward(Transform trans)
    {
        if (unit.agent.enabled)
            unit.agent.SetDestination(trans.position + new Vector3(Random.Range(-0.1f, 0.1f), 0, Random.Range(-0.1f, 0.1f)));
    }

    public virtual void AI_MoveToward_WithAI(Transform trans)
    {
        if (!unit.agent.enabled) return;
        bool smarterMove = viewPoint.CheckHitShpere_If_BlockedByFrienlyUnit(1.2f, 3);
        if (smarterMove)
        {
            unit.agent.obstacleAvoidanceType = UnityEngine.AI.ObstacleAvoidanceType.HighQualityObstacleAvoidance;
        }

        else
        {
            unit.agent.obstacleAvoidanceType = UnityEngine.AI.ObstacleAvoidanceType.LowQualityObstacleAvoidance;
        }

        unit.agent.SetDestination(trans.position + new Vector3(Random.Range(-0.1f, 0.1f), 0, Random.Range(-0.1f, 0.1f)));
    }
    public virtual void AI_LookAt(Transform pos)
    {
        unit.agent.enabled = false;
        unit.transform.LookAt(pos, Vector3.up);
        unit.agent.enabled = true;
    }
    public virtual void AI_Flee()
    {
        if (unit.agent.enabled)
        {
            float xValue = 10f;
            if (unit.unitTeam == Unit.UnitTeam.teamB) xValue = 10f;
            if (unit.unitTeam == Unit.UnitTeam.teamA) xValue = -10f;
            Vector3 newPos = unit.transform.position + new Vector3(xValue + Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));

            unit.agent.SetDestination(newPos);
        }
    }
  
    public virtual void AI_MoveForward()
    {
        if (unit.agent.enabled)
            unit.agent.SetDestination(unit.transform.position + unit.transform.forward * 3f);
    }

    public virtual void AI_MoveBackward()
    {
        if (unit.agent.enabled)
            unit.agent.SetDestination(unit.transform.position - unit.transform.forward * 3f);
    }
}
