using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Unit : MonoBehaviour
{
    [Header("UnitData")]
    public UnitData data;

    [Header("Movement")]
    public Unit moveTarget;
    public GameObject unitSprite;
    private UnityEngine.AI.NavMeshAgent agent;
    private Rigidbody rb;

    public TMP_Text text;
    public SpriteRenderer render;

    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        moveTarget = null;

        if (data.unitTeam == UnitData.UnitTeam.teamA) text.text = "A";
        if (data.unitTeam == UnitData.UnitTeam.teamB) text.text = "B";

        if (data.unitType == UnitData.UnitType.cavalry) render.color = Color.blue;
        if (data.unitType == UnitData.UnitType.archer) render.color = Color.green;
        if (data.unitType == UnitData.UnitType.monster) render.color = Color.red;
        if (data.unitType == UnitData.UnitType.warrior) render.color = Color.white;


    }
    void FixedUpdate()
    {


        //if (Input.GetKeyDown(KeyCode.A))
        //{
        //    agent.enabled = false;
        //    BattleFunction.Add_KnockBack_ToDefender(moveTarget.transform, this.gameObject.transform, rb,30f);
        //}
    }
    void LateUpdate()
    {
        unitSprite.transform.eulerAngles = new Vector3(0, 0, 0);
    }


    public void AI_DecideAction()
    {
        if (moveTarget == null)
        {
            AI_FindTarget();
        }

        if (moveTarget != null)
        {
            if (agent.enabled)
            {
                float dis = Vector3.Distance(this.transform.position, moveTarget.transform.position);
                if (dis < data.attackDis)
                {
                    agent.SetDestination(this.transform.position);
                }
                else agent.SetDestination(moveTarget.transform.position);
            }
        }
    }

    private void AI_FindTarget()
    {
        UnitData.UnitTeam targetTeam = UnitData.UnitTeam.teamB;
        if (data.unitTeam == UnitData.UnitTeam.teamB) targetTeam = UnitData.UnitTeam.teamA;

        if (data.current_AI_Target == UnitData.AI_State_FindTarget.findClosest)
        {
            moveTarget = AIFunctions.AI_Find_ClosestUnit(targetTeam, this);
        }
        if (data.current_AI_Target == UnitData.AI_State_FindTarget.findWarrior)
        {
            moveTarget = AIFunctions.AI_Find_ClosestUnit(targetTeam, UnitData.UnitType.warrior, this);
        }
        if (data.current_AI_Target == UnitData.AI_State_FindTarget.findArcher)
        {
            moveTarget = AIFunctions.AI_Find_ClosestUnit(targetTeam, UnitData.UnitType.archer, this);
        }
        if (data.current_AI_Target == UnitData.AI_State_FindTarget.findCavalry)
        {
            moveTarget = AIFunctions.AI_Find_ClosestUnit(targetTeam, UnitData.UnitType.cavalry, this);
        }
        if (data.current_AI_Target == UnitData.AI_State_FindTarget.findMonster)
        {
            moveTarget = AIFunctions.AI_Find_ClosestUnit(targetTeam, UnitData.UnitType.monster, this);
        }
        if (moveTarget == null)
        {
            moveTarget = AIFunctions.AI_Find_ClosestUnit(targetTeam, this);
        }
    }
}
