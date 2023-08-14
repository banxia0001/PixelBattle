using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAIController : MonoBehaviour
{
    public AttackTrigger attackTrigger;
    [HideInInspector]
    public ViewPoint VP;
    [HideInInspector]
    public Animator anim;
    [HideInInspector]
    public Unit unit;


    public virtual void Start()
    {
        unit = this.transform.parent.GetComponent<Unit>();
        VP = this.transform.parent.GetComponent<ViewPoint>();
        anim = this.GetComponent<Animator>();
        unit = this.transform.parent.GetComponent<Unit>();
    }
    public virtual void SetUp()
    {
        this.transform.localPosition = new Vector3(0, 0.1f, -0.278f);
        this.transform.GetChild(0).localPosition = new Vector3(0, 0, 0);
        this.transform.localEulerAngles = new Vector3(90, -90, 0);
        anim = this.GetComponent<Animator>();
        unit = this.transform.parent.GetComponent<Unit>();
    }

    public virtual void SetUpAttack(int damMin, int damMax, bool causeAOE, bool causeAP)
    {
        return;
    }

    public virtual bool CheckHoldStage()
    {
        int stateNum = (int)GameController.waitState;

        if (unit.current_AI_Wait == UnitData.AI_State_Wait.advance) return true;

        if (unit.current_AI_Wait == UnitData.AI_State_Wait.hold5s && stateNum >= 1) return true;

        if (unit.current_AI_Wait == UnitData.AI_State_Wait.hold10S && stateNum >= 2) return true;

        if (unit.current_AI_Wait == UnitData.AI_State_Wait.hold15S && stateNum >= 3) return true;

        //Debug.Log(stateNum);
        return false;
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
    public virtual void AI_MoveToward(Transform trans)
    {
        if (unit.agent.enabled)
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
    public virtual bool AI_CheckIfShouldFlee()
    {
        Unit.UnitTeam targetTeam = Unit.UnitTeam.teamB;
        if (unit.unitTeam == Unit.UnitTeam.teamB) targetTeam = Unit.UnitTeam.teamA;

        List<Unit> unitList = BattleFunction.Find_UnitsInRange(targetTeam, unit.data.shootDis * 0.2f, unit.transform);
        if (unitList != null)
        {
            if (unitList.Count != 0)
            {
                unit.attackTarget = BattleFunction.Find_ClosestUnitInList(unitList, unit.transform);
                //Debug.Log("moveTarget:" + attackTarget.name);
                return true;
            }
            else return false;
        }
        else return false;
    }

    public virtual void AI_MoveForward()
    {
        if (unit.agent.enabled)
            unit.agent.SetDestination(unit.transform.position + unit.transform.forward * 3f);
    }
 
}
