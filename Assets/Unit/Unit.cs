using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Unit : MonoBehaviour
{
    [Header("UnitData")]
    public UnitData data;
    public int health;
    public int attackCD;

    [Header("Movement")]
    public Unit moveTarget;
    public GameObject unitSprite;
    private UnityEngine.AI.NavMeshAgent agent;
    private Rigidbody rb;



    [Header("UI")]
    public BarController BC;
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

        InputData(data);
    }

    public void InputData(UnitData data)
    {
        this.data = data;
        health = data.health;
        attackCD = 0;
        BC.SetValue_Initial(health, data.health);
    }

    void FixedUpdate()
    {
        agent.speed = data.moveSpeed;
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


    public void AI_Attack()
    {
        //[Attack]
        attackCD = data.attackCD;
        BattleFunction.Attack(this.gameObject.transform, data.damage, moveTarget);
        //[Set Pos]
        agent.SetDestination(moveTarget.transform.position + new Vector3(Random.Range(-0.1f, 0.1f), 0, Random.Range(-0.1f, 0.1f)));
    }
    public void AI_Shoot()
    {

        //[Shoot Arrow]
        attackCD = data.attackCD;

        UnitData.UnitTeam targetTeam = UnitData.UnitTeam.teamB;
        if (data.unitTeam == UnitData.UnitTeam.teamB) targetTeam = UnitData.UnitTeam.teamA;
       
        GameObject Arrow = Instantiate(Resources.Load<GameObject>("VFX/ArrowPrefab"), this.transform.position, Quaternion.identity);
        Projectile ArrowScript = Arrow.GetComponent<Projectile>();
        Vector3 targetPos = moveTarget.transform.position + new Vector3(Random.Range(-data.arrowOffset, data.arrowOffset),0, Random.Range(-data.arrowOffset, data.arrowOffset));
        ArrowScript.SetUpArror(targetPos, data.damage, data.arrowSpeed, targetTeam);

        //[Set Pos]
        agent.SetDestination(moveTarget.transform.position + new Vector3(Random.Range(-0.1f, 0.1f), 0, Random.Range(-0.1f, 0.1f)));
    }
    public void GetDamage(int dam)
    {
        health -= dam;
        BC.SetValue(health,data.health);
        if (health <= 0)
        {
            Destroy(this.gameObject);
            //Anim.Set death;
        }
    }

    public void AI_DecideAction(bool holdStageEnd)
    {
        attackCD--;

        //[If target empty]
        if (moveTarget == null)
        {
            AI_FindTarget();
        }

        //[See if there are new unit that are closer]
        else
        {
            float dis = Vector3.Distance(this.transform.position, moveTarget.transform.position);
            if (dis > data.attackDis)
            {
                AI_FindTarget();
            }
        }

        if (moveTarget != null)
        {
            //If unit are not knock backed.
            if (agent.enabled)
            {
                float dis = Vector3.Distance(this.transform.position, moveTarget.transform.position);

                //If unit can attack
                if (dis < data.attackDis)
                {
                    //[Warrior]
                    if (data.unitType == UnitData.UnitType.warrior)
                    {
                        AI_WarriorAttack();
                    }

                    //[Archer]
                    if (data.unitType == UnitData.UnitType.archer)
                    {
                        AI_ArcherAttack(dis);
                    }
                }

                //Normal Moving
                else
                {
                    agent.SetDestination(moveTarget.transform.position + new Vector3(Random.Range(-0.1f,0.1f),0, Random.Range(-0.1f, 0.1f)));
                }

                //else
                //{
                //    agent.SetDestination(this.transform.position);
                //}      
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


    private void AI_WarriorAttack()
    {
        AI_Attack();
    }
    private void AI_ArcherAttack(float dis)
    {
        if (data.current_AI_Tactic == UnitData.AI_State_Tactic.shoot_n_flee)
        {
            if (dis < data.attackDis / 0.4f)
            {
                AI_Flee();
            }
            if (attackCD <= 0)
            {
                AI_Shoot();
            }
        }

        if (data.current_AI_Tactic == UnitData.AI_State_Tactic.shoot && attackCD <= 0)
        {
            AI_Shoot();
        }
        if (data.current_AI_Tactic == UnitData.AI_State_Tactic.hold_n_shoot && attackCD <= 0)
        {
            AI_Shoot();
        }
    }



    public void AI_ArcherAction(UnitData.AI_State_Tactic tactic, Unit targetAttacker, Unit targetDefender, bool isHold)
    {

    }


    private void AI_Flee()
    {
        float xValue = 10f;
        if(data.unitTeam == UnitData.UnitTeam.teamB) xValue = 10f;
        if(data.unitTeam == UnitData.UnitTeam.teamA) xValue = -10f;
        Vector3 newPos = this.transform.position + new Vector3(xValue + Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
        agent.SetDestination(newPos);
    }
}
